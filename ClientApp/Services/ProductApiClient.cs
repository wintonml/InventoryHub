using System.Net.Http.Json;
using System.Text.Json;
using Shared.Constants;
using Shared.Models;

namespace ClientApp.Services;

public class ProductApiClient : IProductApiClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly SemaphoreSlim CacheLock = new(1, 1);
    private static Product[]? CachedProducts;
    private static DateTimeOffset? CacheExpiresAt;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product[]> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        if (CachedProducts is not null && CacheExpiresAt is not null && DateTimeOffset.UtcNow < CacheExpiresAt.Value)
        {
            return CachedProducts;
        }

        await CacheLock.WaitAsync(cancellationToken);

        try
        {
            if (CachedProducts is not null && CacheExpiresAt is not null && DateTimeOffset.UtcNow < CacheExpiresAt.Value)
            {
                return CachedProducts;
            }

            var products = await FetchProductsAsync(cancellationToken);
            CachedProducts = products;
            CacheExpiresAt = DateTimeOffset.UtcNow.Add(CacheDuration);

            return products;
        }
        finally
        {
            CacheLock.Release();
        }
    }

    private async Task<Product[]> FetchProductsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));

            var products = await _httpClient.GetFromJsonAsync<Product[]>(
                ApiEndpoints.ProductList,
                SerializerOptions,
                timeoutCts.Token);

            return products ?? [];
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException(ApiErrorMessages.RequestTimeout);
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException(ApiErrorMessages.ServerUnreachable);
        }
        catch (JsonException)
        {
            throw new InvalidOperationException(ApiErrorMessages.InvalidResponse);
        }
    }
}
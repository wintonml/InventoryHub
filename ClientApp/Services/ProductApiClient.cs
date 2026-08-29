using System.Net.Http.Json;
using System.Text.Json;
using Shared.Constants;
using Shared.Models;

namespace ClientApp.Services;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product[]> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));

            var response = await _httpClient.GetAsync(ApiEndpoints.Products, timeoutCts.Token);

            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<Product[]>(
                SerializerOptions,
                timeoutCts.Token);

            return products ?? [];
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException("The request timed out while loading products.");
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Unable to reach the products API.");
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("The API returned invalid product data.");
        }
    }
}
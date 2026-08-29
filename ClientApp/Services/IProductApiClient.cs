using Shared.Models;

namespace ClientApp.Services
{
    public interface IProductApiClient
    {
        Task<Product[]> GetProductsAsync(CancellationToken cancellationToken = default);
    }
}


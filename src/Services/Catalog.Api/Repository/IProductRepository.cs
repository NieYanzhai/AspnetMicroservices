using System.Collections.Generic;
using Catalog.Api.Entites;
using System.Threading.Tasks;

namespace Catalog.Api.Repository
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<bool> DeleteProduct(string id);
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCatalogAsync(string catalog);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<bool> UpdateProductAsync(Product product);
    }
}
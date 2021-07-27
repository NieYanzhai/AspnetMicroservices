using System.Collections.Generic;
using Catalog.Api.Data;
using Catalog.Api.Entites;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IMongoCollection<Product> products;

        public ProductRepository(ICatalogContext catalogContext)
        {
            products = catalogContext.Products;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await (await this.products.FindAsync(p => true))
                        .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await (await this.products.FindAsync(filter: p => p.Id == id))
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await (await this.products.FindAsync(filter: p => p.Name == name))
                    .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCatalogAsync(string catalog)
        {
            return await (await this.products.FindAsync(filter: p => p.Category == catalog))
                    .ToListAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await this.products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            // var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            var result = await this.products.ReplaceOneAsync(
                            filter: p => p.Id == product.Id,
                            replacement: product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var result = await this.products.DeleteOneAsync(filter: p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
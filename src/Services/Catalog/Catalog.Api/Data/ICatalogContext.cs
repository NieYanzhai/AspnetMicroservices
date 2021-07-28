using Catalog.Api.Entites;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public interface ICatalogContext
    {
        IMongoDatabase CatalogDB { get; }
        IMongoCollection<Product> Products { get; }
    }
}
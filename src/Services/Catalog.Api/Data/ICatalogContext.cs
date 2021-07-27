using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class ICatalogContext
    {
        IMongoDatabase CatalogDB { get; }
    }
}
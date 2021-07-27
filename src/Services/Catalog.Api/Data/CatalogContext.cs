using System.Threading.Tasks;
using Catalog.Api.Entites;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration configuration;

        public CatalogContext(IConfiguration configuration)
        {
            this.configuration = configuration;

            var mongoClient = new MongoClient(configuration.GetValue<string>("MongoSettings:ConnectionString"));
            CatalogDB = mongoClient.GetDatabase(configuration.GetValue<string>("MongoSettings:DatabaseName"));

            var products = this.CatalogDB.GetCollection<Product>(configuration.GetValue<string>("MongoSettings:CollectionName"));
            MongoContextSeet.SeedData(products);
        }

        public IMongoDatabase CatalogDB { get; }
    }
}
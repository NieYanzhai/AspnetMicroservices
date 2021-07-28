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
            this.CatalogDB = mongoClient.GetDatabase(configuration.GetValue<string>("MongoSettings:DatabaseName"));
            this.Products = this.CatalogDB.GetCollection<Product>(configuration.GetValue<string>("MongoSettings:CollectionName"));

            CatalogContextSeed.SeedData(this.Products);
        }

        public IMongoDatabase CatalogDB { get; }

        public IMongoCollection<Product> Products {get;}
    }
}
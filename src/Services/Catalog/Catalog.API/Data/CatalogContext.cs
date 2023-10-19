using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            MongoClient mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = mongoDatabase.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }
        public IMongoCollection<Product> Products { get; }
    }
}

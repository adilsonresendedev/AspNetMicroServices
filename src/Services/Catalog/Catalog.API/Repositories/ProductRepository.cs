using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task CreateProductAsync(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Id, id);

            DeleteResult result = await _catalogContext
                .Products
                .DeleteOneAsync(filterDefinition);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Id, id);
            Product product = await _catalogContext
                .Products
                .Find(filterDefinition)
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryNameAsync(string categoryName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Category, categoryName);
            return await _catalogContext
                .Products
                .Find(filterDefinition)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Name, name);
            return await _catalogContext
                .Products
                .Find(filterDefinition)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _catalogContext
                .Products
                .Find(x => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            ReplaceOneResult result =  await _catalogContext
                .Products
                .ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}

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
            _catalogContext = catalogContext;
        }

        public async Task CreateProduct(Products product)
        {
            await _catalogContext.products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            FilterDefinition<Products> filter = Builders<Products>.Filter.Eq(p=>p.Id, productId);

            var deleteResulte = await _catalogContext.products.DeleteOneAsync(filter);

            return deleteResulte.IsAcknowledged && deleteResulte.DeletedCount > 0;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await _catalogContext.products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetProductsByProductCatagoryAsync(string productCatagory)
        {
            FilterDefinition<Products> filter= Builders<Products>.Filter.Eq(p => p.Category, productCatagory);

            return await _catalogContext.products.Find(filter).ToListAsync();
        }

        public async Task<Products> GetProductsByProductIdAsync(string productId)
        {
            return await _catalogContext.products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Products>> GetProductsByProductNameAsync(string productName)
        {
            FilterDefinition<Products> filters = Builders<Products>.Filter.Eq(p => p.Name, productName);
            return await _catalogContext.products.Find(filters).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Products product)
        {
            var updateResult = await _catalogContext.products.ReplaceOneAsync(filter: g => g.Id == product.Id,replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}

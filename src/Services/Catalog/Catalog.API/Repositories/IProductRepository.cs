using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetProductsAsync();
        Task<Products> GetProductsByProductIdAsync(string productId);
        Task<IEnumerable<Products>> GetProductsByProductNameAsync(string productName);
        Task<IEnumerable<Products>> GetProductsByProductCatagoryAsync(string productCatagory);

        Task CreateProduct(Products product);
        Task<bool> UpdateProduct(Products product);
        Task<bool> DeleteProduct(string productId);
    }
}

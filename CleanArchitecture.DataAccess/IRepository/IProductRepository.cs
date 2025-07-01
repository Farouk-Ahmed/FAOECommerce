

namespace CleanArchitecture.DataAccess.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsWithCategoriesAsync(); // To eager-load category data
    }
}

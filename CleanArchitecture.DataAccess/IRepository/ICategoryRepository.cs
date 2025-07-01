
namespace CleanArchitecture.DataAccess.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
    }
}

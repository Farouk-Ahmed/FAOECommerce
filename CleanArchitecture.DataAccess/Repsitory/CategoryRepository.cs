using CleanArchitecture.DataAccess.Contexts;
using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.Repsitory
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
        {
            return await _context.Categories.Include(c => c.Products).ToListAsync();
        }
    }
}

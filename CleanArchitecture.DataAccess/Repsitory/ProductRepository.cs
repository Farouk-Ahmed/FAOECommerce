using CleanArchitecture.DataAccess.Contexts;
using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.Repsitory
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Product>();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {

             return await _context.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToListAsync();
        }
        
        

        public async Task<IEnumerable<Product>> GetProductsWithCategoriesAsync()
         =>await _context.Products.Include(p => p.Category).ToListAsync();

        public IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Product> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query.AsNoTracking().ToList();
        }

        public IQueryable<Product> GetAllQuery(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Product> query = _context.Products;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
                return query;
        }

        public Product Get(Expression<Func<Product, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Product> query = _context.Products;

            if (!tracked)
                query = query.AsNoTracking();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault(filter)!;

        }

        public void Add(Product entity)
        {
            _context.Products.Add(entity);

        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);

        }

        public void DeleteRange(IEnumerable<Product> entities)
        {
            _context.Products.RemoveRange(entities);

        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);

        }
    }
}

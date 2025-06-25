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
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Product>();
        }
        public IEnumerable<ShoppingCartItem> GetAll(Expression<Func<ShoppingCartItem, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<ShoppingCartItem> query = (IQueryable<ShoppingCartItem>)_dbSet;

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

            // Execute the query and return the results as a list.
            return query.AsNoTracking().ToList();
        }

        public IQueryable<ShoppingCartItem> GetAllQuery(Expression<Func<ShoppingCartItem, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<ShoppingCartItem> query = (IQueryable<ShoppingCartItem>)_dbSet;

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

            // Return IQueryable for further manipulation (query is not executed yet).
            return query;
        }

        public ShoppingCartItem Get(Expression<Func<ShoppingCartItem, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<ShoppingCartItem> query = _context.ShoppingCartItems;

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

        public void Add(ShoppingCartItem entity)
        {
            _context.ShoppingCartItems.Add(entity);

        }

        public void Delete(ShoppingCartItem entity)
        {
            _context.ShoppingCartItems.Remove(entity);

        }

        public void DeleteRange(IEnumerable<ShoppingCartItem> entities)
        {
            _context.ShoppingCartItems.RemoveRange(entities);

        }

        public void Update(ShoppingCartItem entity)
        {
            _context.ShoppingCartItems.Update(entity);

        }

        public IQueryable<ShoppingCartItem> GetQuery()
        {
            return _context.Set<ShoppingCartItem>();

        }
    }
}

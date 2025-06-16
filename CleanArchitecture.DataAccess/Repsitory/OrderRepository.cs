using CleanArchitecture.DataAccess.Contexts;
using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.Repsitory
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Set<Order>()
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId.ToString())
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Set<Order>()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}

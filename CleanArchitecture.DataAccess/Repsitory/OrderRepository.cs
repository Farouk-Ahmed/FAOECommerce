

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

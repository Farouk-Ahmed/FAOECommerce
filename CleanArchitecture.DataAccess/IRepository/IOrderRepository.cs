using CleanArchitecture.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> GetOrderWithItemsAsync(int orderId);
    }
}

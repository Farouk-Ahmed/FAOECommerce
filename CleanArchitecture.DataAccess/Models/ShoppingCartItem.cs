using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.Models
{
    public class ShoppingCartItem:ModelBase
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Identity User Id
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public string CartCode { get; set; } // Unique code for the cart item
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}

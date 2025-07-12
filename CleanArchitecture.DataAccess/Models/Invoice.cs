using System;
using System.Collections.Generic;

namespace CleanArchitecture.DataAccess.Models
{
    public class Invoice : ModelBase
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } // Same as CartCode
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } // For purchase details
    }
}

using System;
using System.Collections.Generic;

namespace CleanArchitecture.Services.DTOs.Invoices
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceOrderItemDto> OrderItems { get; set; }
    }

    public class InvoiceOrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string CartCode { get; set; }
    }
}

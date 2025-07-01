
namespace CleanArchitecture.Services.DTOs.Orders
{
    public class OrderCreateDto
    {
        public string UserId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; }
    }
    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
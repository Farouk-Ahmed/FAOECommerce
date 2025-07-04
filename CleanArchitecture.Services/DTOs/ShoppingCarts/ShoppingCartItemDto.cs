namespace CleanArchitecture.Services.DTOs.ShoppingCarts
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string CartCode { get; set; } // Expose CartCode in DTO
    }
}
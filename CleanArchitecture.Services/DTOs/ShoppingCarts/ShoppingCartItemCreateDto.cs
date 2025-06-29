namespace CleanArchitecture.Services.DTOs.ShoppingCarts
{
    public class ShoppingCartItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class ShoppingCartItemUpdateDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
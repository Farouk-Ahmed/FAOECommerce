
namespace CleanArchitecture.DataAccess.Models
{
    public class Product:ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; } // Foreign key
        public Category Category { get; set; }
        public ICollection<ProductPhoto> Photos { get; set; }
        public ICollection<ShoppingCartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class ProductPhoto : ModelBase
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsMain { get; set; } = false; // Indicates if this is the main/cover photo

    }
    public class ProductPhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string ProductName { get; set; }

    }
}

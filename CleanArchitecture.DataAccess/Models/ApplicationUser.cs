
namespace CleanArchitecture.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public ICollection<ShoppingCartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}

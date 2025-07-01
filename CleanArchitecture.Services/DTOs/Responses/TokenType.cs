
namespace CleanArchitecture.Services.DTOs.Responses
{
    public class TokenType
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiryTokenDate { get; set; }
    }
}



namespace CleanArchitecture.Services.DTOs.Responses
{
    public class LoginResponse
    {
        public TokenType AccessToken { get; set; }
        public TokenType RefreshToken { get; set; }

    }
}

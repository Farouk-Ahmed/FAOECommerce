

namespace CleanArchitecture.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenType>> RegisterUserAsync(RegisterUser registerUser);

        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
    }
}

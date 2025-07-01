
namespace CleanArchitecture.Services.Interfaces
{
    public interface IOtpService
    {
        Task SetOtpAsync(string key, string otp, string userName, string userId, TimeSpan expiration);
        Task<string> GetOtpAsync(string key);

        Task RemoveOtpAsync(string key);
        Task SetOtpAsUsedAsync(string key);

    }

}

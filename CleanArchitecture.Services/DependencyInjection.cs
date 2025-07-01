
namespace CleanArchitecture.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddValidatorsFromAssemblyContaining<SignInDTOValidator>();
            services.AddFluentValidationAutoValidation(); // Ensure FluentValidation.DependencyInjectionExtensions is referenced
            return services;
        }
    }
}

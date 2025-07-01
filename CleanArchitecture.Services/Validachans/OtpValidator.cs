

namespace CleanArchitecture.Services.Validachans
{
    public class OtpValidator : AbstractValidator<Otp>
    {
        public OtpValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(4, 10).WithMessage("OTP code must be between 4 and 10 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("OTP has already expired.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .When(x => !string.IsNullOrEmpty(x.UserId)); // optional if UserId is sometimes null
        }

    }
}

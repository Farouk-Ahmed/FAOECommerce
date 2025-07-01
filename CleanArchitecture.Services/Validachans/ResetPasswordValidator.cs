

namespace CleanArchitecture.Services.Validachans
{
    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid Email.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Otp is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must be at least 8 characters and include letters, numbers, and a special character.");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
        }
    }


}

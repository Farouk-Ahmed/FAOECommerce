
namespace CleanArchitecture.Services.Validachans
{

public class SignInDTOValidator : AbstractValidator<SignInDTO>
    {
        public SignInDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required!")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("Password must be at least 6 characters long and contain both letters and numbers.");
        }
    }
}

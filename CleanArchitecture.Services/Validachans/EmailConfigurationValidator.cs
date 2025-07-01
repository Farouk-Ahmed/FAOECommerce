
public class EmailConfigurationValidator : AbstractValidator<EmailConfiguration>
{
    public EmailConfigurationValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty().WithMessage("Sender email is required.")
            .EmailAddress().WithMessage("Invalid sender email format.");

        RuleFor(x => x.SmtpServer)
            .NotEmpty().WithMessage("SMTP server is required.");

        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535).WithMessage("Port must be a valid number between 1 and 65535.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("SMTP username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("SMTP password is required.");
    }
}
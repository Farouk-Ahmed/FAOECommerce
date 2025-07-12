using FluentValidation;
using CleanArchitecture.DataAccess.Models;

namespace CleanArchitecture.Services.Validachans
{
    public class InvoiceValidator : AbstractValidator<Invoice>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.")
                .MaximumLength(50).WithMessage("Invoice number must be at most 50 characters.");

            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be non-negative.");
        }
    }
}

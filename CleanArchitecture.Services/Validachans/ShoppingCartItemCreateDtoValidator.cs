
public class ShoppingCartItemUpdateDtoValidator : AbstractValidator<ShoppingCartItemUpdateDto>
{
    public ShoppingCartItemUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Item ID is required.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be greater than zero.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
    public class ShoppingCartItemCreateDtoValidator : AbstractValidator<ShoppingCartItemCreateDto>
    {
        public ShoppingCartItemCreateDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }

}
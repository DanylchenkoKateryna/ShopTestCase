using FluentValidation;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Data.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Code)
                .NotEmpty().WithMessage("Product code is required.")
                .MaximumLength(50).WithMessage("Product code must not exceed 50 characters.");

            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}

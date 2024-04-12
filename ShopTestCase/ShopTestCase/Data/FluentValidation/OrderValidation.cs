using FluentValidation;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Data.FluentValidation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.CustomerFullName).NotEmpty().WithMessage("Customer full name is required.");
            RuleFor(order => order.CustomerPhone).NotEmpty().WithMessage("Customer phone is required.")
                                                   .Matches(@"^\d{10}$").WithMessage("Customer phone must be 10 digits.");

            RuleForEach(order => order.OrderProducts).SetValidator(new OrderProductValidator());
        }
    }
}

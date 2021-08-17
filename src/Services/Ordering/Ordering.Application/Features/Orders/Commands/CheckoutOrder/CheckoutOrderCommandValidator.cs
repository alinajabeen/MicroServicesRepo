using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    //Pre Processor Behaviour class is Validator class
    class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                 .NotEmpty().WithMessage("{UserName} is required.")
                 .NotNull()
                 .MaximumLength(50).WithMessage("{UserName} must not exceed to 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p=> p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is Required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");

        }
    }
}

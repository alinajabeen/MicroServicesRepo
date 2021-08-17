using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
   public class UpdateOrderCommandValidtor : AbstractValidator<UpdateOrderCommand>
    {
      public UpdateOrderCommandValidtor()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed to 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is Required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        } 
    }
}

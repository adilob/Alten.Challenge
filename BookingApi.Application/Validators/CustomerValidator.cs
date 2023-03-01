using BookingApi.Domain.Entities;
using FluentValidation;

namespace BookingApi.Application.Validators
{
    internal class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(entity => entity.Id)
                .NotEmpty();

            RuleFor(entity => entity.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(entity => entity.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(entity => entity.Email)
                .NotEmpty()
                .MaximumLength(300)
                .EmailAddress();
        }
    }
}

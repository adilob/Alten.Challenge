using BookingApi.Domain.Entities;
using FluentValidation;

namespace BookingApi.Application.Validators
{
    public class RoomValidator : AbstractValidator<Room>
    {
        internal RoomValidator()
        {
            RuleFor(entity => entity.Id)
                .NotEmpty();

            RuleFor(entity => entity.RoomNumber)
                .NotEmpty();
        }
    }
}

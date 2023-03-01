using BookingApi.Domain.Entities;
using FluentValidation;

namespace BookingApi.Application.Validators
{
    internal class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator()
        {
            RuleFor(entity => entity.Id)
                .NotEmpty();

            RuleFor(entity => entity.CustomerId)
                .NotEmpty();

            RuleFor(entity => entity.RoomId)
                .NotEmpty();

            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            RuleFor(entity => entity.StartReservation)
                .NotEmpty()
                .LessThan(entity => entity.EndReservation)
                .GreaterThanOrEqualTo(now.AddDays(1)) // All reservations start at least the next day of booking
                .LessThanOrEqualTo(now.AddDays(31)); // can’t be reserved more than 30 days in advance

            RuleFor(entity => entity.EndReservation)
                .NotEmpty()
                .GreaterThan(entity => entity.StartReservation)
                .LessThanOrEqualTo(entity => entity.StartReservation.AddDays(2).AddHours(23).AddMinutes(59).AddSeconds(59)); // the stay can’t be longer than 3 days
        }
    }
}

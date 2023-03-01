using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Validators;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;
using Microsoft.VisualBasic;
using System.Text;

namespace BookingApi.Application.Managers
{
    public class ReservationManager : IReservationManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Reservation> CancelReservation(Guid reservationId)
        {
            if (reservationId == Guid.Empty)
            {
                throw new ReservationNotValidException("You should provide a valid ID to be cancelled.");
            }

            var reservations = _unitOfWork.Reservations.GetAll().Where(x => x.Id == reservationId);
            if (((reservations?.Any()) ?? false) == false)
            {
                throw new ReservationNotFoundException($"The reservation ID {reservationId} was not found.");
            }

            var reservation = reservations.FirstOrDefault();

            _unitOfWork.Reservations.Delete(reservation);
            _unitOfWork.Complete();

            return reservation;
        }

        public async Task<Reservation> GetById(Guid reservationId)
        {
            return await Task.FromResult(_unitOfWork.Reservations.GetById(reservationId));
        }

        public async Task<Reservation> ModifyReservation(Reservation reservation)
        {
            var validator = new ReservationValidator();
            var validationResult = await validator.ValidateAsync(reservation);

            if (!validationResult.IsValid)
            {
                var messages = new StringBuilder();
                validationResult.Errors.ForEach(error => messages.AppendLine(error.ErrorMessage));
                throw new ReservationNotValidException(messages.ToString());
            }

            var hasConflict = await HasConflictWithAnotherReservation(reservation, true);
            if (hasConflict)
            {
                throw new ReservationNotValidException("This reservation conflicts with another one in the same period.");
            }

            _unitOfWork.Reservations.DetachLocal(reservation);
            _unitOfWork.Reservations.Update(reservation);

            _unitOfWork.Complete();

            return await Task.FromResult(reservation);
        }

        public async Task<Reservation> NewReservation(Reservation reservation)
        {
            var validator = new ReservationValidator();
            var validationResult = await validator.ValidateAsync(reservation);

            if (!validationResult.IsValid)
            {
                var messages = new StringBuilder();
                validationResult.Errors.ForEach(error => messages.AppendLine(error.ErrorMessage));
                throw new ReservationNotValidException(messages.ToString());
            }

            var hasConflict = await HasConflictWithAnotherReservation(reservation);
            if (hasConflict)
            {
                throw new ReservationNotValidException("This reservation conflicts with another one in the same period.");
            }

            _unitOfWork.Reservations.Add(reservation);
            _unitOfWork.Complete();

            var reservationToReturn = _unitOfWork.Reservations.GetById(reservation.Id);
            return await Task.FromResult(reservationToReturn);
        }

        private async Task<bool> HasConflictWithAnotherReservation(Reservation reservation, bool isEdit = false)
        {
            var reservations = _unitOfWork.Reservations.GetAll().OrderBy(x => x.StartReservation).ToList();
            IEnumerable<Reservation> reservationsInPeriod;

            if (isEdit)
            {
                reservationsInPeriod = from r in reservations
                                       where ((r.StartReservation >= reservation.StartReservation && r.StartReservation <= reservation.EndReservation) ||
                                                (r.EndReservation >= reservation.StartReservation && r.EndReservation <= reservation.EndReservation)) &&
                                                r.Id != reservation.Id
                                       select r;
            }
            else
            {
                reservationsInPeriod = from r in reservations
                                       where ((r.StartReservation >= reservation.StartReservation && r.StartReservation <= reservation.EndReservation) ||
                                                (r.EndReservation >= reservation.StartReservation && r.EndReservation <= reservation.EndReservation))
                                       select r;
            }

            return await Task.FromResult(reservationsInPeriod.Any());
        }
    }
}

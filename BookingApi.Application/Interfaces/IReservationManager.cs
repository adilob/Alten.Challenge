using BookingApi.Domain.Entities;

namespace BookingApi.Application.Interfaces
{
    public interface IReservationManager
    {
        Task<Reservation> GetById(Guid reservationId);
        Task<Reservation> NewReservation(Reservation reservation);
        Task<Reservation> CancelReservation(Guid reservationId);
        Task<Reservation> ModifyReservation(Reservation reservation);
    }
}

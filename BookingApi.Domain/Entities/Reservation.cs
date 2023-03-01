using BookingApi.Core.Abstractions;

namespace BookingApi.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid CustomerId { get; private init; }
        public Guid RoomId { get; private init; }
        public DateTime StartReservation { get; private init; }
        public DateTime EndReservation { get; private init; }

        public Reservation(Guid customerId, Guid roomId, DateTime startReservation, DateTime endReservation)
        {
            CustomerId = customerId;
            RoomId = roomId;
            StartReservation = new DateTime(startReservation.Year, startReservation.Month, startReservation.Day, 0, 0, 0); // a “DAY’ in the hotel room starts from 00:00 to 23:59:59
            EndReservation = new DateTime(endReservation.Year, endReservation.Month, endReservation.Day, 23, 59, 59); // a “DAY’ in the hotel room starts from 00:00 to 23:59:59
        }
    }
}

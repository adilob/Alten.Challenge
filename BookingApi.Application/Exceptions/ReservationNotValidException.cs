namespace BookingApi.Application.Exceptions
{
    public class ReservationNotValidException : Exception
    {
        public ReservationNotValidException(string? message) : base(message)
        {
        }
    }
}

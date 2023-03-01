namespace BookingApi.Application.Exceptions
{
    public class CustomerNotValidException : Exception
    {
        public CustomerNotValidException(string? message) : base(message)
        {
        }
    }
}

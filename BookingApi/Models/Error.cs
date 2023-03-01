namespace BookingApi.Models
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Resource { get; set; }

        public Error(string errorCode, string errorMessage, string resource)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Resource = resource;
        }
    }
}

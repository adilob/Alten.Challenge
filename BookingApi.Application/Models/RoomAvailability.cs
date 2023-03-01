namespace BookingApi.Application.Models
{
    public class RoomAvailability
    {
        public DateTime? NextAvailableDate { get; set; }
        public int? AvailableDays { get; set; }
    }
}

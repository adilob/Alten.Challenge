using BookingApi.Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Domain.Entities
{
    public class Room : BaseEntity
    {
        [Required]
        public int RoomNumber { get; private init; }
        public string? Description { get; set; }

        public Room(int roomNumber)
        {
            RoomNumber = roomNumber;
        }
    }
}

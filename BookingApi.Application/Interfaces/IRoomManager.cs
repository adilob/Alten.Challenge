using BookingApi.Application.Models;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.Interfaces
{
    public interface IRoomManager
    {
        Task<List<Room>> GetAll();
        Task<RoomAvailability> GetAvailability(int roomNumber);
    }
}

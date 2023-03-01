using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Models;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;

namespace BookingApi.Application.Managers
{
    public class RoomManager : IRoomManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Room>> GetAll()
        {
            var result = _unitOfWork.Rooms.GetAll();
            return await Task.FromResult(new List<Room>(result));
        }

        public async Task<RoomAvailability> GetAvailability(int roomNumber)
        {
            var result = new RoomAvailability();
            var rooms = _unitOfWork.Rooms.GetAll().Where(x => x.RoomNumber == roomNumber);

            if (!rooms.Any())
            {
                throw new RoomNotFoundException($"The room number {roomNumber} was not found.");
            }

            var reservations = _unitOfWork.Reservations.GetAll().OrderBy(x => x.StartReservation).ToList();
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            if (!reservations.Any())
            {
                result.NextAvailableDate = now.AddDays(1);
                result.AvailableDays = 3; // can't stay longer than 3 days
                return await Task.FromResult(result);
            }

            var dayToCheck = now.AddDays(1);

            while (true)
            {
                var hasReservation = reservations.Any(x => dayToCheck >= x.StartReservation && dayToCheck <= x.EndReservation);
                if (hasReservation)
                {
                    dayToCheck = reservations.FirstOrDefault(x => dayToCheck >= x.StartReservation && dayToCheck <= x.EndReservation).EndReservation.AddSeconds(1);
                }
                else
                {
                    if (!result.NextAvailableDate.HasValue)
                    {
                        result.NextAvailableDate = dayToCheck;
                    }

                    var nextReservation = reservations.FirstOrDefault(x => x.StartReservation > dayToCheck);
                    if (nextReservation != null)
                    {
                        var availableDays = nextReservation.StartReservation.Subtract(dayToCheck);
                        result.AvailableDays = availableDays.Days > 3 ? 3 : availableDays.Days;
                    }
                    else
                    {
                        result.AvailableDays = 3;
                    }

                    break;
                }
            }

            return await Task.FromResult(result);
        }
    }
}

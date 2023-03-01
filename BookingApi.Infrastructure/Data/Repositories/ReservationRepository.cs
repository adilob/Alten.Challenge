using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data.Context;
using BookingApi.Infrastructure.Data.Repositories;
using BookingApi.Infrastructure.Interfaces;

namespace BookingApi.Infrastructure.Repositories
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}

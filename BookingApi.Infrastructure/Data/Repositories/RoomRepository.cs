using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data.Context;
using BookingApi.Infrastructure.Data.Repositories;
using BookingApi.Infrastructure.Interfaces;

namespace BookingApi.Infrastructure.Repositories
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}

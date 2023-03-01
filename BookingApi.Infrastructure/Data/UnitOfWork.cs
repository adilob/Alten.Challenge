using BookingApi.Infrastructure.Data.Context;
using BookingApi.Infrastructure.Interfaces;
using BookingApi.Infrastructure.Repositories;

namespace BookingApi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public ICustomerRepository Customers { get; private set; }

        public IReservationRepository Reservations { get; private set; }

        public IRoomRepository Rooms { get; private set; }

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _context = applicationContext;

            Customers = new CustomerRepository(_context);
            Reservations = new ReservationRepository(_context);
            Rooms = new RoomRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

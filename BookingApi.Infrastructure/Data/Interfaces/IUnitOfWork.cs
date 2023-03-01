namespace BookingApi.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IReservationRepository Reservations { get; }
        IRoomRepository Rooms { get; }

        int Complete();
    }
}

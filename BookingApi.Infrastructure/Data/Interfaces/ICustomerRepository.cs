using BookingApi.Core.Interfaces;
using BookingApi.Domain.Entities;

namespace BookingApi.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}

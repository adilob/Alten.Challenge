using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data.Context;
using BookingApi.Infrastructure.Data.Repositories;
using BookingApi.Infrastructure.Interfaces;

namespace BookingApi.Infrastructure.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}

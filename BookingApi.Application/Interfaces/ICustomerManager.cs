using BookingApi.Domain.Entities;

namespace BookingApi.Application.Interfaces
{
    public interface ICustomerManager
    {
        Task<Customer> GetById(Guid id);
        Task<List<Customer>> GetAll();
        Task CreateNewCustomer(Customer customer);
    }
}

using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Validators;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;
using System.Text;

namespace BookingApi.Application.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewCustomer(Customer customer)
        {
            var validator = new CustomerValidator();
            var validationResult = await validator.ValidateAsync(customer);

            if (!validationResult.IsValid)
            {
                var messages = new StringBuilder();
                validationResult.Errors.ForEach(error => messages.AppendLine(error.ErrorMessage));
                throw new CustomerNotValidException(messages.ToString());
            }

            _unitOfWork.Customers.Add(customer);
            _unitOfWork.Complete();
        }

        public async Task<List<Customer>> GetAll()
        {
            var result = _unitOfWork.Customers.GetAll();
            return await Task.FromResult(new List<Customer>(result));
        }

        public async Task<Customer> GetById(Guid id)
        {
            var result = _unitOfWork.Customers.GetById(id);
            return await Task.FromResult(result);
        }
    }
}

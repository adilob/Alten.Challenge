using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Managers;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;
using Moq;

namespace BookingApi.Application.Tests.Managers
{
    public class CustomerManagerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private ICustomerManager _customerManager;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _customerManager = new CustomerManager(_unitOfWork.Object);
        }

        [Test]
        public async Task GetAllCustomers_Expect_NoContent()
        {
            var customers = new List<Customer>();

            _unitOfWork
                .Setup(x => x.Customers.GetAll())
                .Returns(customers);

            var result = await _customerManager.GetAll();

            Assert.That(result, Is.EqualTo(customers));
        }

        [Test]
        public async Task GetAllCustomers_Expect_OneResult()
        {
            var customers = new List<Customer>
            {
                new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com")
            };

            _unitOfWork
                .Setup(x => x.Customers.GetAll())
                .Returns(customers);

            var result = await _customerManager.GetAll();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(customers));
        }

        [Test]
        public async Task GetCustomerById_Expect_Result()
        {
            var customer = new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com");

            _unitOfWork
                .Setup(x => x.Customers.GetById(It.IsAny<Guid>()))
                .Returns(customer);

            var result = await _customerManager.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result, Is.EqualTo(customer));
        }

        [Test]
        public async Task GetCustomerById_Expect_NotFound()
        {
            Customer customer = null;

            _unitOfWork
                .Setup(x => x.Customers.GetById(It.IsAny<Guid>()))
                .Returns(customer);

            var result = await _customerManager.GetById(It.IsAny<Guid>());
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateNewCustomer_Expect_Result()
        {
            var customer = new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com");

            _unitOfWork
                .Setup(x => x.Customers.Add(customer))
                .Verifiable();

            await _customerManager.CreateNewCustomer(customer);
            _unitOfWork.Verify(x => x.Customers.Add(customer), Times.Once());
        }

        [Test]
        public void CreateNewCustomer_Expect_Exception()
        {
            var customer = new Customer("Adilo", "B", "adilobertoncello@gmail");

            _unitOfWork
                .Setup(x => x.Customers.Add(customer))
                .Verifiable();

            Assert.That(async () => await _customerManager.CreateNewCustomer(customer), Throws.TypeOf(typeof(CustomerNotValidException)));
        }
    }
}

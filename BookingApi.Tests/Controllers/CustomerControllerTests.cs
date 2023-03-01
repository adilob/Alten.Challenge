using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Controllers;
using BookingApi.Domain.Entities;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingApi.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private Mock<ICustomerManager> _customerManager;

        private CustomerController _customerController;

        [SetUp]
        public void Setup()
        {
            _customerManager = new Mock<ICustomerManager>();
            _customerController = new CustomerController(_customerManager.Object);
        }

        [Test]
        public async Task GetAll_Expect_OkResult()
        {
            var customers = new List<Customer>
            {
                new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com")
            };

            _customerManager
                .Setup(m => m.GetAll())
                .ReturnsAsync(customers);

            var result = await _customerController.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.TypeOf<List<Customer>>());

            var customersResult = okResult.Value as List<Customer>;
            Assert.That(customersResult?.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAll_Expect_NoContent()
        {
            var customers = new List<Customer>();

            _customerManager
                .Setup(m => m.GetAll())
                .ReturnsAsync(customers);

            var result = await _customerController.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task GetById_Expect_OkResult()
        {
            var customer = new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com");

            _customerManager
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(customer);

            var result = await _customerController.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.TypeOf<Customer>());

            var customerResult = okResult.Value as Customer;
            Assert.That(customerResult, Is.EqualTo(customer));
        }

        [Test]
        public async Task GetById_Expect_NotFound()
        {
            Customer customer = null;

            _customerManager
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(customer);

            var result = await _customerController.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.TypeOf<Error>());

            var error = notFoundResult.Value as Error;
            Assert.That(error?.ErrorCode, Is.EqualTo("NOT_FOUND"));
        }

        [Test]
        public async Task CreateNewCustomer_Expect_Created()
        {
            var customer = new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com");

            _customerManager
                .Setup(x => x.CreateNewCustomer(customer))
                .Verifiable();

            var result = await _customerController.CreateNewCustomer(customer);

            _customerManager.Verify(x => x.CreateNewCustomer(customer), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<CreatedResult>());

            var createdResult = (CreatedResult)result;
            Assert.That(createdResult.Value, Is.EqualTo(customer));
        }

        [Test]
        public async Task CreateNewCustomer_Expect_BadRequest()
        {
            var customer = new Customer("Adilo", "Bertoncello", "adilobertoncello@gmail.com");

            _customerManager
                .Setup(x => x.CreateNewCustomer(customer))
                .Throws(new CustomerNotValidException("Internal error message"))
                .Verifiable();

            var result = await _customerController.CreateNewCustomer(customer);

            _customerManager.Verify(x => x.CreateNewCustomer(customer), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = (BadRequestObjectResult)result;

            Assert.That(badRequestResult.Value, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.TypeOf(typeof(Error)));

            var error = (Error)badRequestResult.Value;

            Assert.That(error?.ErrorMessage, Is.EqualTo("Internal error message"));
            Assert.That(error?.ErrorCode, Is.EqualTo("INPUT_NOT_VALID"));
        }
    }
}

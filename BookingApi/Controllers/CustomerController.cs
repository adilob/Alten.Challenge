using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Domain.Entities;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerManager _customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        /// <summary>
        /// Gets all customers from database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Customer>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerManager.GetAll();

            if (result?.Count <= 0)
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets the customer by ID.
        /// </summary>
        /// <param name="customerId">The <see cref="Guid"/> value that identifies the customer.</param>
        [HttpGet]
        [Route("{customerId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Customer))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Error))]
        public async Task<IActionResult> GetById(Guid customerId)
        {
            var result = await _customerManager.GetById(customerId);

            if (result == null)
            {
                var notFoundError = new Error(
                    "NOT_FOUND",
                    $"Customer not found using the provided Id, ID = {customerId}",
                    "customer");

                return NotFound(notFoundError);
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new customer on the database.
        /// </summary>
        /// <param name="customer">The <see cref="Customer"/> object which has the customer's information.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Customer))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CreateNewCustomer([FromBody] Customer customer)
        {
            try
            {
                await _customerManager.CreateNewCustomer(customer);
                return Created($"/customer/{customer.Id}", customer);
            }
            catch (CustomerNotValidException ex)
            {
                var badRequestError = new Error(
                    "INPUT_NOT_VALID",
                    ex.Message,
                    "customer");

                return BadRequest(badRequestError);
            }
        }
    }
}

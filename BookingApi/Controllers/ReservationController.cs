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
    public class ReservationController : Controller
    {
        private readonly IReservationManager _reservationManager;

        public ReservationController(IReservationManager reservationManager)
        {
            _reservationManager = reservationManager;
        }

        /// <summary>
        /// Gets a reservation by the ID.
        /// </summary>
        /// <param name="reservationId">The <see cref="Guid"/> value that identifies the reservation.</param>
        [HttpGet]
        [Route("{reservationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Reservation))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Error))]
        public async Task<IActionResult> GetById(Guid reservationId)
        {
            var result = await _reservationManager.GetById(reservationId);

            if (result == null)
            {
                var notFoundError = new Error("NOT_FOUND", $"No reservation found for the ID: {reservationId}", "reservation");
                return NotFound(notFoundError);
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new reservation on the database.
        /// </summary>
        /// <param name="reservation">The <see cref="Reservation"/> object which has the reservation's information.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Reservation))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> NewReservation([FromBody] Reservation reservation)
        {
            try
            {
                var result = await _reservationManager.NewReservation(reservation);
                return Created($"/reservation/{result.Id}", result);
            }
            catch (ReservationNotValidException ex)
            {
                var badRequestError = new Error("INPUT_NOT_VALID", ex.Message, "reservation");
                return BadRequest(badRequestError);
            }
        }

        /// <summary>
        /// Modifies a reservation.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> object that identifies the reservation.</param>
        /// <param name="reservation">The <see cref="Reservation"/> object with the modified properties to be saved.</param>
        [HttpPut]
        [Route("{reservationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Reservation))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Error))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> ChangeReservation(Guid reservationId, [FromBody] Reservation reservation)
        {
            try
            {
                var r = await _reservationManager.GetById(reservationId);
                if (r == null)
                {
                    var notFoundError = new Error("NOT_FOUND", $"The reservation with ID {reservationId} was not found.", "reservation");
                    return NotFound(notFoundError);
                }

                reservation.Id = r.Id;
                var result = await _reservationManager.ModifyReservation(reservation);

                return Ok(result);
            }
            catch (ReservationNotValidException ex)
            {
                var badRequestError = new Error("INPUT_NOT_VALID", ex.Message, "reservation");
                return BadRequest(badRequestError);
            }
        }

        /// <summary>
        /// Cancels a reservation by the ID.
        /// </summary>
        /// <param name="reservationId">The <see cref="Guid"/> object that identifies the reservation.</param>
        [HttpDelete]
        [Route("{reservationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Reservation))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Error))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CancelReservation(Guid reservationId)
        {
            try
            {
                var result = await _reservationManager.CancelReservation(reservationId);
                return Ok(result);
            }
            catch (ReservationNotValidException ex)
            {
                var badRequestError = new Error("INPUT_NOT_VALID", ex.Message, "reservation");
                return BadRequest(badRequestError);
            }
            catch (ReservationNotFoundException ex)
            {
                var notFoundError = new Error("NOT_FOUND", ex.Message, "reservation");
                return NotFound(notFoundError);
            }
        }
    }
}

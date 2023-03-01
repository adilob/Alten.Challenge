using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Models;
using BookingApi.Domain.Entities;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;

        public RoomController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        /// <summary>
        /// Gets all available rooms from database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Room>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomManager.GetAll();

            if (result?.Count <= 0)
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets the room's availability.
        /// </summary>
        /// <param name="roomNumber">The room number.</param>
        [HttpGet]
        [Route("{roomNumber}/availability")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RoomAvailability))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Error))]
        public async Task<IActionResult> GetAvailability(int roomNumber)
        {
            try
            {
                var result = await _roomManager.GetAvailability(roomNumber);
                return Ok(result);
            }
            catch (RoomNotFoundException ex)
            {
                var notFoundError = new Error("NOT_FOUND", ex.Message, "room");
                return NotFound(notFoundError);
            }
        }
    }
}

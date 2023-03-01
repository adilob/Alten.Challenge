using BookingApi.Application.Interfaces;
using BookingApi.Controllers;
using BookingApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingApi.Tests.Controllers
{
    public class RoomControllerTests
    {
        private Mock<IRoomManager> _roomManager;

        private RoomController _roomController;

        [SetUp]
        public void Setup()
        {
            _roomManager = new Mock<IRoomManager>();
            _roomController = new RoomController(_roomManager.Object);
        }

        [Test]
        public async Task GetAll_Expect_OkResult()
        {
            var rooms = new List<Room>
            {
                new Room(101)
            };

            _roomManager
                .Setup(r => r.GetAll())
                .ReturnsAsync(rooms);

            var result = await _roomController.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<List<Room>>());
        }

        [Test]
        public async Task GetAll_Expect_NoContent()
        {
            var rooms = new List<Room>();

            _roomManager
                .Setup(r => r.GetAll())
                .ReturnsAsync(rooms);

            var result = await _roomController.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task IsRoomAvailable_Expect_OkResult()
        {
            var isAvailable = true;

            //_roomManager
            //    .Setup(r => r.IsRoomAvailable(It.IsAny<int>()))
            //    .ReturnsAsync(isAvailable);

            //var result = await _roomController.IsRoomAvailable(It.IsAny<int>());

            //Assert.That(result, Is.Not.Null);
            //Assert.That(result, Is.TypeOf<OkObjectResult>());

            //var okResult = (OkObjectResult)result;

            //Assert.That(okResult, Is.Not.Null);
            //Assert.That(okResult.Value, Is.True);
        }
    }
}

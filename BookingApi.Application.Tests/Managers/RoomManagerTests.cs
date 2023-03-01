using BookingApi.Application.Interfaces;
using BookingApi.Application.Managers;
using BookingApi.Application.Models;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;
using Moq;

namespace BookingApi.Application.Tests.Managers
{
    public class RoomManagerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IRoomManager _roomManager;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _roomManager = new RoomManager(_unitOfWork.Object);
        }

        [Test]
        public async Task GetAll_Expect_NoContent()
        {
            var rooms = new List<Room>();

            _unitOfWork
                .Setup(x => x.Rooms.GetAll())
                .Returns(rooms);

            var result = await _roomManager.GetAll();
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAll_Expect_OneResult()
        {
            var rooms = new List<Room>
            {
                new Room(101)
            };

            _unitOfWork
                .Setup(x => x.Rooms.GetAll())
                .Returns(rooms);

            var result = await _roomManager.GetAll();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(rooms));
        }

        [Test]
        public async Task GetAvailability_Expect_Next_Day_After_Reservation_And_3_Days_Available()
        {
            IEnumerable<Room> rooms = new List<Room> { new Room(101) };
            IEnumerable<Reservation> reservations = new List<Reservation>
            {
                new Reservation(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2))
            };

            _unitOfWork
                .Setup(x => x.Rooms.GetAll())
                .Returns(rooms);

            _unitOfWork
                .Setup(x => x.Reservations.GetAll())
                .Returns(reservations);

            var result = await _roomManager.GetAvailability(101);
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RoomAvailability>());
            Assert.That(result.NextAvailableDate, Is.EqualTo(now.AddDays(3)));
            Assert.That(result.AvailableDays, Is.EqualTo(3));
        }
    }
}

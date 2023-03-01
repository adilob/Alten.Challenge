using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Controllers;
using BookingApi.Domain.Entities;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingApi.Tests.Controllers
{
    public class ReservationControllerTests
    {
        private Mock<IReservationManager> _reservationManager;

        private ReservationController _reservationController;

        [SetUp]
        public void Setup()
        {
            _reservationManager = new Mock<IReservationManager>();
            _reservationController = new ReservationController(_reservationManager.Object);
        }

        [Test]
        public async Task GetById_Expect_OkResult()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(reservation);

            var result = await _reservationController.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<Reservation>());
            Assert.That(okResult.Value, Is.EqualTo(reservation));
        }

        [Test]
        public async Task GetById_Expect_NotFound()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            Reservation reservation = null;

            _reservationManager
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(reservation);

            var result = await _reservationController.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = (NotFoundObjectResult)result;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.Value, Is.TypeOf<Error>());
        }

        [Test]
        public async Task NewReservation_Expect_Created()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.NewReservation(reservation))
                .ReturnsAsync(reservation);

            var result = await _reservationController.NewReservation(reservation);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<CreatedResult>());

            var createdResult = (CreatedResult)result;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.Value, Is.TypeOf<Reservation>());
        }

        [Test]
        public async Task NewReservation_Expect_BadRequest()
        {
            var customerId = Guid.Empty;
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.NewReservation(reservation))
                .Throws(new ReservationNotValidException("Internal error message"));

            var result = await _reservationController.NewReservation(reservation);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = (BadRequestObjectResult)result;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.TypeOf<Error>());
        }

        [Test]
        public async Task ChangeReservation_Expect_OkResult()
        {
            var reservationId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.ModifyReservation(reservation))
                .ReturnsAsync(reservation);

            _reservationManager
                .Setup(x => x.GetById(reservationId))
                .ReturnsAsync(reservation);

            var result = await _reservationController.ChangeReservation(reservationId, reservation);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<Reservation>());
        }

        [Test]
        public async Task ChangeReservation_Expect_BadRequest()
        {
            var reservationId = Guid.NewGuid();
            var customerId = Guid.Empty;
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.ModifyReservation(reservation))
                .Throws(new ReservationNotValidException("Internal error message"));

            _reservationManager
                .Setup(x => x.GetById(reservationId))
                .ReturnsAsync(reservation);

            var result = await _reservationController.ChangeReservation(reservationId, reservation);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = (BadRequestObjectResult)result;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.TypeOf<Error>());
        }

        [Test]
        public async Task CancelReservation_Expect_OkResult()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.CancelReservation(reservation.Id))
                .ReturnsAsync(reservation);

            var result = await _reservationController.CancelReservation(reservation.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<Reservation>());
        }

        [Test]
        public async Task CancelReservation_Expect_NotFound()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);

            _reservationManager
                .Setup(x => x.CancelReservation(It.IsAny<Guid>()))
                .Throws(new ReservationNotFoundException("Internal error message"));

            var result = await _reservationController.CancelReservation(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = (NotFoundObjectResult)result;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.Value, Is.TypeOf<Error>());
        }

        [Test]
        public async Task CancelReservation_Expect_BadRequest()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = DateTime.Now.AddDays(4);
            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _reservationManager
                .Setup(x => x.CancelReservation(It.IsAny<Guid>()))
                .Throws(new ReservationNotValidException("Internal error message"));

            var result = await _reservationController.CancelReservation(It.IsAny<Guid>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = (BadRequestObjectResult)result;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.TypeOf<Error>());
        }
    }
}

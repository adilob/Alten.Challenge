using BookingApi.Application.Exceptions;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Managers;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Interfaces;
using Moq;

namespace BookingApi.Application.Tests.Managers
{
    public class ReservationManagerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IReservationManager _reservationManager;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _reservationManager = new ReservationManager(_unitOfWork.Object);
        }

        [Test]
        public async Task NewReservation_Expects_Success()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = startReservation.AddDays(2);

            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.Add(reservation));

            _unitOfWork
                .Setup(x => x.Complete())
                .Returns(1);

            var newReservationResult = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.GetById(It.IsAny<Guid>()))
                .Returns(newReservationResult);

            var result = await _reservationManager.NewReservation(reservation);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void NewReservation_4_Days_Expects_Fail()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(1);
            var endReservation = startReservation.AddDays(4);

            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.Add(reservation));

            _unitOfWork
                .Setup(x => x.Complete())
                .Returns(1);

            var newReservationResult = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.GetById(It.IsAny<Guid>()))
                .Returns(newReservationResult);

            Assert.That(async () => await _reservationManager.NewReservation(reservation),
                Throws.TypeOf<ReservationNotValidException>());
        }

        [Test]
        public void NewReservation_More_Than_30_Days_In_Advance_Expects_Fail()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(32);
            var endReservation = startReservation.AddDays(4);

            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.Add(reservation));

            _unitOfWork
                .Setup(x => x.Complete())
                .Returns(1);

            var newReservationResult = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.GetById(It.IsAny<Guid>()))
                .Returns(newReservationResult);

            Assert.That(async () => await _reservationManager.NewReservation(reservation),
                Throws.TypeOf<ReservationNotValidException>());
        }

        [Test]
        public void NewReservation_Start_Reservation_Starts_In_Past_Expects_Fail()
        {
            var customerId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startReservation = DateTime.Now.AddDays(-1);
            var endReservation = startReservation.AddDays(4);

            var reservation = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.Add(reservation));

            _unitOfWork
                .Setup(x => x.Complete())
                .Returns(1);

            var newReservationResult = new Reservation(customerId, roomId, startReservation, endReservation);

            _unitOfWork
                .Setup(x => x.Reservations.GetById(It.IsAny<Guid>()))
                .Returns(newReservationResult);

            Assert.That(async () => await _reservationManager.NewReservation(reservation),
                Throws.TypeOf<ReservationNotValidException>());
        }
    }
}

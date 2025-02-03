using Moq;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.Interfaces;

public class ReturnReservedVehicleHandlerTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork = new();
    private readonly Mock<IReservationRepository> mockReservationRepository = new();
    private readonly ReturnReservedVehicleCommandHandler handler;
    private readonly ReturnReservedVehicleCommand command;

    public ReturnReservedVehicleHandlerTest()
    {
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.ReservationRepository).Returns(mockReservationRepository.Object);
        handler = new ReturnReservedVehicleCommandHandler(mockUnitOfWork.Object);

        command = new ReturnReservedVehicleCommand
        {
            ReservationId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Vehicle_Is_Returned_Successfully()
    {
        mockReservationRepository.Setup(reservation => reservation.ReturnReservationAsync(command.ReservationId));
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.Complete()).ReturnsAsync(true);

        await handler.Handle(command, CancellationToken.None);

        mockReservationRepository.Verify(reservation => reservation.ReturnReservationAsync(command.ReservationId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Return_Fails()
    {
        mockReservationRepository.Setup(rr => rr.ReturnReservationAsync(command.ReservationId));
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.Complete()).ReturnsAsync(false);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_ContractId_Not_Found()
    {
        mockReservationRepository.Setup(reservation => reservation.ReturnReservationAsync(command.ReservationId)).Throws(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Is_Already_Returned()
    {
        mockReservationRepository.Setup(reservation => reservation.ReturnReservationAsync(command.ReservationId)).Throws(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }
}
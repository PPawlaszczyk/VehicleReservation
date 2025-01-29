using Microsoft.OpenApi.Validations;
using Moq;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

public class ReturnReservedVehicleHandlerTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IReservationRepository> mockReservationRepository;
    private readonly ReturnReservedVehicleCommandHandler handler;
    private readonly ReturnReservedVehicleCommand command;

    public ReturnReservedVehicleHandlerTest()
    {
        mockUnitOfWork = new();
        mockReservationRepository = new();
        mockUnitOfWork.Setup(u => u.ReservationRepository).Returns(mockReservationRepository.Object);
        handler = new ReturnReservedVehicleCommandHandler(mockUnitOfWork.Object);

        command = new ReturnReservedVehicleCommand
        {
            ReservationId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Vehicle_Is_Returned_Successfully()
    {
        mockReservationRepository.Setup(rr => rr.ReturnReservation(command.ReservationId));
        mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(true);

        await handler.Handle(command, CancellationToken.None);

        mockReservationRepository.Verify(rr => rr.ReturnReservation(command.ReservationId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Return_Fails()
    {
        mockReservationRepository.Setup(rr => rr.ReturnReservation(command.ReservationId));
        mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(false);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_ContractId_Not_Found()
    {
        mockReservationRepository.Setup(rr => rr.ReturnReservation(command.ReservationId)).Throws(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Is_Already_Returned()
    {
        mockReservationRepository.Setup(rr => rr.ReturnReservation(command.ReservationId)).Throws(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }
}
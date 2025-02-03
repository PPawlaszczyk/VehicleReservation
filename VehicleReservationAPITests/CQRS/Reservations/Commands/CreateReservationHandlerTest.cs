using Moq;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

public class CreateReservationHandlerTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork = new();
    private readonly Mock<IVehiclesRepository> mockVehiclesRepository = new();
    private readonly Mock<IReservationRepository> mockReservationRepository = new();
    private readonly Mock<IUserRepository> mockUserRepository = new();
    private readonly CreateReservationCommandHandler handler;
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    private readonly DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    private readonly CreateReservationCommand command;

    public CreateReservationHandlerTest()
    {
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.VehiclesRepository).Returns(mockVehiclesRepository.Object);
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.ReservationRepository).Returns(mockReservationRepository.Object);
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.UserRepository).Returns(mockUserRepository.Object);

        handler = new CreateReservationCommandHandler(mockUnitOfWork.Object);

        command = new CreateReservationCommand
        {
            StartDate = today,
            EndDate = tomorrow,
            VehicleId = Guid.NewGuid(),
            AppUserId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task Handle_Should_Create_Reservation_When_Vehicle_Is_Available_And_No_Conflict()
    {
        // Arrange

        VehicleBuilder vehicleBuilder = new();
        vehicleBuilder.WithId(command.VehicleId);
        var vehicle = vehicleBuilder.Build();

        ReservationBuilder reservationBuilder = new();
        reservationBuilder.WithVehicleId(vehicle.Id);
        reservationBuilder.WithCustomerId(command.AppUserId);
        reservationBuilder.WithStatus(Status.Returned);

        var reservation = reservationBuilder.Build();

        mockUserRepository.Setup(user => user.GetUserByIdAsync(command.AppUserId.ToString())).ReturnsAsync(
            new AppUser
            {
                Created = DateTime.UtcNow,
            });
        mockVehiclesRepository.Setup(vehicle => vehicle.GetVehicleByIdAsync(command.VehicleId)).ReturnsAsync(vehicle);
        mockReservationRepository.Setup(reservation => reservation.IsVehicleReservatedAsync(command.StartDate, command.EndDate, command.VehicleId)).ReturnsAsync(false);
        mockReservationRepository.Setup(reservation => reservation.AddVehicleReservation(command)).Returns(reservation);
        mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(true);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Is_Not_Available()
    {
        // Arrange

        VehicleBuilder vehicleBuilder = new();
        vehicleBuilder.WithIsAvailable(false);
        var vehicle = vehicleBuilder.Build();

        mockUserRepository.Setup(user => user.GetUserByIdAsync(command.AppUserId.ToString())).ReturnsAsync(
            new AppUser
            {
                Created = DateTime.UtcNow,
            });
        mockVehiclesRepository.Setup(vehicle => vehicle.GetVehicleByIdAsync(command.VehicleId)).ReturnsAsync(vehicle);

        // Act & Assert
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));    
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Is_Already_Reserved()
    {
        // Arrange

        VehicleBuilder vehicleBuilder = new();
        vehicleBuilder.WithId(command.VehicleId);
        var vehicle = vehicleBuilder.Build();

        ReservationBuilder reservationBuilder = new();
        reservationBuilder.WithVehicleId(vehicle.Id);
        reservationBuilder.WithCustomerId(command.AppUserId);
        reservationBuilder.WithStatus(Status.Reserved);
        var existingReservation = reservationBuilder.Build();

        mockUserRepository.Setup(user => user.GetUserByIdAsync(command.AppUserId.ToString())).ReturnsAsync(
            new AppUser { 
                Created = DateTime.UtcNow,
            });
        mockVehiclesRepository.Setup(vehicle => vehicle.GetVehicleByIdAsync(command.VehicleId)).ReturnsAsync(vehicle);
        mockReservationRepository.Setup(reservation => reservation.IsVehicleReservatedAsync(command.StartDate, command.EndDate, command.VehicleId)).ReturnsAsync(true);

        // Act & Assert

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }
}
using Moq;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

public class CreateVehicleHandlerTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork = new();
    private readonly Mock<IVehiclesRepository> mockVehiclesRepository = new();
    private readonly CreateVehicleHandler handler;
    private readonly CreateVehicleCommand command;

    public CreateVehicleHandlerTest()
    {
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.VehiclesRepository).Returns(mockVehiclesRepository.Object);
        handler = new CreateVehicleHandler(mockUnitOfWork.Object);
        command = new CreateVehicleCommand
        {
            Name = "Toyota Highlander",
            Type = VehicleType.Car,
            Mark = "Toyota",
            Seats = 7,
            Fuel = "Gasoline",
            Year = 2020,
            Cost = 250.00,
            RegistrationNumber = "ABC1234"
        };
    }

    [Fact]
    public async Task Handle_Should_Return_Vehicle_Id_When_Vehicle_Is_Created_Successfully()
    {
        // Arrange

        var newVehicle = new VehicleBuilder()
             .WithName(command.Name)
             .WithType(command.Type)
             .WithMark(command.Mark)
             .WithSeats(command.Seats)
             .WithFuel(command.Fuel)
             .WithYear(command.Year)
             .WithCost(command.Cost)
             .WithRegistrationNumber(command.RegistrationNumber)
             .Build();

        mockVehiclesRepository.Setup(vehicle => vehicle.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(false);

        mockVehiclesRepository.Setup(vehicle => vehicle.AddVehicle(command))
                              .Returns(newVehicle);

        mockUnitOfWork.Setup(unitOfWork => unitOfWork.Complete()).ReturnsAsync(true);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        Assert.NotEqual(Guid.Empty, result);
        mockVehiclesRepository.Verify(vehicle => vehicle.IsVehicleRegistrationExistsAsync(command.RegistrationNumber), Times.Once);
        mockVehiclesRepository.Verify(vehicle => vehicle.AddVehicle(command), Times.Once);
        mockUnitOfWork.Verify(unitOfWork => unitOfWork.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Registration_Exists()
    {
        // Arrange

        mockVehiclesRepository.Setup(v => v.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(true);

        // Act & Assert

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("This registration number already exists.", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Creation_Fails()
    {
        // Arrange

        mockVehiclesRepository.Setup(vehicle => vehicle.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(false);

        mockVehiclesRepository.Setup(vehicle => vehicle.AddVehicle(command))
                              .Returns((Vehicle)null);

        mockUnitOfWork.Setup(unitOfWork => unitOfWork.Complete()).ReturnsAsync(false);

        // Act & Assert

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Couldn't add new car.", exception.Message);
    }
}
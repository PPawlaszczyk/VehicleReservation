using Moq;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

public class CreateVehicleHandlerTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IVehiclesRepository> mockVehiclesRepository;
    private readonly CreateVehicleHandler handler;
    private readonly CreateVehicleCommand command;

    public CreateVehicleHandlerTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockVehiclesRepository = new Mock<IVehiclesRepository>();
        mockUnitOfWork.Setup(u => u.VehiclesRepository).Returns(mockVehiclesRepository.Object);
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

        mockVehiclesRepository.Setup(v => v.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(false);

        mockVehiclesRepository.Setup(v => v.AddVehicle(command))
                              .Returns(newVehicle);

        mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(true);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        Assert.NotEqual(Guid.Empty, result);
        mockVehiclesRepository.Verify(v => v.IsVehicleRegistrationExistsAsync(command.RegistrationNumber), Times.Once);
        mockVehiclesRepository.Verify(v => v.AddVehicle(command), Times.Once);
        mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Registration_Exists()
    {
        // Arrange

        mockVehiclesRepository.Setup(v => v.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(true);  // Simulate that the registration number already exists

        // Act & Assert

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("This registration number already exists.", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Vehicle_Creation_Fails()
    {
        // Arrange

        mockVehiclesRepository.Setup(v => v.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
                              .ReturnsAsync(false);

        mockVehiclesRepository.Setup(v => v.AddVehicle(command))
                              .Returns((Vehicle)null);  // Simulate that vehicle creation failed

        mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(false);  // Simulate that unit of work didn't succeed

        // Act & Assert

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Couldn't add new car.", exception.Message);
    }
}
using Moq;
using VehicleReservationAPI.CQRS.Vehicles.Queries;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

public class GetAvailableVehiclesHandlerTests
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork = new();
    private readonly Mock<IVehiclesRepository> mockVehiclesRepository = new();
    private readonly GetAvailableVehicleQueryHandler handler;

    public GetAvailableVehiclesHandlerTests()
    {
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.VehiclesRepository).Returns(mockVehiclesRepository.Object);
        handler = new GetAvailableVehicleQueryHandler(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Available_Vehicles_When_Vehicles_Are_Available()
    {
        // Arrange

        var command = new GetAvailableVehicleQuery
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            Type = VehicleType.Car
        };

        var availableVehicles = new List<GetAvailableVehiclesDto>
        {
            new GetAvailableVehiclesDto
            {
                VehicleId = Guid.NewGuid(),
                Name = "Toyota Highlander",
                Type = VehicleType.Car,
                Mark = "Toyota",
                Seats = 7,
                Fuel = "Gasoline",
                Year = 2020,
                Cost = 250.00
            },
            new GetAvailableVehiclesDto
            {
                VehicleId = Guid.NewGuid(),
                Name = "Honda Pilot",
                Type = VehicleType.Car,
                Mark = "Honda",
                Seats = 8,
                Fuel = "Gasoline",
                Year = 2021,
                Cost = 270.00
            }
        };

        mockVehiclesRepository.Setup(vehicle => vehicle.GetAvailbleVehiclesAsync(command.StartDate, command.EndDate, command.Type))
                              .ReturnsAsync(availableVehicles);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Vehicles_Are_Available()
    {
        // Arrange

        var command = new GetAvailableVehicleQuery
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            Type = VehicleType.Car
        };

        mockVehiclesRepository.Setup(vehicle => vehicle.GetAvailbleVehiclesAsync(command.StartDate, command.EndDate, command.Type))
                              .ReturnsAsync(new List<GetAvailableVehiclesDto>());

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_When_Command_Is_Invalid()
    {
        // Arrange

        var command = new GetAvailableVehicleQuery
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            Type = VehicleType.Car
        };

        // Act & Assert

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
    }
}
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;

public class VehicleBuilder
{
    private Vehicle vehicle;

    public VehicleBuilder()
    {
        vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Name = "Default Vehicle",
            Type = VehicleType.Car,
            Created = DateTime.UtcNow,
            Mark = "Default Mark",
            Seats = 4,
            Fuel = "Petrol",
            Year = 2025,
            Cost = 50.0,
            RegistrationNumber = "ABC123",
            IsAvailable = true
        };
    }

    public VehicleBuilder WithId(Guid id)
    {
        vehicle.Id = id;
        return this;
    }

    public VehicleBuilder WithType(VehicleType type)
    {
        vehicle.Type = type;
        return this;
    }

    public VehicleBuilder WithName(string name)
    {
        vehicle.Name = name;
        return this;
    }

    public VehicleBuilder WithMark(string mark)
    {
        vehicle.Mark = mark;
        return this;
    }

    public VehicleBuilder WithSeats(int seats)
    {
        vehicle.Seats = seats;
        return this;
    }

    public VehicleBuilder WithFuel(string fuel)
    {
        vehicle.Fuel = fuel;
        return this;
    }

    public VehicleBuilder WithYear(int year)
    {
        vehicle.Year = year;
        return this;
    }

    public VehicleBuilder WithCost(double cost)
    {
        vehicle.Cost = cost;
        return this;
    }

    public VehicleBuilder WithRegistrationNumber(string registrationNumber)
    {
        vehicle.RegistrationNumber = registrationNumber;
        return this;
    }

    public VehicleBuilder WithIsAvailable(bool isAvailable)
    {
        vehicle.IsAvailable = isAvailable;
        return this;
    }

    public Vehicle Build()
    {
        return vehicle;
    }
}
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;

public class ReservationBuilder
{
    private Reservation reservation;

    public ReservationBuilder()
    {
        reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            AppUserId = Guid.NewGuid(),
            Status = Status.Reserved,
            Created = DateTime.UtcNow,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };
    }

    public ReservationBuilder WithId(Guid id)
    {
        reservation.Id = id;
        return this;
    }

    public ReservationBuilder WithVehicleId(Guid vehicleId)
    {
        reservation.VehicleId = vehicleId;
        return this;
    }

    public ReservationBuilder WithCustomerId(Guid customerId)
    {
        reservation.AppUserId = customerId;
        return this;
    }

    public ReservationBuilder WithStartDate(DateOnly startDate)
    {
        reservation.StartDate = startDate;
        return this;
    }

    public ReservationBuilder WithEndDate(DateOnly endDate)
    {
        reservation.EndDate = endDate;
        return this;
    }

    public ReservationBuilder WithStatus(Status status)
    {
        reservation.Status = status;
        return this;
    }

    public ReservationBuilder WithCreated(DateTime created)
    {
        reservation.Created = created;
        return this;
    }

    public Reservation Build()
    {
        return reservation;
    }
}
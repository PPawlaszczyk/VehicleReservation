namespace VehicleReservationAPI.DTOs
{
    public class CreateReservationDto
    {
        public required DateOnly EndDate { get; init; }
        public required DateOnly StartDate { get; init; }
        public required Guid VehicleId { get; init; }
    }
}
namespace VehicleReservationAPI.DTOs
{
    public record UserReservationDto
    {
        public Guid Id { get; init; }
        public required VehicleForUserReservationDto Vehicle { get; init; }
        public required DateOnly EndDate { get; init; }
        public required DateOnly StartDate { get; init; }
    }
}
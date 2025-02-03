namespace VehicleReservationAPI.DTOs
{
    public record GetExpiringReservationsDto
    {
        public required Guid AppUserId { get; init; }
        public required string RegistrationNumber { get; init; }
        public required string Name { get; init; }
        public required DateOnly ReturnDate { get; init; }
    }
}
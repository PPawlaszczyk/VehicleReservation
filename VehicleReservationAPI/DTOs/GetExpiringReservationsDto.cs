namespace VehicleReservationAPI.DTOs
{
    public class GetExpiringReservationsDto
    {
        public required Guid AppUserId { get; set; }
        public required string RegistrationNumber { get; set; }
        public required string Name { get; set; }

    }
}
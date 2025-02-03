using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.DTOs
{
    public record VehicleForUserReservationDto
    {
        public required string Name { get; init; }
        public required VehicleType Type { get; init; }
        public required string Mark { get; init; }
        public required int Seats { get; init; }
        public required string Fuel { get; init; }
        public required int Year { get; init; }
        public required double Cost { get; init; }
        public required string RegistrationNumber { get; init; }
    }
}
using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.DTOs
{
    public class GetAvailableVehiclesDto
    {
        public required Guid VehicleId { get; set; }
        public required string Name { get; set; }
        public required VehicleType Type { get; set; }
        public required string Mark { get; set; }
        public required int Seats { get; set; }
        public required string Fuel { get; set; }
        public required int Year { get; set; }
        public required double Cost { get; set; }
    }
}
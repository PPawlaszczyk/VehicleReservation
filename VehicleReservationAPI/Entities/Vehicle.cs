using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required VehicleType Type { get; set; }
        public DateTime? IsDeletedOrNull { get; set; }
        public required DateTime Created { get; set; }
        public required string Mark { get; set; }
        public required int Seats { get; set; }
        public required string Fuel { get; set; }
        public required int Year { get; set; }
        public required double Cost { get; set; }
        public required string RegistrationNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
        public ICollection<Reservation>? Reservations { get; set; }
    }
}

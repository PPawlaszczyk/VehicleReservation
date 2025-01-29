using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Vehicle? Vehicle { get; set; }
        public required Guid VehicleId { get; set; }
        public required Guid AppUserId { get; set; }
        public required DateOnly EndDate { get; set; }
        public required DateOnly StartDate { get; set; }
        public required Status Status { get; set; }
        public required DateTime Created { get; set; }
    }
}
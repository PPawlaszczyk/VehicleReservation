using Microsoft.AspNetCore.Identity;

namespace VehicleReservationAPI.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public required DateTime Created { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime? IsDeletedOrNull { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
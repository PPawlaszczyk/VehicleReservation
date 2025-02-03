using Microsoft.AspNetCore.Identity;

namespace VehicleReservationAPI.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
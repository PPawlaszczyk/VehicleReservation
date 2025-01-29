

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.Entities;

namespace VehicleReservationAPI.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
           .HasMany(ur => ur.UserRoles)
           .WithOne(u => u.User)
           .HasForeignKey(ur => ur.UserId)
           .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        }

    }
}

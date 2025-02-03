using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Data.Repositories
{
    public class UserRepository(DataContext context) : IUserRepository
    {
        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await context.Users.FindAsync(Guid.Parse(id));
        }
    }
}
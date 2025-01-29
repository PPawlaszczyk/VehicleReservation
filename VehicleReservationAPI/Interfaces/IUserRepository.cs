using VehicleReservationAPI.Entities;

namespace VehicleReservationAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserByIdAsync(string id);

    }
}
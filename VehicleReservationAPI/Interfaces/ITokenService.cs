using VehicleReservationAPI.Entities;

namespace VehicleReservationAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
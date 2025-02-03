using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Interfaces
{
    public interface IVehiclesRepository
    {
        Vehicle AddVehicle(CreateVehicleCommand vehicle);
        Task<IEnumerable<GetAvailableVehiclesDto>> GetAvailbleVehiclesAsync(DateOnly startDate, DateOnly endDate, VehicleType type);
        Task<bool> IsVehicleRegistrationExistsAsync(string registration);
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
    }
}
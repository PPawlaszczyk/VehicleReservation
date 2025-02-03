using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Data.Repositories
{
    public class VehiclesRepository(DataContext context) : IVehiclesRepository
    {
        public Vehicle AddVehicle(CreateVehicleCommand vehicle)
        {
            Vehicle newVehicle = new()
            {
                Cost = vehicle.Cost,
                Fuel = vehicle.Fuel.Trim(),
                Created = DateTime.UtcNow,
                Mark = vehicle.Mark.Trim(),
                Name = vehicle.Name.Trim(),
                Type = vehicle.Type,
                Seats = vehicle.Seats,
                Year = vehicle.Year,
                RegistrationNumber = vehicle.RegistrationNumber,
            };

            context.Vehicles.Add(newVehicle);
            return newVehicle;
        }

        public async Task<IEnumerable<GetAvailableVehiclesDto>> GetAvailbleVehiclesAsync(DateOnly startDate, DateOnly endDate, VehicleType type)
        {
            return await context.Vehicles
            .Include(x => x.Reservations)
            .Where(x => x.IsAvailable 
            && !x.IsDeletedOrNull.HasValue 
            && x.Type == type 
            && !x.Reservations
                .Any(r =>
                     (r.StartDate <= endDate &&
                     r.EndDate >= startDate)
            ))
            .Select(x=> new GetAvailableVehiclesDto 
            {
                VehicleId = x.Id,
                Cost = x.Cost, 
                Fuel = x.Fuel, 
                Mark = x.Mark, 
                Name = x.Name, 
                Seats = x.Seats,
                Type = x.Type, 
                Year = x.Year
            })
            .ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await context.Vehicles.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeletedOrNull.HasValue);  
        }

        public async Task<bool> IsVehicleRegistrationExistsAsync(string registration)
        {
            return await context.Vehicles
                .AnyAsync(x => x.RegistrationNumber.ToLower() == registration.Trim().ToLower() && !x.IsDeletedOrNull.HasValue);
        }
    }
}
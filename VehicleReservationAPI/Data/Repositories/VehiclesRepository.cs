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
            .Include(vehicle => vehicle.Reservations)
            .Where(vehicle => vehicle.IsAvailable 
            && !vehicle.IsDeletedOrNull.HasValue 
            && vehicle.Type == type 
            && !vehicle.Reservations
                .Any(reservation =>
                     (reservation.StartDate <= endDate &&
                     reservation.EndDate >= startDate)
            ))
            .Select(vehicle => new GetAvailableVehiclesDto 
            {
                VehicleId = vehicle.Id,
                Cost = vehicle.Cost, 
                Fuel = vehicle.Fuel, 
                Mark = vehicle.Mark, 
                Name = vehicle.Name, 
                Seats = vehicle.Seats,
                Type = vehicle.Type, 
                Year = vehicle.Year
            })
            .ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == id && !vehicle.IsDeletedOrNull.HasValue);  
        }

        public async Task<bool> IsVehicleRegistrationExistsAsync(string registration)
        {
            return await context.Vehicles
                .AnyAsync(vehicle => vehicle.RegistrationNumber.ToLower() == registration.Trim().ToLower() && !vehicle.IsDeletedOrNull.HasValue);
        }
    }
}
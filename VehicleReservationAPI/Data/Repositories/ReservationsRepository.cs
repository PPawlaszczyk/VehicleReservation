using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Data.Repositories
{
    public class ReservationsRepository(DataContext context) : IReservationRepository
    {
        public Reservation AddVehicleReservation(CreateReservationCommand reservation)
        {
            Reservation newVehicle = new()
            {
                Created = DateTime.UtcNow,
                StartDate = reservation.StartDate,
                AppUserId = reservation.AppUserId,
                EndDate = reservation.EndDate,
                Status = Status.Reserved,
                VehicleId = reservation.VehicleId,
            };

            context.Reservations.Add(newVehicle);
            return newVehicle;
        }

        public async Task<IEnumerable<UserReservationDto>> GetCurrentReservationsByUserAsync(Guid UserId)
        {
            return await context.Reservations
                .Include(x=>x.Vehicle)
                .Where(reservation=>reservation.AppUserId == UserId && reservation.Status == Status.Reserved)
                .Select(reservation=> new UserReservationDto
                {
                    StartDate = reservation.StartDate,
                    EndDate= reservation.EndDate,
                    Id = reservation.Id,
                    Vehicle = new VehicleForUserReservationDto
                        {
                            Cost = reservation.Vehicle.Cost,
                            Seats = reservation.Vehicle.Seats,
                            Fuel = reservation.Vehicle.Fuel,
                            Mark = reservation.Vehicle.Mark,
                            Name = reservation.Vehicle.Name,
                            RegistrationNumber = reservation.Vehicle.RegistrationNumber,
                            Type = reservation.Vehicle.Type,
                            Year = reservation.Vehicle.Year,
                        }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetExpiringReservationsDto>> GetExpiringReservationsAsync(DateOnly date)
        {
            return await context.Reservations
                .Include(reservation => reservation.Vehicle)
                .Where(reservation => reservation.EndDate <= date && reservation.Status == Status.Reserved)
                .Select(reservation => new GetExpiringReservationsDto 
                { 
                    AppUserId = reservation.AppUserId, 
                    Name = reservation.Vehicle.Name,
                    RegistrationNumber = reservation.Vehicle.RegistrationNumber,
                    ReturnDate = reservation.EndDate,
                })
                .ToListAsync();
        }
        public async Task<Reservation?> GetReservationByIdAsync(Guid id)
        {
            return await context.Reservations.FirstOrDefaultAsync(reservation => reservation.Id == id);
        }

        public async Task<bool> IsVehicleReservatedAsync(DateOnly startDate, DateOnly endDate, Guid vehicleid)
        {
            return await context.Reservations
                .Where(reservation => reservation.VehicleId == vehicleid)
                .AnyAsync(reservation => 
                startDate < reservation.EndDate 
                && endDate > reservation.StartDate 
                );
        }

        public async Task ReturnReservationAsync(Guid reservationId)
        {
            var reservation = await context.Reservations.FirstOrDefaultAsync(reservation => reservation.Id == reservationId);

            if (reservation != null && reservation.Status == Status.Reserved)
            {
                reservation.Status = Status.Returned;
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.CQRS.Account.Commands;
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

        public async Task<IEnumerable<GetExpiringReservationsDto>> GetExpiringReservationsAsync(DateOnly date)
        {
            return await context.Reservations
                .Include(r => r.Vehicle)
                .Where(r => r.EndDate == date && r.Status == Status.Reserved)
                .Select(reservation => new GetExpiringReservationsDto 
                { 
                    AppUserId = reservation.AppUserId, 
                    Name = reservation.Vehicle.Name,
                    RegistrationNumber = reservation.Vehicle.RegistrationNumber
                })
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationById(Guid id)
        {
            return await context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> IsVehicleReservatedAsync(DateOnly startDate, DateOnly endDate, Guid vehicleid)
        {
            return await context.Reservations
                .Where(reservation => reservation.VehicleId == vehicleid)
                .AnyAsync(reservation => startDate < reservation.EndDate && endDate > reservation.StartDate);
        }

        public void ReturnReservation(Guid reservationId)
        {
            var reservation = context.Reservations.FirstOrDefault(reservation => reservation.Id == reservationId);

            if (reservation != null && reservation.Status == Status.Reserved)
            {
                reservation.Status = Status.Returned;
            }
        }
    }
}
using VehicleReservationAPI.CQRS.Account.Commands;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;

namespace VehicleReservationAPI.Interfaces
{
    public interface IReservationRepository
    {
        Reservation AddVehicleReservation(CreateReservationCommand reservation);
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task<IEnumerable<UserReservationDto>> GetAllReservationsByUserAsync(Guid UserId);
        void ReturnReservation(Guid reservationId);
        Task<IEnumerable<GetExpiringReservationsDto>> GetExpiringReservationsAsync(DateOnly date);
        Task<bool> IsVehicleReservatedAsync(DateOnly startDate, DateOnly endDate, Guid vehicleid);
    }
}
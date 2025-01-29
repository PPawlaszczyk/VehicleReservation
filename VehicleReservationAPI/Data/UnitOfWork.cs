
using VehicleReservationAPI.Data;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Data
{
    public class UnitOfWork(DataContext context, IReservationRepository reservationRepository, IVehiclesRepository vehiclesRepository, IUserRepository userRepository) : IUnitOfWork
    {


        public IReservationRepository ReservationRepository => reservationRepository;

        public IVehiclesRepository VehiclesRepository => vehiclesRepository;

        public IUserRepository UserRepository => userRepository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}

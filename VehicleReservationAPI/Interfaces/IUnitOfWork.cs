namespace VehicleReservationAPI.Interfaces
{
    public interface IUnitOfWork
    {
        public IReservationRepository ReservationRepository { get; }
        public IVehiclesRepository VehiclesRepository { get; }
        public IUserRepository UserRepository{ get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
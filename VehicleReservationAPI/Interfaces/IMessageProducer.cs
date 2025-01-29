namespace VehicleReservationAPI.Interfaces
{
    public interface IMessageProducer
    {
        public Task SendingMessage<T>(T message);


    }
}

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Services
{
    public class MessageProducer : IMessageProducer
    {
        public async Task SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            var conn = await factory.CreateConnectionAsync();

            using var channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync("reservations", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await channel.BasicPublishAsync("", "reservations", body: body);
        }
    }
}
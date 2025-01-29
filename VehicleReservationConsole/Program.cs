using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "rabbitmq",
    UserName = "user",
    Password = "mypass",
    VirtualHost = "/",
};

var conn = await factory.CreateConnectionAsync();

using var channel = await conn.CreateChannelAsync();

await channel.QueueDeclareAsync("reservations", durable: true, exclusive: false);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"New resevation is initialized: {message}");
    await Task.CompletedTask;
};

await channel.BasicConsumeAsync("reservations", true, consumer);

while(!File.Exists("closeApp"))
{
    await Task.Delay(5000);
}
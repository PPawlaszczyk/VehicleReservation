using Microsoft.AspNetCore.SignalR;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.SignalR;

public class ReservationNotifier : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<ReservationHub> _hubContext;

    public ReservationNotifier(IServiceScopeFactory serviceScopeFactory, IHubContext<ReservationHub> hubContext)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAndNotifyExpiringReservations();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task CheckAndNotifyExpiringReservations()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationRepository>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var expiringReservations = await reservationService.GetExpiringReservationsAsync(today);

        foreach (var reservation in expiringReservations)
        {
            var message = $"Your reservation for vehicle {reservation.Name} with registration {reservation.RegistrationNumber}" +
                $" need to be returned until {reservation.ReturnDate} till 12:00";
            await _hubContext.Clients.User(reservation.AppUserId.ToString()).SendAsync("ReceiveNotification", message);
        }
    }
}
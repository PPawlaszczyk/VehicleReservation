using Microsoft.AspNetCore.SignalR;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.SignalR;

namespace VehicleReservationAPI.Services
{
    public class ReservationNotifier(IHubContext<ReservationHub> hubContext, IUnitOfWork unitOfWork) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var datetimeNow = DateOnly.FromDateTime(DateTime.UtcNow);

                var expiringReservations = await unitOfWork.ReservationRepository.GetExpiringReservationsAsync(datetimeNow);

                foreach (var reservation in expiringReservations)
                {
                    if (reservation.AppUserId != Guid.Empty)
                    {
                        var message = $"The reservation for the vehicle {reservation.Name} is ending soon.";
                        await hubContext.Clients.User(reservation.AppUserId.ToString())
                            .SendAsync("ReceiveNotification", message, cancelToken);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancelToken);
            }
        }
    }
}
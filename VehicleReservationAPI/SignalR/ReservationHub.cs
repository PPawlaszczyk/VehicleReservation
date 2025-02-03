using Microsoft.AspNetCore.SignalR;
using VehicleReservationAPI.Extensions;

namespace VehicleReservationAPI.SignalR
{
    public class ReservationHub : Hub
    {
        public async Task NotifyUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
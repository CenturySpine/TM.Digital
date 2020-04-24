using Microsoft.AspNetCore.SignalR;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services
{
    public static class Hubconcentrator
    {
        public static IHubContext<ClientNotificationHub> Hub { get; set; }
    }
}
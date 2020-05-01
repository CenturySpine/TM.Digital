using Microsoft.AspNetCore.SignalR.Client;

namespace TM.Digital.Client.Screens.ActionChoice
{
    internal interface IComponentConfigurable
    {
        void RegisterSubscriptions(HubConnection hubConnection);
    }
}
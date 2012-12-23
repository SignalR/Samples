using Microsoft.AspNet.SignalR.Hubs;

namespace BasicChat.SelfHost.Hubs
{
    public class Chat : Hub
    {
        public void Send(string message)
        {
            Clients.All.send(message);
        }
    }
}

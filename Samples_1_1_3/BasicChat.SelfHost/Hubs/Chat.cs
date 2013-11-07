using Microsoft.AspNet.SignalR;

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

using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BasicChat
{
    public class ChatWithTracking : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public void Send(string message)
        {
            string name = Context.QueryString["name"];

            Clients.All.send(name + ": " + message);
        }

        public void Send(string who, string message)
        {
            string name = Context.QueryString["name"];

            foreach (var connectionId in _connections.GetConnections(who))
            {
                Clients.Client(connectionId).send(name + ": " + message);
            }
        }

        public override Task OnConnected()
        {
            string name = Context.QueryString["name"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            string name = Context.QueryString["name"];

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected();
        }        
    }
}
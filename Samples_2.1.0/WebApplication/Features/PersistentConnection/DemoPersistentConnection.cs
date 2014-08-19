using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace WebApplication.Features.SamplePersistentConnection
{
    public class DemoPersistentConnection : PersistentConnection
    {       
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return Connection.Broadcast("OnConnected " + connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            return Connection.Broadcast("OnDisconnected " + connectionId);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var message = JsonConvert.DeserializeObject<Message>(data);           

            switch(message.Type)
            {
                case "sendToMe":
                    Connection.Send(connectionId, message.Content);
                    break;
                case "sendToConnectionId":
                    Connection.Send(message.ConnectionId, message.Content);
                    break;
                case "sendBroadcast":
                    Connection.Broadcast(message.Content);
                    break;
                case "sendToGroup":
                    Groups.Send(message.GroupName, message.Content);
                    break;
                case "joinGroup":
                    Groups.Add(message.ConnectionId, message.GroupName);
                    Connection.Broadcast(message.ConnectionId + " joined group " + message.GroupName);
                    break;
                case "leaveGroup":
                    Groups.Remove(message.ConnectionId, message.GroupName);
                    Connection.Broadcast(message.ConnectionId + " left group " + message.GroupName);
                    break;
                case "throw":
                    throw new InvalidOperationException("Client does not receive this error");
                    break;
            }

            return base.OnReceived(request, connectionId, data);
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return Connection.Send(connectionId, "OnReconnected");
        }
    }
}
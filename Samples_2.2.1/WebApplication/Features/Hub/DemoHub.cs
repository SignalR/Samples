﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace WebApplication.Features.SampleHub
{
    public class DemoHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.All.hubMessage("OnConnected " + Context.ConnectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return Clients.All.hubMessage("OnDisconnected(stopCalled: " + stopCalled + "): " + Context.ConnectionId);
        }

        public override Task OnReconnected()
        {
            return Clients.Caller.hubMessage("OnReconnected");
        }

        public void SendToMe(string value)
        {
            Clients.Caller.hubMessage(value);
        }

        public void SendToConnectionId(string connectionId, string value)
        {
            Clients.Client(connectionId).hubMessage(value);
        }

        public void SendToAll(string value)
        {
            Clients.All.hubMessage(value);
        }

        public void SendToGroup(string groupName, string value)
        {
            Clients.Group(groupName).hubMessage(value);
        }

        public void JoinGroup(string groupName, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                connectionId = Context.ConnectionId;    
            }
            
            Groups.Add(connectionId, groupName);
            Clients.All.hubMessage(connectionId + " joined group " + groupName);
        }

        public void LeaveGroup(string groupName, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                connectionId = Context.ConnectionId;
            }
            
            Groups.Remove(connectionId, groupName);
            Clients.All.hubMessage(connectionId + " left group " + groupName);
        }

        public void IncrementClientVariable()
        {
            Clients.Caller.counter = Clients.Caller.counter + 1;
            Clients.Caller.hubMessage("Incremented counter to " + Clients.Caller.counter);
        }

        public void ThrowOnVoidMethod()
        {
            throw new InvalidOperationException("ThrowOnVoidMethod");
        }

        public async Task ThrowOnTaskMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            throw new InvalidOperationException("ThrowOnTaskMethod");
        }

        public void ThrowHubException()
        {
            throw new HubException("ThrowHubException", new { Detail = "I can provide additional error information here!" });
        }

        public async Task StartBackgroundThread()
        {
            BackgroundThread.Enabled = true;
            await BackgroundThread.SendOnPersistentConnection();
            await BackgroundThread.SendOnHub();
        }

        public void StopBackgroundThread()
        {
            BackgroundThread.Enabled = false;
        }
    }
}
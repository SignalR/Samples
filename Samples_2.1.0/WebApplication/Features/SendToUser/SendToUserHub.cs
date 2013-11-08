using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication.Features.SendToUser
{
    public class SendToUserHub : Hub
    {
        public void SendToMe(string value)
        {
            Clients.User(Context.User.Identity.Name).message(value);
        }

        public void SendToUser(string userId, string value)
        {
            Clients.User(userId).message(value);
        }

        public void SendToUsers(IList<string> userIds, string value)
        {
            Clients.Users(userIds).message(value);
        }
    }
}
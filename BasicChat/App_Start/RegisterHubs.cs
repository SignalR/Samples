using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

[assembly: PreApplicationStartMethod(typeof(Samples.RegisterHubs), "Start")]

namespace Samples
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            var config = new HubConfiguration
            {
                EnableCrossDomain = true
            };

            RouteTable.Routes.MapHubs(config);
        }
    }
}

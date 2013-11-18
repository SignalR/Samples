using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using WebApplication.Features.SamplePersistentConnection;

[assembly: OwinStartupAttribute(typeof(WebApplication.Startup))]
namespace WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            app.MapSignalR<DemoPersistentConnection>("/Connections/DemoPersistentConnection");
        }
    }
}

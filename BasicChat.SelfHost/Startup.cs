using Owin;

namespace BasicChat.SelfHost
{
    public class Startup
    {
        // This method name is important
        public void Configuration(IAppBuilder app)
        {
            app.MapHubs("/signalr");
        }
    }
}

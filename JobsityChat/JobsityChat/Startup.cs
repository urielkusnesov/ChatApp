using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(JobsityChat.Startup))]
namespace JobsityChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
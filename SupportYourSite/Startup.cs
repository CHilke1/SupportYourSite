using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SupportYourSite.Startup))]
namespace SupportYourSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Application.Startup))]
namespace Web_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

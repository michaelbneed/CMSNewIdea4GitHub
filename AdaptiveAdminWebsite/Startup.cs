using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FamtasticAdminWebsite.Startup))]
namespace FamtasticAdminWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

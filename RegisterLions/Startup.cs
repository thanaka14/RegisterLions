using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RegisterLions.Startup))]
namespace RegisterLions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

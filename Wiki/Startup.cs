using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wiki.Startup))]
namespace Wiki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

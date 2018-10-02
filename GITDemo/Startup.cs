using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GITDemo.Startup))]
namespace GITDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

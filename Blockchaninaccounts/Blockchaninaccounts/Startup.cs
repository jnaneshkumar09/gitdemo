using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blockchaninaccounts.Startup))]
namespace Blockchaninaccounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

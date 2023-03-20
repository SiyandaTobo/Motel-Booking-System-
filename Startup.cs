using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Motel_BOoking.Startup))]
namespace Motel_BOoking
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BNR_ComputerClass.Startup))]
namespace BNR_ComputerClass
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BBSSite.Startup))]
namespace BBSSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

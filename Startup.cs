using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VaccinationManager.Startup))]
namespace VaccinationManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CandidateManager.Web.Startup))]
namespace CandidateManager.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

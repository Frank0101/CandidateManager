using CandidateManager.Web.Infrastructure;
using Castle.Windsor;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CandidateManager.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Creates Castle Container
            var container = new WindsorContainer();
            container.Install(new CastleInstaller());

            //Creates Castle Controller Factory (MVC)
            var castleControllerFactory = new CastleControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(castleControllerFactory);
        }
    }
}

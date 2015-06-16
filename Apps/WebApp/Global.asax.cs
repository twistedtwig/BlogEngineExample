using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BlogEngine.Domain;
using BlogEngine.Domain.Interfaces;
using Castle.MicroKernel.Registration;
using HoHUtilities.Mvc.Windsor;
using Mvc.Windsor;
using WebApp.Models;

namespace WebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new WindsorContainerGeneration().SetupWithWebRequest();
            container.RegisterControllers(Assembly.GetExecutingAssembly());

            AutoMapperWebConfiguration.Configure();

            //setup the website to get its user from aspnet.
            container.Register(Component.For<IGetCurrentUserName>().LifeStyle.PerWebRequest.ImplementedBy(typeof(GetCurrentUserNameUsingAspnet)));

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }

    }
}

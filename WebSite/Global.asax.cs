using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using System.Threading.Tasks;

using BusinessLogic.Models;

using MessageServices.Identity;

namespace WebSite
{
    using Core.Identity;
    using Helpers;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SimpleInjectorConfig.RegisterDependencyInjection();

            // DataTables.AspNet registration with default options.
            DataTables.AspNet.Mvc5.Configuration.RegisterDataTables();

            //Use for Code-First only
            //Database.SetInitializer(new DBDevelopmentData(DependencyResolver.Current.GetService<IIdentityEmailService>(), DependencyResolver.Current.GetService<IIdentitySmsService>()));
        }
    }
}
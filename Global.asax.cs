using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VaccinationManager.DAL;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity;
using System.Web.Helpers;

namespace VaccinationManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new VaccinationInterceptorTransientErrors());
            DbInterception.Add(new VaccinationInterceptorLogging());
            Database.SetInitializer<VaccinationContext>(null);
        }
    }
}

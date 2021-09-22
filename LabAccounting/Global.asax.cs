using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LabAccounting
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LabAccEntity.NHibernateHelper.OnStart();
            AuthorizationModule.Initializer.Init(
                int.Parse(WebConfigurationManager.AppSettings["AuthSiteID"]),
                WebConfigurationManager.AppSettings["AuthDatabaseConnectionString"]);
        }
    }
}

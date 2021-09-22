using System.Web;
using System.Web.Mvc;

namespace LabAccounting
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizationModule.Filters.AuthorizationFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}

using SmartCattle.Web.CustomFilters;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LocalizationAttribute("fa-IR"));
           // filters.Add(new AuthenticateFilterAttribute());
        }
    }
}

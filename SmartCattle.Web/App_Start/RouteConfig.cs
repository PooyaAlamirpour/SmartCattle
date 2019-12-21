using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartCattle.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           // routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
 
            routes.MapRoute(
               name: "Language",
               url: "{language}-{culture}/{controller}/{action}/{id}",
               defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional },
               constraints:new { language = @"en|fa" , culture=@"US|IR"}
           );
             
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, language = "fa", culture = "IR" },
              namespaces: new[] { "SmartCattle.Web.Controllers" }
          );

            routes.MapRoute(
            name: "User_Login",
            url: "{controller}/{action}/{returnUrl}",
            defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional},
            namespaces: new[] { "SmartCattle.Web.Controllers" }
        );
            
        }
    }
}

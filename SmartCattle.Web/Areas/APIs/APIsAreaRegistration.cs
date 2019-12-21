using System.Web.Mvc;

namespace SmartCattle.Web.Areas.APIs
{
    public class APIsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "APIs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "APIs_default",
                "APIs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System.Web.Mvc;

namespace SmartCattle.Web.Areas.BackOffice
{
    public class BackOfficeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BackOffice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BackOffice_default",
                "BackOffice/{controller}/{action}/{id}",
                new { controller="Main", action = "Index", id = UrlParameter.Optional }
            ); 
        }
    }
}
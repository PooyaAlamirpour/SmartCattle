using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartCattle.Web.CustomFilters
{
    public class LocalizationAttribute:ActionFilterAttribute
    {
        public readonly string _defaultLanguage;
        public static String Current_Language_Culture = "fa-IR";

        public LocalizationAttribute(string defaulLanguage)
        {
           //_defaultLanguage = defaulLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string language = (string)filterContext.RouteData.Values["language"] ?? "fa";
            //string culture = (string)filterContext.RouteData.Values["culture"] ?? "IR";
            //if(filterContext.ActionParameters.Count != 0)
            //{
            //    var tmp = filterContext.ActionParameters["langCulture"];
            //    Current_Language_Culture = tmp.ToString();
            //}

            String[] splitedlangCulture = Current_Language_Culture.Split('-');
            string language = splitedlangCulture[0];
            string culture = splitedlangCulture[1];

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
            }
            catch(CultureNotFoundException ex)
            {
               // TODO
            } 
            catch(ArgumentNullException ex)
            {
                //TODO
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace SmartCattle.Web.Areas.BackOffice.CustomFilter
{
    public class SuperAdmin : FilterAttribute, IAuthenticationFilter
    {
     
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
        }


        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;             
            
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            if (user.Identity.Name.ToLower().Trim() != "p.alamirpour@gmail.com"/*"smart.cattle.ir@gmail.com"*/)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("/Account/Login"); 
            }
        }
    }
}
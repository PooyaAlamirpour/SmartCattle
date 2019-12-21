using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MvcSiteMapProvider.Collections.Specialized;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartCattle.Web.CustomFilters
{
    public class CheckRoleAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string Methpd = HttpContext.Current.Request.HttpMethod;

            var identity = (ClaimsPrincipal)filterContext.HttpContext.User;
            Claim claim = identity.Claims.Where(c => c.Type == "Role").FirstOrDefault() ?? new Claim("Role", "");
            string ActionName = filterContext.ActionDescriptor.ActionName;
            string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string roles = claim.Value;

            //var db = new SmartCattleContext();

            //if (db.ACLs.FirstOrDefault(a => a.Action == ActionName && a.Controller == ControllerName && roles.Contains(a.Role.Id)) == null)
            //{
            //    filterContext.Result = new RedirectResult("/Authorization/unAuthorized");
            //}
        }      
    }
}
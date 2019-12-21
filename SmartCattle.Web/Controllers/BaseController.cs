using SmartCattle.Web.CustomFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartCattle.Web.Controllers
{
    [AuthenticateFilter]
    public class BaseController : Controller
    {
       protected string userID;
       protected int farmID;
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                var identity = (ClaimsPrincipal)HttpContext.User;
                if (identity.FindFirst("UserID") != null && identity.FindFirst("farmID") != null)
                {
                    userID = identity.FindFirst("UserID").Value;
                    int.TryParse(identity.FindFirst("farmID").Value, out farmID);
                }
                else
                { 
                    requestContext.HttpContext.Response.Clear();
                    requestContext.HttpContext.Response.Redirect(Url.Action("Login", "Account"));
                    requestContext.HttpContext.Response.End();
                }
            }
            catch
            {
                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.Redirect(Url.Action("Login", "Account"));
                requestContext.HttpContext.Response.End();
            }
        }
        //  public ResourceManager rm;
        public BaseController()
        {
            //  rm =new ResourceManager("SmartCattle.App_LocalResources.Resource.fa-IR", System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
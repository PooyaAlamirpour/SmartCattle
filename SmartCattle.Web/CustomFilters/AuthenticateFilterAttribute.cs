using MvcSiteMapProvider.Collections.Specialized;
using NHibernate;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace SmartCattle.Web.CustomFilters
{
    public class AuthenticateFilterAttribute : FilterAttribute, IAuthenticationFilter
    {
        #region Private
        private void CheckPermissions(List<UserInfo> user_permissions, String NeededPermission, AuthenticationChallengeContext filterContext, String ControllerName, String ActionName)
        {
            if (user_permissions.Count != 0)
            {
                String userPermissionsStr = Helper.Helper.getPermissionList(user_permissions[0].RoleId);
                if (!userPermissionsStr.Contains(NeededPermission) && NeededPermission != "Home-Alerts")
                {
                    SetRedirectToHomeErrorPageForContext(filterContext, ActionName, ControllerName);
                }
                else
                {
                    String CurrentControlActionName = ControllerName + "_" + ActionName;
                    if (LastControlActionName != CurrentControlActionName)
                    {
                        if (!CurrentControlActionName.Contains("changeLanguage"))
                        {
                            ISession mContext = Context.Open();
                            ActionControllerListTbl ac = mContext.QueryOver<ActionControllerListTbl>().Where(x => x.Action == ActionName && x.Controller == ControllerName).SingleOrDefault();
                            bool isPartialView = ac.PartialView;
                            Context.Close(mContext);
                            if (!isPartialView)
                            {
                                var tmp1 = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                                var tmp2 = Thread.CurrentThread.CurrentCulture.Name.Split('-')[1];
                                filterContext.Result = new RedirectToRouteResult("Language",
                                new System.Web.Routing.RouteValueDictionary{
                                    {"language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName },
                                    {"culture", Thread.CurrentThread.CurrentCulture.Name.Split('-')[1] },
                                    {"controller", ControllerName},
                                    {"action", ActionName}});
                                LastControlActionName = CurrentControlActionName;
                            }
                        }
                    }
                }
            }
            else
            {
                SetRedirectToHomeErrorPageForContext(filterContext, ActionName, ControllerName);
            }
        }

        private static void SetRedirectToHomeErrorPageForContext(AuthenticationChallengeContext filterContext, string ActionName, string ControllerName)
        {
            var tmp1 = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var tmp2 = Thread.CurrentThread.CurrentCulture.Name.Split('-')[1];

            ISession mContext = Context.Open();
            ActionControllerListTbl ac = mContext.QueryOver<ActionControllerListTbl>().Where(x => x.Action == ActionName && x.Controller == ControllerName).SingleOrDefault();
            bool isPartialView;

            if (ac != null)
                isPartialView = ac.PartialView;
            else
                isPartialView = false;

            if (isPartialView)
            {
                filterContext.Result = new PartialViewResult
                {
                    ViewName = "~/Views/Home/Alerts.cshtml"
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult("Language",
                    new System.Web.Routing.RouteValueDictionary{
                    {"language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName },
                    {"culture", Thread.CurrentThread.CurrentCulture.Name.Split('-')[1] },
                    {"controller", "Home"},
                    {"action", "Alerts"}
                    });
            }

        }

        private static void SetRedirectToAccountLoginForContext(AuthenticationChallengeContext filterContext)
        {
            var tmp1 = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var tmp2 = Thread.CurrentThread.CurrentCulture.Name.Split('-')[1];
            filterContext.Result = new RedirectToRouteResult("Language",
                new System.Web.Routing.RouteValueDictionary{
                    {"language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName },
                    {"culture", Thread.CurrentThread.CurrentCulture.Name.Split('-')[1] },
                    {"controller", "Account"},
                    {"action", "Login"}
                });
        }

        private static String LastControlActionName = "";

        #endregion
        #region Public
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
            String ActionName = filterContext.ActionDescriptor.ActionName;
            String ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            String NeededPermission = ControllerName + "-" + ActionName;

            var lang = Helper.Helper.getCurrentCulture().Split('-')[0];

            //PRMContext mContext = new PRMContext();
            String tmpName = user.Identity.Name;
            ISession mContext = Context.Open();
            //Replace later this line
            //UserInfo user_permissions = mContext.QueryOver<UserInfo>().Where(x => x.Email == tmpName).SingleOrDefault();
            //There is similar email. so remove List<UserInfo> and just use single row returned not list.
            //UserInfo user_permissions = mContext.QueryOver<UserInfo>().Where(x => x.Email == tmpName).SingleOrDefault();
            List<UserInfo> user_permissions = mContext.QueryOver<UserInfo>().Where(x => x.Email == tmpName).List().ToList();
            Context.Close(mContext);

            if (NeededPermission.Equals("Home-Index"))
            {
                CheckPermissions(user_permissions, NeededPermission, filterContext, ControllerName, ActionName);
            }
            else
            {
                if (HttpContext.Current.Session["CurrentFarmId"] != null)
                {
                    CheckPermissions(user_permissions, NeededPermission, filterContext, ControllerName, ActionName);
                }
                else
                {             
                    SetRedirectToAccountLoginForContext(filterContext);
                }
            }

            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult || identity == null || identity.FindFirst("UserID") == null || identity.FindFirst("FarmID") == null)
            {
                SetRedirectToHomeErrorPageForContext(filterContext, ActionName, ControllerName);
            }

        }
        #endregion
    }
}
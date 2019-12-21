using NHibernate;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.CustomFilters;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class UserPermissionsController : Controller
    {
        // GET: UserPermissions
        [AuthenticateFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthenticateFilter]
        public ActionResult Create()
        {
            return View();
        }

        [AuthenticateFilter]
        public string SavePermissions(string ControllerName, string ActionName, string Comment)
        {
            ControllerName = ControllerName.Replace("Controller", "");
            String retValue = "NO";

            ISession mContext = Context.Open();
            List<ActionControllerListTbl> models = mContext.QueryOver<ActionControllerListTbl>().Where(x => x.Action == ActionName && x.Controller == ControllerName).List().ToList();
            if (models.Count == 0)
            {
                retValue = "OK";
                ActionControllerListTbl newUserPermissions = new ActionControllerListTbl()
                {
                    Action = ActionName,
                    Controller = ControllerName,
                    Comment = Comment,
                    UniqueId = ""
                };
                mContext.Save(newUserPermissions);
            }
            else
            {
                retValue = "این دسترسی قبلا ثبت شده است.";
            }
            Context.Close(mContext);

            return retValue;
        }
    }
}
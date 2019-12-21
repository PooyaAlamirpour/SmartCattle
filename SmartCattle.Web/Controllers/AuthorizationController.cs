using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: Authorization
        public ActionResult unAuthorized()
        { 
            return View();
        }
    }
}
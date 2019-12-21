﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
   public class ElmahResult : ActionResult
    {
        private string _resouceType;

        public ElmahResult(string resouceType)
        {
            _resouceType = resouceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var factory = new Elmah.ErrorLogPageFactory();
            if (!string.IsNullOrEmpty(_resouceType))
            {
                var pathInfo = "." + _resouceType;
                HttpContext.Current.RewritePath(HttpContext.Current.Request.Path, pathInfo, HttpContext.Current.Request.QueryString.ToString());
            }

            var handler = factory.GetHandler(HttpContext.Current, null, null, null);
            
            handler.ProcessRequest(HttpContext.Current);
        }
    }
    public class ElmahController : Controller
    {
      
        // GET: Elmah
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }
        public ActionResult Detail(string type)
        {
            return new ElmahResult(type);
        }
    }
}
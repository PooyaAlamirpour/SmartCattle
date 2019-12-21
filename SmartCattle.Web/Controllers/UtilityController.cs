using SmartCattle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public String getCurrentDate(int AddHours = 0)
        {
            String retValue = "";
            retValue = Utility.ConvertToPersian(DateTime.Now.AddHours(AddHours));
            return retValue;
        }

        [HttpPost]
        public String getCurrentGeoDate(int AddHours = 0)
        {
            DateTime _date = DateTime.Now.AddHours(AddHours);
            StringBuilder sb = new StringBuilder();
            sb.Append(_date.Year.ToString("0000"));
            sb.Append("/");
            sb.Append(_date.Month.ToString("00"));
            sb.Append("/");
            sb.Append(_date.Day.ToString("00"));
            return sb.ToString();
        }
            

    }
}
using Newtonsoft.Json;
using SmartCattle.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartCattle.Web.CustomFilters;

namespace SmartCattle.Web.Controllers
{
    public class PositioningController : Controller
    {
        // GET: Positioning
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthenticateFilter]
        public String getPersonalTrackingWithMap(String MAC)
        {
            String retValueMain = "";
            try
            {
                var requestGetMap = (HttpWebRequest)WebRequest.Create("http://79.175.133.194:2222/getMaps?apiKey=a43f6670-9d37-11e7-ad9d-819a9b28ee42&spId=0&maps=[{\"mapSuperType\":\"zoning\",\"mapSubSuperType\":\"physical\"}]");
                var responseGetMap = (HttpWebResponse)requestGetMap.GetResponse();
                String rawJsonGetMap = new StreamReader(responseGetMap.GetResponseStream()).ReadToEnd();

                var requestGetZone = (HttpWebRequest)WebRequest.Create("http://79.175.133.194:2222/getZone?spId=7&apiKey=a43f6670-9d37-11e7-ad9d-819a9b28ee42&zoneId=0&limit=1000&MAC=" + MAC);
                var responseGetZone = (HttpWebResponse)requestGetZone.GetResponse();
                String rawJsonGetZon = new StreamReader(responseGetZone.GetResponseStream()).ReadToEnd();
                getZoneViewModels ZoneModel = JsonConvert.DeserializeObject<getZoneViewModels>(rawJsonGetZon);

                String ZoneId = ZoneModel.zoneId.ToString();

                retValueMain = rawJsonGetMap.Remove(rawJsonGetMap.Length - 1) + ",";
                retValueMain += rawJsonGetZon;
                retValueMain += "]";
            }
            catch (Exception)
            {

            }

            return retValueMain;
        }
    }
}
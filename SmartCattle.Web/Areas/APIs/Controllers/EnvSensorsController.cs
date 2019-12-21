using Newtonsoft.Json;
using NHibernate;
using SmartCattle.Core;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Areas.APIs.Controllers
{
    public class EnvSensorsController : Controller
    {
        // GET: APIs/EnvSensors
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List()
        {
            ISession mContext = Context.Open();

            String body = new StreamReader(Request.InputStream).ReadToEnd();
            X_Farm MacModel = JsonConvert.DeserializeObject<X_Farm>(body);

            List<EnvSensors> obj = new List<EnvSensors>();
            int tmpFarmId = MacModel.FarmId;
            int tmpFreeStallId = MacModel.FreeStallId;
            obj = mContext.QueryOver<EnvSensors>().Where(x => x.FarmId == tmpFarmId && x.FreeStallId == tmpFreeStallId).List().ToList();
            Context.Close(mContext);

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult THIValue()
        {
            ISession mContext = Context.Open();

            List<X_EnvTHIModel> obj = new List<X_EnvTHIModel>();
            List<List<EnvTHITbl>> sqlModel = new List<List<EnvTHITbl>>();
            List<List<X_EnvSensorsValues>> retValue = new List<List<X_EnvSensorsValues>>();

            String body = new StreamReader(Request.InputStream).ReadToEnd();
            X_MAC MacModel = JsonConvert.DeserializeObject<X_MAC>(body);

            String minValue = MacModel.StartTime;
            String maxValue = MacModel.EndTime;

            

            for (int i = 0; i < MacModel.MACs.Count; i++)
            {
                int tmpFarmID = MacModel.FarmId;
                int tmpFreeStall = MacModel.FreeStallId;
                String tmpMac = MacModel.MACs[i].Address;

                DateTime tmp_minValue = DateTime.ParseExact(minValue, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                DateTime tmp_maxValue = DateTime.ParseExact(maxValue, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                DateTime Newtmp_minValue = new DateTime(tmp_minValue.Year, tmp_minValue.Month, tmp_minValue.Day, tmp_minValue.Hour, tmp_minValue.Minute, tmp_minValue.Second);
                DateTime Newtmp_maxValue = new DateTime(tmp_maxValue.Year, tmp_maxValue.Month, tmp_maxValue.Day, tmp_maxValue.Hour, tmp_maxValue.Minute, tmp_maxValue.Second);

                var tmpEnv = mContext.QueryOver<EnvTHITbl>().Where(x => x.FarmID == tmpFarmID && x.FreeStallId == tmpFreeStall && x.MAC == tmpMac && x.date >= Newtmp_minValue && x.date <= Newtmp_maxValue).List().ToList();

                sqlModel.Add(tmpEnv);
            }

            for (int i = 0; i < sqlModel.Count; i++)
            {
                List<X_EnvSensorsValues> tmpValue = new List<X_EnvSensorsValues>();
                X_EnvSensorsValues items = new X_EnvSensorsValues();
                List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass> Values = new List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass>();
                items.Values = new List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass>();
                int currentN = 0;
                try
                {
                    items.ID = i;
                    items.FarmId = sqlModel[i][0].FarmID;
                    items.FreeStallId = sqlModel[i][0].FreeStallId;
                    items.SensorLat = sqlModel[i][0].SensorLat;
                    items.SensorLng = sqlModel[i][0].SensorLng;
                    items.MacAddress = sqlModel[i][0].MAC;
                    DateTime LastDate = DateTime.ParseExact(MacModel.StartTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));
                    for (int n = 0; n < sqlModel[i].Count; n++)
                    {
                        currentN = n;
                        if (currentN == 171)
                        {
                            String OK = "";
                        }
                        if (currentN == 172)
                        {
                            String OK = "";
                        }
                        DateTime currentTimeDateTime = sqlModel[i][n].date;
                        if ((currentTimeDateTime - LastDate).TotalSeconds > MacModel.MACs[i].Step)
                        {
                            X_EnvSensorsValues.ValuesClass val = new X_EnvSensorsValues.ValuesClass();
                            val.Humidity = sqlModel[i][n].RHValue.ToString().Replace("/", ".");
                            val.Temperature = sqlModel[i][n].TdbValue.ToString().Replace("/", ".");
                            val.THI = Math.Round(Convert.ToDouble(sqlModel[i][n].THIValue), 2).ToString().Replace("/", ".");
                            val.Date = sqlModel[i][n].date.ToString("yyyy/MM/dd HH:mm");
                            LastDate = sqlModel[i][n].date;
                            Values.Add(val);
                        }
                    }
                    items.Values = Values;
                    tmpValue.Add(items);
                    retValue.Add(tmpValue);
                }
                catch (Exception ex)
                {
                    String exMsg = ex.Message;
                    int hello = currentN;
                }

            }

            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(retValue),
                ContentType = "application/json"
            };

            Context.Close(mContext);
            return result;
        }
    }
}

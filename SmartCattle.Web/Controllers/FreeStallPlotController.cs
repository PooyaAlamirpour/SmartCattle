using CS;
using Newtonsoft.Json;
using NHibernate;
using SmartCattle.Core;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class FreeStallPlotController : Controller
    {
        // GET: FreeStallPlot
        public ActionResult Index()
        {
            List<FreeStallTbl> FreestallList = new List<FreeStallTbl>();
            ISession mContext = Context.Open();
            int FarmId = Helper.Helper.getCurrentFarmId();
            FreestallList = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == FarmId).List().ToList();
            if(FreestallList.Count != 0)
            {
                ViewBag.FreeStallId = FreestallList[0].ServerName;
            }
            Context.Close(mContext);
            return View(FreestallList);
        }

        [HttpPost]
        public JsonResult getMap(String MAC, int ServerName)
        {
            FreeStallFeatureMap returnVal = new FreeStallFeatureMap();
            returnVal.CenterMap = new FreeStallFeatureMap.Center();
            try
            {
                int CurrentFarmId = Helper.Helper.getCurrentFarmId();
                ISession mContext = Context.Open();
                FreeStallTbl _FreestallServerName = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == CurrentFarmId).Where(x => x.ServerName == ServerName).SingleOrDefault();
                int CurrentFreestallId = _FreestallServerName.ID;
                List<EnvSensors> _EnvSensors = mContext.QueryOver<EnvSensors>().Where(x => x.FarmId == CurrentFarmId).Where(x => x.FreeStallId == _FreestallServerName.ServerName).List().ToList();
                Context.Close(mContext);

                getPhysicalMap _getPhysicalMap = new getPhysicalMap();
                _getPhysicalMap.apiKey = "a43f6670-9d37-11e7-ad9d-819a9b28ee42";
                _getPhysicalMap.spId = Helper.Helper.getCurrentSubId();
                _getPhysicalMap.mapType = "physical(json)";

                ServicePointManager.Expect100Continue = false;
                WebService.HttpService HttpServiceClientSensorsList = new WebService.HttpService();
                HttpServiceClientSensorsList.ContentType = "application/form-data";
                HttpServiceClientSensorsList.EndPoint = "http://79.175.133.194:2222/getPhysicalMap";
                HttpServiceClientSensorsList.Method = WebService.HttpVerb.POST;
                HttpServiceClientSensorsList.Body = _getPhysicalMap;
                //String rawJsonGetMap = HttpServiceClientSensorsList.MakeRequest();
                String rawJsonGetMap = Helper.Helper.getCurrentMap();

                //String rawJsonGetMap = mContext.QueryOver<FarmMapTbl>().Where(x => x.SubId == Helper.Helper.getCurrentSubId()).SingleOrDefault().Map;

                List<X_getPhysicalMap> PhysicalMap = JsonConvert.DeserializeObject<List<X_getPhysicalMap>>(rawJsonGetMap);

                double x_tmp = 0;
                double y_tmp = 0;
                double counter = 0;

                List<X_getPhysicalMap.Feature> FreeStall = new List<X_getPhysicalMap.Feature>();
                if (PhysicalMap.Count != 0)
                {
                    for (int i = 0; i < PhysicalMap[0].features.Count; i++)
                    {
                        String item = PhysicalMap[0].features[i].geometry.type;
                        if (item.Equals("Polygon"))
                        {
                            FreeStall.Add(PhysicalMap[0].features[i]);
                            //if(_FreestallServerName.ServerName.Equals(PhysicalMap[0].features[i].properties.name))
                            {
                                foreach (var coord in PhysicalMap[0].features[i].geometry.coordinates[0])
                                {
                                    List<double> dCoord = JsonConvert.DeserializeObject<List<double>>(coord.ToString());
                                    x_tmp += dCoord[0];
                                    y_tmp += dCoord[1];
                                    counter++;
                                }
                            }
                        }
                    }
                }

                String PhysicalMapStr = JsonConvert.SerializeObject(FreeStall);
                List<X_FreestallPolygon> FreestallPolygon = JsonConvert.DeserializeObject<List<X_FreestallPolygon>>(PhysicalMapStr);

                X_Farm x_farm = new X_Farm();
                x_farm.FarmId = CurrentFarmId;
                x_farm.FreeStallId = CurrentFreestallId;

                List<X_EnvSensors> EnvSensorsModels = new List<X_EnvSensors>();

                foreach (var item in _EnvSensors)
                {
                    X_EnvSensors SensorItem = new X_EnvSensors()
                    {
                        FreeStallId = item.FreeStallId,
                        FarmId = item.FarmId,
                        Lat = item.Lat,
                        Lng = item.Lng,
                        MAC = item.MAC
                    };

                    EnvSensorsModels.Add(SensorItem);
                }

                returnVal.Map = FreestallPolygon;
                returnVal.MacList = EnvSensorsModels;
                returnVal.ZoneId = Convert.ToInt32(_FreestallServerName.ServerName);// FreeStallId;
                returnVal.CenterMap.Lat = y_tmp / counter;// 35.720208; 
                returnVal.CenterMap.Lng = x_tmp / counter;// 50.871365;
            }
            catch (Exception ex)
            {
                String execption_msg = ex.Message;
                returnVal.Map = new List<X_FreestallPolygon>();
                returnVal.MacList = new List<X_EnvSensors>();
                returnVal.ZoneId = 0;
                returnVal.CenterMap.Lat = 0;// 35.720208; 
                returnVal.CenterMap.Lng = 0;// 50.871365;
            }

            return Json(returnVal);
        }

        public class FreeStallFeatureMap
        {
            public List<X_FreestallPolygon> Map { get; set; }
            public List<X_EnvSensors> MacList { get; set; }
            public int ZoneId { get; set; }
            public Center CenterMap { get; set; }

            public class Center
            {
                public double Lat { get; set; }
                public double Lng { get; set; }
            }
        }

        [HttpPost]
        public ContentResult getHTData(String StartDate, String EndDate, String Step, int FreeStallId)
        {
            List<List<X_EnvSensorsValues>> retValue = new List<List<X_EnvSensorsValues>>();
            List<long> LastId = new List<long>();
            String tmpDate = "";
            try
            {
                List<X_EnvSensors> EnvSensorsModels = new List<X_EnvSensors>();

                StartDate = Utility.toEnglishNumber(StartDate);
                EndDate = Utility.toEnglishNumber(EndDate);
                Step = Utility.toEnglishNumber(Step);

                X_Farm x_farm = new X_Farm();
                x_farm.FarmId = Helper.Helper.getCurrentFarmId();
                x_farm.FreeStallId = FreeStallId;

                ISession mContext = Context.Open();
                List<EnvSensors> getAllEnvSensors = mContext.QueryOver<EnvSensors>().Where(x => x.FarmId == x_farm.FarmId).Where(x => x.FreeStallId == x_farm.FreeStallId).List().ToList();
                Context.Close(mContext);
                if (getAllEnvSensors.Count != 0)
                {
                    EnvSensorsModels = new List<X_EnvSensors>();
                    foreach (var _Sensor in getAllEnvSensors)
                    {
                        X_EnvSensors item = new X_EnvSensors();
                        item.id = _Sensor.id;
                        item.FarmId = _Sensor.FarmId;
                        item.FreeStallId = _Sensor.FreeStallId;
                        item.MAC = _Sensor.MAC;
                        item.Lat = _Sensor.Lat;
                        item.Lng = _Sensor.Lng;

                        EnvSensorsModels.Add(item);
                    }
                }
                else
                {
                    EnvSensorsModels = new List<X_EnvSensors>();
                    X_EnvSensors item = new X_EnvSensors();
                    item.id = 0;
                    item.FarmId = 0;
                    item.FreeStallId = 0;
                    item.MAC = "";
                    item.Lat = 0;
                    item.Lng = 0;

                    EnvSensorsModels.Add(item);
                }

                X_RequestEnvSensrosValue req = new X_RequestEnvSensrosValue();
                List<X_RequestEnvSensrosValue.MAC> MACList = new List<X_RequestEnvSensrosValue.MAC>();
                
                if (EnvSensorsModels.Count != 0)
                {
                    for (int i = 0; i < EnvSensorsModels.Count; i++)
                    {
                        X_RequestEnvSensrosValue.MAC MAC_Item = new X_RequestEnvSensrosValue.MAC();
                        MAC_Item.Address = EnvSensorsModels[i].MAC;
                        MAC_Item.Step = Convert.ToInt32(Step);
                        MACList.Add(MAC_Item);
                    }

                    req.FarmId = Helper.Helper.getCurrentFarmId();
                    req.FreeStallId = FreeStallId;
                    tmpDate = StartDate + " - " + EndDate;
                    if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
                    {
                        req.StartTime = Utility.ConvertFromPersian(StartDate + " 00:01");
                        req.EndTime = Utility.ConvertFromPersian(EndDate + " 23:59");
                    }
                    else
                    {
                        req.StartTime = StartDate.Replace("/", "-") + " 00:01";
                        req.EndTime = EndDate.Replace("/", "-") + " 23:59";
                    }
                    req.MACs = MACList;

                    //SmartCattle.Web.Helper.WebService.HttpService HttpServiceClient = new SmartCattle.Web.Helper.WebService.HttpService();
                    //HttpServiceClient.EndPoint = "http://localhost:44340/APIs/EnvSensors/THIValue";// "http://79.175.133.194/EnvSensors/THIValue";
                    //HttpServiceClient.Method = SmartCattle.Web.Helper.WebService.HttpVerb.POST;
                    //HttpServiceClient.Body = req;
                    List<List<X_EnvSensorsValues>> THIData = THIValue(req);
                    if(THIData.Count != 0)
                    {
                        retValue = THIData;
                        retValue[0][0].Values.Sort((value1, value2) => DateTime.Compare(
                            DateTime.ParseExact(value1.Date, "yyyy/MM/dd HH:mm", null),
                            DateTime.ParseExact(value2.Date, "yyyy/MM/dd HH:mm", null)));
                    }
                    
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                retValue = null;
                retValue = new List<List<X_EnvSensorsValues>>();
                X_EnvSensorsValues item = new X_EnvSensorsValues();
                item.MacAddress = ex.Message + "(" + tmpDate + ")" + "_ Line: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' '));
                
                List <X_EnvSensorsValues> ItemList = new List<X_EnvSensorsValues>();
                ItemList.Add(item);

                retValue.Add(ItemList);

            }
            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(retValue),
                ContentType = "application/json"
            };

            return result;
            //return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public List<List<X_EnvSensorsValues>> THIValue(X_RequestEnvSensrosValue req)
        {
            ISession mContext = Context.Open();

            List<X_EnvTHIModel> obj = new List<X_EnvTHIModel>();
            List<List<EnvTHITbl>> sqlModel = new List<List<EnvTHITbl>>();
            List<List<X_EnvSensorsValues>> retValue = new List<List<X_EnvSensorsValues>>();

            String minValue = req.StartTime;
            String maxValue = req.EndTime;

            for (int i = 0; i < req.MACs.Count; i++)
            {
                int tmpFarmID = req.FarmId;
                int tmpFreeStall = req.FreeStallId;
                String tmpMac = req.MACs[i].Address;

                DateTime tmp_minValue = DateTime.ParseExact(minValue, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                DateTime tmp_maxValue = DateTime.ParseExact(maxValue, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                DateTime Newtmp_minValue = new DateTime(tmp_minValue.Year, tmp_minValue.Month, tmp_minValue.Day, tmp_minValue.Hour, tmp_minValue.Minute, tmp_minValue.Second);
                DateTime Newtmp_maxValue = new DateTime(tmp_maxValue.Year, tmp_maxValue.Month, tmp_maxValue.Day, tmp_maxValue.Hour, tmp_maxValue.Minute, tmp_maxValue.Second);

                var tmpEnv = mContext.QueryOver<EnvTHITbl>().Where(x => x.FarmID == tmpFarmID && x.FreeStallId == tmpFreeStall && x.MAC == tmpMac && x.date >= Newtmp_minValue && x.date <= Newtmp_maxValue).OrderBy(x => x.date).Asc.List().ToList();

                sqlModel.Add(tmpEnv);
            }

            for (int i = 0; i < sqlModel.Count; i++)
            {
                List<X_EnvSensorsValues> tmpValue = new List<X_EnvSensorsValues>();
                X_EnvSensorsValues items = new X_EnvSensorsValues();
                List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass> Values = new List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass>();
                items.Values = new List<SmartCattle.Web.Areas.APIs.Models.X_EnvSensorsValues.ValuesClass>();
                int check_n = 0, check_i = 0;
                try
                {
                    items.ID = i;
                    if(sqlModel[i].Count != 0)
                    {
                        items.FarmId = sqlModel[i][0].FarmID;
                        items.FreeStallId = sqlModel[i][0].FreeStallId;
                        items.SensorLat = sqlModel[i][0].SensorLat;
                        items.SensorLng = sqlModel[i][0].SensorLng;
                        items.MacAddress = sqlModel[i][0].MAC;
                        DateTime LastDate = DateTime.ParseExact(req.StartTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));
                        for (int n = 0; n < sqlModel[i].Count; n++)
                        {
                            DateTime currentTimeDateTime = sqlModel[i][n].date;
                            //if ((currentTimeDateTime - LastDate).TotalSeconds > req.MACs[i].Step)
                            {
                                X_EnvSensorsValues.ValuesClass val = new X_EnvSensorsValues.ValuesClass();
                                val.Humidity = sqlModel[i][n].RHValue.ToString().Replace("/", ".");
                                val.Temperature = sqlModel[i][n].TdbValue.ToString().Replace("/", ".");
                                val.THI = Math.Round(Convert.ToDouble(sqlModel[i][n].THIValue), 2).ToString().Replace("/", ".");
                                val.Date = sqlModel[i][n].date.ToString("yyyy/MM/dd HH:mm");
                                LastDate = sqlModel[i][n].date;
                                Values.Add(val);
                            }
                            check_n = n;
                            check_i = i;
                        }
                        items.Values = Values;
                        tmpValue.Add(items);
                        retValue.Add(tmpValue);
                    }
                }
                catch (Exception ex)
                {
                    String exMsg = ex.Message;
                    int tmp_check_n = check_n;
                    int tmp_check_i = check_i;
                }
            }

            Context.Close(mContext);
            return retValue;
        }

        [HttpPost]
        public ContentResult getTopTenFreeStall()
        {
            List<TopTenFreeStall> retValue = new List<TopTenFreeStall>();
            Random rand = new Random();
            TopTenFreeStall item = new TopTenFreeStall();

            ISession mContext = Context.Open();
            List<EquipmentTbl> _Equipment = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == Helper.Helper.getCurrentSubId()).Where(x => x.DeviceType == "EnvironmentSensor").List().ToList();
            for (int i = 0; i < _Equipment.Count; i++)
            {
                EnvTHITbl _EnvTHITbl = mContext.QueryOver<EnvTHITbl>().Where(x => x.MAC == _Equipment[i].Mac).OrderBy(x => x.date).Desc.Take(1).SingleOrDefault();
                FreeStallTbl _FreeStallTbl = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Where(x => x.ServerName == _EnvTHITbl.FreeStallId).SingleOrDefault();
                item = new TopTenFreeStall();
                item.Name = _FreeStallTbl == null ? "No Name" : _FreeStallTbl.name;
                item.Humidity = _EnvTHITbl.RHValue.ToString().Replace("/", ".");
                item.Tempreture = _EnvTHITbl.TdbValue.ToString().Replace("/", ".");
                retValue.Add(item);
            }
            
            Context.Close(mContext);

            item = new TopTenFreeStall();
            item.Name = "FreeStall R3";
            item.Humidity = rand.Next(0, 50).ToString();
            item.Tempreture = rand.Next(0, 30).ToString();
            retValue.Add(item);

            item = new TopTenFreeStall();
            item.Name = "FreeStall R4";
            item.Humidity = rand.Next(0, 50).ToString();
            item.Tempreture = rand.Next(0, 30).ToString();
            retValue.Add(item);

            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(retValue),
                ContentType = "application/json"
            };

            return result;
        }

        public class TopTenFreeStall
        {
            public String Name { get; set; }
            public String Humidity { get; set; }
            public String Tempreture { get; set; }
        }

        private String int2str(int integer)
        {
            String retValue = "";
            if (integer < 10)
            {
                retValue = "0" + integer.ToString();
            }
            else
            {
                retValue = integer.ToString();
            }
            return retValue;
        }
    }
}
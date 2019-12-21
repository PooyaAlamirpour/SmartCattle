using Newtonsoft.Json;
using NHibernate;
using ServiceStack;
using SmartCattle.Core;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models;
using SmartCattle.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class FreeStallPositionController : Controller
    {
        // GET: FreeStallPosition
        public ActionResult Index(String input)
        {
            if(input != null)
            {
                Encryption _crypt = new Encryption();
                String iv = WebConfigurationManager.AppSettings["ENCRYPTION_IV"];
                String key = WebConfigurationManager.AppSettings["ENCRYPTION_KEY"];
                String decrypted_input = _crypt.decrypt(input, key, iv).Split('_')[0];
                ViewBag.FreeStallId = decrypted_input;
            }
            return View();
        }

        [HttpPost]
        public JsonResult getMap(String MAC, int FreeStallId)
        {
            FreeStallFeatureMap returnVal = new FreeStallFeatureMap();
            returnVal.CenterMap = new FreeStallFeatureMap.Center();
            try
            {
                ISession mContext = Context.Open();
                FreeStallTbl _FreestallServerName = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == FreeStallId).SingleOrDefault();
                Context.Close(mContext);

                getPhysicalMap _getPhysicalMap = new getPhysicalMap();
                _getPhysicalMap.apiKey = "a43f6670-9d37-11e7-ad9d-819a9b28ee42";
                _getPhysicalMap.spId = 0;
                _getPhysicalMap.mapType = "physical(json)";

                ServicePointManager.Expect100Continue = false;
                WebService.HttpService HttpServiceClientSensorsList = new WebService.HttpService();
                HttpServiceClientSensorsList.ContentType = "application/form-data";
                HttpServiceClientSensorsList.EndPoint = "http://79.175.133.194:2222/getPhysicalMap";
                HttpServiceClientSensorsList.Method = WebService.HttpVerb.POST;
                HttpServiceClientSensorsList.Body = _getPhysicalMap;
                String rawJsonGetMap = HttpServiceClientSensorsList.MakeRequest();
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
                x_farm.FarmId = Helper.Helper.getCurrentFarmId();
                x_farm.FreeStallId = 5;

                //Get all mac address for current freestall
                //HttpServiceClientSensorsList = new SmartCattle.Web.Helper.WebService.HttpService();
                //HttpServiceClientSensorsList.EndPoint = @"http://79.175.133.194/APIs/EnvSensors/List";
                //HttpServiceClientSensorsList.Method = SmartCattle.Web.Helper.WebService.HttpVerb.POST;
                //HttpServiceClientSensorsList.Body = x_farm;
                //String jsonMacList = HttpServiceClientSensorsList.MakeRequest();
                //List<X_EnvSensors> EnvSensorsModels = JsonConvert.DeserializeObject<List<X_EnvSensors>>(jsonMacList);

                List<X_EnvSensors> EnvSensorsModels = new List<X_EnvSensors>();
                X_EnvSensors SensorItem = new X_EnvSensors()
                {
                    FreeStallId = 5,
                    FarmId = 3,
                    Lat = 35.720374,
                    Lng = 50.87155,
                    MAC = "e3:ad:eb:31:e0:ef"
                };
                EnvSensorsModels.Add(SensorItem);

                SensorItem = new X_EnvSensors()
                {
                    FreeStallId = 5,
                    FarmId = 3,
                    Lat = 35.720053,
                    Lng = 50.871568,
                    MAC = "d1:13:a3:c5:d1:be"
                };
                EnvSensorsModels.Add(SensorItem);

                returnVal.Map = FreestallPolygon;
                returnVal.MacList = EnvSensorsModels;
                returnVal.ZoneId = _FreestallServerName.ServerName;
                returnVal.CenterMap.Lat = y_tmp / counter;// 35.720208; 
                returnVal.CenterMap.Lng = x_tmp / counter;// 50.871365;
            }
            catch (Exception ex)
            {
                String execption_msg = ex.Message;
            }

            return Json(returnVal);
        }

        [HttpPost]
        public JsonResult getMapWithCattlePosition(String MAC, int FreeStallId)
        {
            FreeStallFeatureMap returnVal = new FreeStallFeatureMap();
            returnVal.CenterMap = new FreeStallFeatureMap.Center();
            returnVal.CattlePosition = new FreeStallFeatureMap.Position();
            try
            {
                ISession mContext = Context.Open();
                FreeStallTbl _FreestallServerName = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == FreeStallId).SingleOrDefault();
                List<SensorTbl> _cattle = mContext.QueryOver<SensorTbl>().Where(x => x.MacAddress == MAC).List().ToList();
                double CattleLat = 0;
                double CattleLng = 0;
                if (_cattle.Count != 0)
                {
                    CattlePositionTbl _CattlePosition = mContext.QueryOver<CattlePositionTbl>().Where(x => x.cattleId == _cattle[0].cattleId).OrderBy(x => x.date).Desc.Take(1).SingleOrDefault();
                    if(_CattlePosition != null)
                    {
                        CattleLat = _CattlePosition.Latitude;
                        CattleLng = _CattlePosition.Longitude;
                    }
                }
                Context.Close(mContext);

                getPhysicalMap _getPhysicalMap = new getPhysicalMap();
                _getPhysicalMap.apiKey = "a43f6670-9d37-11e7-ad9d-819a9b28ee42";
                _getPhysicalMap.spId = 0;
                _getPhysicalMap.mapType = "physical(json)";

                ServicePointManager.Expect100Continue = false;
                WebService.HttpService HttpServiceClientSensorsList = new WebService.HttpService();
                HttpServiceClientSensorsList.ContentType = "application/form-data";
                HttpServiceClientSensorsList.EndPoint = "http://79.175.133.194:2222/getPhysicalMap";
                HttpServiceClientSensorsList.Method = WebService.HttpVerb.POST;
                HttpServiceClientSensorsList.Body = _getPhysicalMap;
                //String rawJsonGetMap = HttpServiceClientSensorsList.MakeRequest();
                String rawJsonGetMap = Helper.Helper.getCurrentMap();
                List<X_getPhysicalMap> PhysicalMap = new List<X_getPhysicalMap>();
                if (rawJsonGetMap != "")
                {
                    PhysicalMap = JsonConvert.DeserializeObject<List<X_getPhysicalMap>>(rawJsonGetMap);
                }

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
                x_farm.FarmId = Helper.Helper.getCurrentFarmId();
                x_farm.FreeStallId = 5;

                //Get all mac address for current freestall
                //HttpServiceClientSensorsList = new SmartCattle.Web.Helper.WebService.HttpService();
                //HttpServiceClientSensorsList.EndPoint = @"http://79.175.133.194/APIs/EnvSensors/List";
                //HttpServiceClientSensorsList.Method = SmartCattle.Web.Helper.WebService.HttpVerb.POST;
                //HttpServiceClientSensorsList.Body = x_farm;
                //String jsonMacList = HttpServiceClientSensorsList.MakeRequest();
                //List<X_EnvSensors> EnvSensorsModels = JsonConvert.DeserializeObject<List<X_EnvSensors>>(jsonMacList);

                List<X_EnvSensors> EnvSensorsModels = new List<X_EnvSensors>();
                X_EnvSensors SensorItem = new X_EnvSensors()
                {
                    FreeStallId = 5,
                    FarmId = 3,
                    Lat = 35.720681,
                    Lng = 50.871101,
                    MAC = "e3:ad:eb:31:e0:ef"
                };
                EnvSensorsModels.Add(SensorItem);

                SensorItem = new X_EnvSensors()
                {
                    FreeStallId = 5,
                    FarmId = 3,
                    Lat = 35.720461,
                    Lng = 50.871161,
                    MAC = "d1:13:a3:c5:d1:be"
                };
                EnvSensorsModels.Add(SensorItem);

                returnVal.Map = FreestallPolygon;
                returnVal.MacList = EnvSensorsModels;
                returnVal.ZoneId = -1;
                returnVal.CenterMap.Lat = y_tmp / counter;// 35.720208; 
                returnVal.CenterMap.Lng = x_tmp / counter;// 50.871365;
                returnVal.CattlePosition.Lat = CattleLat;
                returnVal.CattlePosition.Lng = CattleLng;
            }
            catch (Exception ex)
            {
                String execption_msg = ex.Message;
            }

            return Json(returnVal);
        }

        public class FreeStallFeatureMap
        {
            public List<X_FreestallPolygon> Map { get; set; }
            public List<X_EnvSensors> MacList { get; set; }
            public int ZoneId { get; set; }
            public Center CenterMap { get; set; }
            public Position CattlePosition { get; set; }

            public class Center
            {
                public double Lat { get; set; }
                public double Lng { get; set; }
            }

            public class Position
            {
                public double Lat { get; set; }
                public double Lng { get; set; }
            }
        }

        [HttpPost]
        public ContentResult getHTData(String StartDate, String EndDate, String Step, String MAC)
        {
            List<List<X_EnvSensorsValues>> retValue = new List<List<X_EnvSensorsValues>>();
            List<long> LastId = new List<long>();
            try
            {
                List<X_EnvSensors> EnvSensorsModels = new List<X_EnvSensors>();

                StartDate = Utility.toEnglishNumber(StartDate);
                EndDate = Utility.toEnglishNumber(EndDate);
                Step = Utility.toEnglishNumber(Step);

                X_Farm x_farm = new X_Farm();
                x_farm.FarmId = 3;
                x_farm.FreeStallId = 5;

                if(MAC == "")
                {
                    //Get all mac address for current freestall
                    SmartCattle.Web.Helper.WebService.HttpService HttpServiceClientSensorsList = new SmartCattle.Web.Helper.WebService.HttpService();
                    HttpServiceClientSensorsList.EndPoint = "http://localhost:44340/APIs/EnvSensors/List";//*/ "http://79.175.133.194/EnvSensors/List";
                    HttpServiceClientSensorsList.Method = SmartCattle.Web.Helper.WebService.HttpVerb.POST;
                    HttpServiceClientSensorsList.Body = x_farm;
                    String jsonMacList = HttpServiceClientSensorsList.MakeRequest();
                    EnvSensorsModels = JsonConvert.DeserializeObject<List<X_EnvSensors>>(jsonMacList);
                }
                else
                {
                    EnvSensorsModels = new List<X_EnvSensors>();
                    X_EnvSensors item = new X_EnvSensors();
                    item.id = 0;
                    item.FarmId = 3;
                    item.FreeStallId = 5;
                    item.MAC = MAC;
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

                    req.FarmId = 3;
                    req.FreeStallId = 5;
                    if(Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
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

                    SmartCattle.Web.Helper.WebService.HttpService HttpServiceClient = new SmartCattle.Web.Helper.WebService.HttpService();
                    HttpServiceClient.EndPoint = /*"http://localhost:44340/APIs/EnvSensors/THIValue";//*/ "http://79.175.133.194/EnvSensors/THIValue";
                    HttpServiceClient.Method = SmartCattle.Web.Helper.WebService.HttpVerb.POST;
                    HttpServiceClient.Body = req;
                    String jsonSensorValues = HttpServiceClient.MakeRequest();
                    if(jsonSensorValues == "[]")
                    {
                        retValue = new List<List<X_EnvSensorsValues>>();
                    }
                    else
                    {
                        retValue = JsonConvert.DeserializeObject<List<List<X_EnvSensorsValues>>>(jsonSensorValues);
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
            }
            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(retValue),
                ContentType = "application/json"
            };

            return result;
            //return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        private String int2str(int integer)
        {
            String retValue = "";
            if(integer < 10)
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
using Newtonsoft.Json;
using NHibernate;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models;
using SmartCattle.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class FreeStallController : BaseController
    {

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> List()
        {
            List<FreeStallTbl> FreeStallList = new List<FreeStallTbl>();
            ISession mContext = Context.Open();

            try
            {
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
                
                rawJsonGetMap = rawJsonGetMap.Replace("stroke-opacity", "stroke_opacity");
                rawJsonGetMap = rawJsonGetMap.Replace("stroke-width", "stroke_width");
                rawJsonGetMap = rawJsonGetMap.Replace("fill-opacity", "fill_opacity");

                List<X_getPhysicalMap> SubProjectModel = JsonConvert.DeserializeObject<List<X_getPhysicalMap>>(rawJsonGetMap);
                List<X_getPhysicalMap.Feature> ServerFreeStall = new List<X_getPhysicalMap.Feature>();
                if (SubProjectModel.Count != 0)
                {
                    for (int i = 0; i < SubProjectModel[0].features.Count; i++)
                    {
                        String item = SubProjectModel[0].features[i].geometry.type;
                        if (item.Equals("Polygon"))
                        {
                            ServerFreeStall.Add(SubProjectModel[0].features[i]);
                        }
                    }
                }
                int CurrentFarmID = Helper.Helper.getCurrentFarmId();
                List<FreeStallTbl> DatabaseFreestall = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == CurrentFarmID).List().ToList();

                List<int> strServerName = new List<int>();
                List<int> strDatabaseName = new List<int>();

                foreach (var item in ServerFreeStall)
                {
                    if(item.properties.name != null)
                    {
                        strServerName.Add(Convert.ToInt16(item.properties.name.Replace("kml_", "").Replace("KML_", "").Replace("kml-", "")));
                    }
                }

                foreach (var item in DatabaseFreestall)
                {
                    strDatabaseName.Add(item.ServerName);
                }

                List<int> lstInServerNotInDatabase = strServerName.Except(strDatabaseName).ToList();
                List<int> lstInDatabaseNotInServer = strDatabaseName.Except(strServerName).ToList();

                if (lstInDatabaseNotInServer.Count != 0)
                {
                    foreach (var item in lstInDatabaseNotInServer)
                    {
                        String RemoveStaffRole = string.Format("DELETE FROM {0} where ServerName = '{1}'", "SmartCattle.FreeStallTbl", item);
                        mContext.CreateSQLQuery(RemoveStaffRole).ExecuteUpdate();
                    }
                }

                if (lstInServerNotInDatabase.Count != 0)
                {
                    if (Helper.Helper.getCurrentFarmId() != -5 && Helper.Helper.getCurrentFarmId() != -1)
                    {
                        foreach (var item in lstInServerNotInDatabase)
                        {
                            FreeStallTbl _FreeStallTbl = new FreeStallTbl()
                            {
                                name = "Untitled",
                                Description = "",
                                ServerName = item,
                                FarmID = CurrentFarmID,
                                GroupID = 0,
                                UserId = "NaN"
                            };
                            mContext.Save(_FreeStallTbl);
                        }
                    }
                }

                String Ack = "";
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
            }

            int getCurrentFarmId = Helper.Helper.getCurrentFarmId();
            FreeStallList = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == getCurrentFarmId).OrderBy(x => x.ServerName).Asc.List().ToList();
            Context.Close(mContext);

            return View(FreeStallList);
        }

        [HttpPost]
        public String EditFreeStallName(int FreeStallId, String Name, String Desc)
        {
            String retValue = "NaN";
            try
            {
                ISession mContext = Context.Open();
                FreeStallTbl _FreeStall = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == FreeStallId).SingleOrDefault();
                _FreeStall.name = Name;
                _FreeStall.Description = Desc;
                mContext.Update(_FreeStall);
                mContext.Flush();
                Context.Close(mContext);

                retValue = "OK";
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
            }
            return retValue;
        }

        [HttpPost]
        public String getEncryptedValue(String value)
        {
            Random _rand = new Random();
            String rand_value = _rand.NextDouble().ToString().Substring(2);
            Encryption _cryp = new Encryption();
            String iv = WebConfigurationManager.AppSettings["ENCRYPTION_IV"];
            String key = WebConfigurationManager.AppSettings["ENCRYPTION_KEY"];
            String retValue = _cryp.encrypt(value + "_" + rand_value, key, iv);
            return retValue;
        }
    }
}

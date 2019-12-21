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
    public class CattleSensorsController : Controller
    {
        // GET: APIs/CattleSensors
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Temperature()
        {
            String body = new StreamReader(Request.InputStream).ReadToEnd();
            X_CattleData CattleModel = JsonConvert.DeserializeObject<X_CattleData>(body);
            List<TempretureTbl> retValueModel = new List<TempretureTbl>();

            List<TempretureTbl> obj = new List<TempretureTbl>();

            String minValue = CattleModel.StartTime.Split(' ')[0] + " 00:00";//2017-12-24 18:28
            String maxValue = CattleModel.EndTime.Split(' ')[0] + " 23:59";//2017-12-24 18:28

            DateTime tmp_minValue = DateTime.ParseExact(minValue, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));
            DateTime tmp_maxValue = DateTime.ParseExact(maxValue, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));

            DateTime Newtmp_minValue = new DateTime(tmp_minValue.Year, tmp_minValue.Month, tmp_minValue.Day, tmp_minValue.Hour, tmp_minValue.Minute, tmp_minValue.Second);
            DateTime Newtmp_maxValue = new DateTime(tmp_maxValue.Year, tmp_maxValue.Month, tmp_maxValue.Day, tmp_maxValue.Hour, tmp_maxValue.Minute, tmp_maxValue.Second);

            //PRMContext mContext = new PRMContext();
            int tmpFarmId = CattleModel.FarmId;
            int tmpCattleId = CattleModel.CattleId;

            ISession mContext = Context.Open();
            IList<TempretureTbl> sqlModel = mContext.QueryOver<TempretureTbl>()
                .Where(x => x.FarmID == tmpFarmId)
                .Where(x => x.cattleId == tmpCattleId)
                .Where(x => x.date >= Newtmp_minValue)
                .Where(x => x.date <= Newtmp_maxValue).OrderBy(x => x.date).Asc.List<TempretureTbl>();
            Context.Close(mContext);

            if(sqlModel.Count > 0)
            {
                DateTime LastSaveddate = sqlModel[0].date;
                for (int i = 0; i < sqlModel.Count; i++)
                {
                    DateTime tmpDate = sqlModel[i].date;
                    TimeSpan duration = tmpDate - LastSaveddate;
                    if (duration.TotalMinutes >= CattleModel.Step)
                    {
                        TempretureTbl tmpItem = new TempretureTbl();

                        tmpItem.cattleId = sqlModel[i].cattleId;
                        tmpItem.date = sqlModel[i].date;
                        tmpItem.dateStr = Num2Str(sqlModel[i].date.Year) + "/" + Num2Str(sqlModel[i].date.Month) + "/" + Num2Str(sqlModel[i].date.Day) + " " +
                            Num2Str(sqlModel[i].date.Hour) + ":" + Num2Str(sqlModel[i].date.Minute);
                        tmpItem.FarmID = sqlModel[i].FarmID;
                        tmpItem.ID = sqlModel[i].ID;
                        tmpItem.value = sqlModel[i].value;

                        retValueModel.Add(tmpItem);

                        LastSaveddate = tmpDate;
                    }

                }
            }
            
            JsonResult retValues = Json(retValueModel, JsonRequestBehavior.AllowGet);
            retValues.MaxJsonLength = 8675309;
            return retValues;
        }

        private String Num2Str(int input)
        {
            String retValue = "0";
            if(input <= 9)
            {
                retValue += input.ToString();
            }
            else
            {
                retValue = input.ToString();
            }
            return retValue;
        }

        [HttpPost]
        public JsonResult Activity()
        {
            String body = new StreamReader(Request.InputStream).ReadToEnd();
            X_CattleData CattleModel = JsonConvert.DeserializeObject<X_CattleData>(body);
            List<ActivityStateTbl> Values = new List<ActivityStateTbl>();

            List<ActivityStateTbl> obj = new List<ActivityStateTbl>();

            String minValue = CattleModel.StartTime.Split(' ')[0] + " 00:00";//2017-12-24 18:28
            String maxValue = CattleModel.EndTime.Split(' ')[0] + " 23:59";//2017-12-24 18:28

            DateTime minValueDateTime = DateTime.ParseExact(minValue, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime maxValueDateTime = DateTime.ParseExact(maxValue, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            ISession mContext = Context.Open();
            int tmpFarmId = CattleModel.FarmId;
            int tmpCattleId = CattleModel.CattleId;
            Values = mContext.QueryOver<ActivityStateTbl>().Where(x => x.FarmID == tmpFarmId && x.cattleId == tmpCattleId && x.date >= minValueDateTime && x.date <= maxValueDateTime).List().ToList();
            Context.Close(mContext);

            X_CattleSensorValues items = new X_CattleSensorValues();
            List<X_CattleSensorValues.Activity> activityList = new List<X_CattleSensorValues.Activity>();
            X_CattleSensorValues.Activity activityItem = new X_CattleSensorValues.Activity();
            if (Values.Count != 0)
            {
                items.ID = 1;
                items.FarmId = CattleModel.FarmId;
                items.CattleId = CattleModel.CattleId;
                items.MacAddress = "";
                items.CattleTemperature = new List<X_CattleSensorValues.Temperature>();
                decimal totalStanding = 0;
                decimal totalWalking = 0;
                decimal totalEating = 0;
                decimal totalDrinking = 0;
                decimal totalSitting = 0;
                decimal totalRuminating = 0;
                double AllTotal = 0;
                //////////////////////////////////////////
                List<DateTime> dateTimeList = new List<DateTime>();
                for (int i = 0; i < Values.Count; i++)
                {
                    dateTimeList.Add(Values[i].date);
                }
                double totalStep = (double)(maxValueDateTime - minValueDateTime).TotalHours / (double)CattleModel.Step;
                for (int i = 0; i < totalStep; i++)
                {
                    DateTime nextValueDateTime = minValueDateTime.AddHours(CattleModel.Step);
                    List<DateTime> tmp = dateTimeList.FindAll(item => item >= minValueDateTime && item <= nextValueDateTime);
                    for (int n = 0; n < tmp.Count; n++)
                    {
                        int index = dateTimeList.IndexOf(tmp[n]);
                        totalStanding += Values[index].Standing;
                        totalWalking += Values[index].Walking;
                        totalEating += Values[index].Eating;
                        totalDrinking += Values[index].Drinking;
                        totalSitting += Values[index].Sitting;
                        totalRuminating += Values[index].Rumination;
                    }
                    AllTotal = (double)totalStanding + (double)totalWalking + (double)totalEating + (double)totalDrinking + (double)totalSitting + (double)totalRuminating;
                    if (AllTotal != 0)
                    {
                        activityItem = new X_CattleSensorValues.Activity();
                        activityItem.standing =  (Math.Round(Convert.ToDouble(100 * ((double)totalStanding / (double)AllTotal)), 2));
                        activityItem.walking = (Math.Round(Convert.ToDouble(100 * ((double)totalWalking / (double)AllTotal)), 2));
                        activityItem.eating = (Math.Round(Convert.ToDouble(100 * ((double)totalEating / (double)AllTotal)), 2));
                        activityItem.drinking = (Math.Round(Convert.ToDouble(100 * ((double)totalDrinking / (double)AllTotal)), 2));
                        activityItem.sitting = (Math.Round(Convert.ToDouble(100 * ((double)totalSitting / (double)AllTotal)), 2));
                        activityItem.ruminating = (Math.Round(Convert.ToDouble(100 * ((double)totalRuminating / (double)AllTotal)), 2));
                        activityItem.Date = Utility.ConvertToPersian(minValueDateTime) + " " + minValueDateTime.ToString("HH:mm");

                        activityList.Add(activityItem);

                        AllTotal = 0;
                        totalStanding = 0;
                        totalWalking = 0;
                        totalEating = 0;
                        totalDrinking = 0;
                        totalSitting = 0;
                        totalRuminating = 0;
                    }
                    else
                    {
                        activityItem = new X_CattleSensorValues.Activity();
                        activityItem.standing = 0;
                        activityItem.walking = 0;
                        activityItem.eating = 0;
                        activityItem.drinking = 0;
                        activityItem.sitting = 0;
                        activityItem.ruminating = 0;
                        activityItem.Date = Utility.ConvertToPersian(minValueDateTime) + " " + minValueDateTime.ToString("HH:mm");

                        activityList.Add(activityItem);
                    }

                    minValueDateTime = nextValueDateTime;
                }
                
                items.CattleActivity = activityList;
            }
            else
            {

            }
            JsonResult retValues = Json(items, JsonRequestBehavior.AllowGet);
            retValues.MaxJsonLength = 8675309;
            return retValues;
        }
    }
}
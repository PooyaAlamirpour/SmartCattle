using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DataAccess.Dapper;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Text;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.CustomFilters;
using System.Reflection;
using System.Globalization;
using SmartCattle.Web.Domain;
using NHibernate;
using SmartCattle.Web.Models;
using NHibernate.Criterion;
using System.Threading;

namespace SmartCattle.Web.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class CattleController : BaseController
    {
        #region Variables
        public const byte ItemPerPage = 15;
        BaseServices<Cattle> cattleService;
        BaseServices<CattleEvent> EventService;
        BaseServices<CattleScore> ScoreService;

        #endregion
        public CattleController(BaseServices<Cattle> cattleService, BaseServices<CattleGroup> groupService,
            BaseServices<CattleScore> ScoreService, BaseServices<CattleEvent> EventService)
        {
            this.cattleService = cattleService;
            //this.groupService = groupService;
            this.ScoreService = ScoreService;
            this.EventService = EventService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthenticateFilter]
        public async Task<ActionResult> Insert()
        {
            ISession mContext = Context.Open();
            List<CattleGroupTbl> groups = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);
            return View(groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public String setCattle(int animalNumber, String Name, String Sex, int MotherID, String Genetics_type_num, String birthDate, String lastCalvingDate, int lactationNumber)
        {
            String ret = "";
            ret = SaveCattle(animalNumber, Name, Sex, Genetics_type_num, birthDate, MotherID, lastCalvingDate, lactationNumber);
            return ret;
        }

        private String SaveCattle(int animalNumber, String Name, String Sex, String Genetics_type_num, String birthDate, int MotherID, String lastCalvingDate, int lactationNumber)
        {
            String ret = "";
            if(birthDate == "")
            {
                birthDate = "1300/01/01";
            }
            if(lastCalvingDate == "")
            {
                lastCalvingDate = "1300/01/01";
            }
            
            if (animalNumber != 0 && Helper.Helper.getCurrentFarmId() != -1)
            {
                try
                {
                    ret = SaveOneCattle(animalNumber, Name, Sex, Genetics_type_num, birthDate, MotherID, lastCalvingDate, lactationNumber);
                }
                catch (Exception ex)
                {
                    ret = "EXCEPTION: " + ex.Message;
                }
            }
            else
            {
                ret = "EMPTY";
            }
            return ret;
        }

        private String SaveOneCattle(int animalNumber, String Name, String Sex, String Genetics_type_num, String birthDate, 
            int MotherID, String lastCalvingDate, int lactationNumber)
        {
            String ret = "";
            farmID = Helper.Helper.getCurrentFarmId();
            birthDate = birthDate.Replace("۰", "0");
            birthDate = birthDate.Replace("۱", "1");
            birthDate = birthDate.Replace("۲", "2");
            birthDate = birthDate.Replace("۳", "3");
            birthDate = birthDate.Replace("۴", "4");
            birthDate = birthDate.Replace("۵", "5");
            birthDate = birthDate.Replace("۶", "6");
            birthDate = birthDate.Replace("۷", "7");
            birthDate = birthDate.Replace("۸", "8");
            birthDate = birthDate.Replace("۹", "9");

            lastCalvingDate = lastCalvingDate.Replace("۰", "0");
            lastCalvingDate = lastCalvingDate.Replace("۱", "1");
            lastCalvingDate = lastCalvingDate.Replace("۲", "2");
            lastCalvingDate = lastCalvingDate.Replace("۳", "3");
            lastCalvingDate = lastCalvingDate.Replace("۴", "4");
            lastCalvingDate = lastCalvingDate.Replace("۵", "5");
            lastCalvingDate = lastCalvingDate.Replace("۶", "6");
            lastCalvingDate = lastCalvingDate.Replace("۷", "7");
            lastCalvingDate = lastCalvingDate.Replace("۸", "8");
            lastCalvingDate = lastCalvingDate.Replace("۹", "9");


            DateTime CattleBirthday = DateTime.Now;// DateHelper.ConvertToGeorginDate(birthDate);
            DateTime CattleLastCalvingDate = DateTime.Now;// DateHelper.ConvertToGeorginDate(lastCalvingDate);

            CattleTbl cattle = new CattleTbl();
            cattle.age = DateHelper.calculateAge(CattleBirthday);
            cattle.animalNumber = animalNumber;
            cattle.Name = Name;
            cattle.Sex = (Sex == "Sex")? "Female" : "mail";
            cattle.Genetics_type_num = Genetics_type_num;
            cattle.birthDate = CattleBirthday;
            cattle.MotherID = MotherID;
            cattle.lastCalvingDate = CattleLastCalvingDate;
            cattle.lactationNumber = lactationNumber;
            cattle.FarmID = farmID;

            cattle.milkAvgDate = DateTime.Now;
            cattle.healthStatusDate = DateTime.Now;
            cattle.heatStatusDate = DateTime.Now;
            cattle.fertilityStatusDate = DateTime.Now;
            cattle.lastInseminationDate = DateTime.Now;
            cattle.Body_Condition_ScoreDate = DateTime.Now;
            cattle.CleanlinessDate = DateTime.Now;
            cattle.HockDate = DateTime.Now;
            cattle.MobilityDate = DateTime.Now;
            cattle.ManureDate = DateTime.Now;
            cattle.RumenDate = DateTime.Now;
            cattle.TeatDate = DateTime.Now;
            cattle.CreatedDate = DateTime.Now;

            ISession mContext = Context.Open();
            List<CattleTbl> itemes = mContext.QueryOver<CattleTbl>().Where(x => x.animalNumber == animalNumber).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            if (itemes.Count == 0)
            {
                mContext.Clear();
                mContext.Save(cattle);
                ret = "SAVED";
            }
            else
            {
                ret = "SIMILAR";
            }
            Context.Close(mContext);
            return ret;
        }

        [HttpPost]
        public String CattleUploading(HttpPostedFileBase file, String txtExcelType)
        {
            String FileName = "NaN";
            if (file.ContentLength > 0)
            {
                String rndName = new Random().NextDouble().ToString().Replace("0.", "").Replace("0/", "");
                var fileName = Path.GetFileName(file.FileName).Replace(".xlsx", rndName + ".xlsx");
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
                String[] SplitedFileName = path.Split('\\');
                FileName = SplitedFileName[SplitedFileName.Length - 1];
            }

            return FileName;
        }

        [HttpPost]
        public String CattleSaveAsList(String eExcelType, String FileName)
        {
            String retValue = "";
            String path = Path.Combine(Server.MapPath("~/App_Data/uploads"), FileName);
            List<List<String>> ParsedExcell = Helper.Helper.ParseCattleExcel(path, eExcelType);
            int CurrentFarmId = Helper.Helper.getCurrentFarmId();
            foreach (var item in ParsedExcell)
            {
                retValue = SaveOneCattle(
                    Convert.ToInt32(item[(int)ExcelType.SmartCattle.animalNumber]),
                    item[(int)ExcelType.SmartCattle.Name],
                    item[(int)ExcelType.SmartCattle.Sex],
                    item[(int)ExcelType.SmartCattle.Genetics_type_num],
                    DateTime.Now.ToString(),//item[(int)ExcelType.SmartCattle.birthDate]
                    Convert.ToInt32(item[(int)ExcelType.SmartCattle.MotherID]),
                    DateTime.Now.ToString(),//item[(int)ExcelType.SmartCattle.lastCalvingDate]
                    Convert.ToInt32(item[(int)ExcelType.SmartCattle.lactationNumber]));
            }
            return retValue;
        }

        public async Task<PartialViewResult> CattleScore(int CattleID)
        {
            ISession mContext = Context.Open();
            List<CattleScoreTbl> CattleScoreList = mContext.QueryOver<CattleScoreTbl>().Where(x => x.CattleId == CattleID).OrderBy(x => x.Date).Desc.List().ToList();
            Context.Close(mContext);

            ViewBag.CattleId = CattleID;
            return PartialView(CattleScoreList);
        }

        public async Task<PartialViewResult> CattleNotification(int CattleID)
        {
            ViewBag.CattleId = CattleID;
            IList<NotificationsTable> currentNotifications = null;
            ISessionFactory sessionCattleTbl = FluentNHibernateHelper.Notifications_Session();
            ISession _sessionCattleTbl = sessionCattleTbl.OpenSession();
            IList<CattleTbl> currentCattle = _sessionCattleTbl.QueryOver<CattleTbl>().Where(x => x.FarmID == 3 && x.ID == CattleID).List<CattleTbl>();
            _sessionCattleTbl.Close();
            sessionCattleTbl.Close();
            _sessionCattleTbl.Dispose();
            sessionCattleTbl.Dispose();

            if (currentCattle.Count != 0)
            {
                String NotificationTag = "#Tag_CattleNotificationSettingID_" + currentCattle[0].animalNumber;
                ISessionFactory sessionNotifications = FluentNHibernateHelper.Notifications_Session();
                ISession _sessionNotifications = sessionNotifications.OpenSession();
                currentNotifications = _sessionNotifications.QueryOver<NotificationsTable>().Where(x => x.TagName == NotificationTag).List<NotificationsTable>();
                _sessionNotifications.Close();
                sessionNotifications.Close();
                _sessionNotifications.Dispose();
                sessionNotifications.Dispose();

                for (int i = 0; i < currentNotifications.Count; i++)
                {
                    SnoozeMessageModel _SnoozeMessageString = JsonConvert.DeserializeObject<SnoozeMessageModel>(currentNotifications[i].SnoozeMsg);
                    currentNotifications[i].SnoozeMsg = _SnoozeMessageString.SnoozeMessage[_SnoozeMessageString.SnoozeMessage.Count - 1];
                }
            }

            return PartialView(currentNotifications);
        }

        public PartialViewResult CattleLocation(int CattleID)
        {
            CattlePosition _CattlePosition = new CattlePosition();
            ISession mContext = Context.Open();
            List<SensorTbl> MACList = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == CattleID).List().ToList();
            if (MACList.Count != 0)
            {
                ViewBag.MAC = MACList[0].MacAddress;
            }
            else
            {
                ViewBag.MAC = "";
            }

            Context.Close(mContext);
            return PartialView(_CattlePosition);
        }

        [HttpPost]
        public String CattlePosition(String MAC)
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            var SensorList = mContext.QueryOver<SensorTbl>().Where(x => x.MacAddress == MAC).List().ToList();
            if (SensorList.Count != 0)
            {
                int CattleId = SensorList[0].cattleId;
                var CurrentPosition = mContext.QueryOver<CattlePositionTbl>().Where(x => x.cattleId == CattleId).OrderBy(x => x.ID).Desc.Take(1).SingleOrDefault();
                if (CurrentPosition != null)
                {
                    retValue = CurrentPosition.Latitude.ToString().Replace("/", ".") + "_";
                    retValue += CurrentPosition.Longitude.ToString().Replace("/", ".");
                }
                else
                {
                }
            }
            else
            {
            }
            Context.Close(mContext);
            return retValue;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SetScore(int CattleId, double Value, ScoreTypes ScoreItem, String ScoreDate)
        {
            DateTime ScoreDateTime = DateTime.Now;
            if (ScoreDate != null)
            {
                ScoreDate = ScoreDate.Replace("۰", "0");
                ScoreDate = ScoreDate.Replace("۱", "1");
                ScoreDate = ScoreDate.Replace("۲", "2");
                ScoreDate = ScoreDate.Replace("۳", "3");
                ScoreDate = ScoreDate.Replace("۴", "4");
                ScoreDate = ScoreDate.Replace("۵", "5");
                ScoreDate = ScoreDate.Replace("۶", "6");
                ScoreDate = ScoreDate.Replace("۷", "7");
                ScoreDate = ScoreDate.Replace("۸", "8");
                ScoreDate = ScoreDate.Replace("۹", "9");
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("fa-IR"))
                {
                    String[] SplitedScoreDate = ScoreDate.Split(' ')[0].Split('/');
                    String[] SplitedScoreDateHour = ScoreDate.Split(' ')[1].Split(':');
                    ScoreDateTime = DateHelper.toGeorjian(
                        Convert.ToInt16(SplitedScoreDate[0]),
                        Convert.ToInt16(SplitedScoreDate[1]),
                        Convert.ToInt16(SplitedScoreDate[2]),
                        Convert.ToInt16(SplitedScoreDateHour[0]),
                        Convert.ToInt16(SplitedScoreDateHour[1]),
                        0);
                }
            }

            ISession mContext = Context.Open();
            mContext.Clear();

            int ScoreItemId = 0;
            CattleTbl _cattle = mContext.Get<CattleTbl>(CattleId);
            switch (ScoreItem)
            {
                case ScoreTypes.Body_Condition_Score:
                    _cattle.Body_Condition_Score = Value;
                    _cattle.Body_Condition_ScoreDate = ScoreDateTime;
                    ScoreItemId = 0;
                    break;
                case ScoreTypes.Cleanliness:
                    _cattle.Cleanliness = Value;
                    _cattle.CleanlinessDate = ScoreDateTime;
                    ScoreItemId = 1;
                    break;
                case ScoreTypes.Hock:
                    _cattle.Hock = Value;
                    _cattle.HockDate = ScoreDateTime;
                    ScoreItemId = 2;
                    break;
                case ScoreTypes.Mobility:
                    _cattle.Mobility = Value;
                    _cattle.MobilityDate = ScoreDateTime;
                    ScoreItemId = 3;
                    break;
                case ScoreTypes.Manure:
                    _cattle.Manure = Value;
                    _cattle.ManureDate = ScoreDateTime;
                    ScoreItemId = 4;
                    break;
                case ScoreTypes.Rumen:
                    _cattle.Rumen = Value;
                    _cattle.RumenDate = ScoreDateTime;
                    ScoreItemId = 5;
                    break;
                case ScoreTypes.Teat:
                    _cattle.Teat = Value;
                    _cattle.TeatDate = ScoreDateTime;
                    ScoreItemId = 6;
                    break;
                case ScoreTypes.Milk_Production:
                    _cattle.milkAvg = Value;
                    _cattle.milkAvgDate = ScoreDateTime;
                    ScoreItemId = 7;
                    break;
                default:
                    break;
            }
            mContext.Update(_cattle);
            mContext.Flush();

            mContext.Clear();
            CattleScoreTbl _NewScore = new CattleScoreTbl()
            {
                item = Enum.GetName(typeof(ScoreTypes), ScoreItemId),
                value = Value,
                CattleId = CattleId,
                Date = ScoreDateTime,
                UserId = Helper.Helper.getCurrentUserId(),
                UserName = Helper.Helper.getCurrentUserNameFamily()
            };
            mContext.Save(_NewScore);
            Context.Close(mContext);

            ViewBag.cattleId = CattleId;
            return Json("OK");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UnSetScore(int scoreId)
        {
            ISession mContext = Context.Open();
            try
            {
                CattleScoreTbl curCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.ID == scoreId).SingleOrDefault();
                if (curCattleScore != null)
                {
                    String item = curCattleScore.item;
                    CattleScoreTbl LastCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                    mContext.Clear();
                    if (LastCattleScore != null)
                    {
                        switch (LastCattleScore.item)
                        {
                            case "Body_Condition_Score":
                                String deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                CattleScoreTbl OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Body_Condition_Score = 0;
                                    Cattle.Body_Condition_ScoreDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Body_Condition_Score = OldCattleScore.value;
                                    Cattle.Body_Condition_ScoreDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Cleanliness":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Cleanliness = 0;
                                    Cattle.CleanlinessDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Cleanliness = OldCattleScore.value;
                                    Cattle.CleanlinessDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Hock":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Hock = 0;
                                    Cattle.HockDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Hock = OldCattleScore.value;
                                    Cattle.HockDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Mobility":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Mobility = 0;
                                    Cattle.MobilityDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Mobility = OldCattleScore.value;
                                    Cattle.MobilityDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Manure":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Manure = 0;
                                    Cattle.ManureDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Manure = OldCattleScore.value;
                                    Cattle.ManureDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Rumen":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Rumen = 0;
                                    Cattle.RumenDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Rumen = OldCattleScore.value;
                                    Cattle.RumenDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Teat":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.Teat = 0;
                                    Cattle.TeatDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.Teat = OldCattleScore.value;
                                    Cattle.TeatDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            case "Milk_Production":
                                deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleScoreTbl", scoreId);
                                mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();

                                OldCattleScore = mContext.QueryOver<CattleScoreTbl>().Where(x => x.item == item).Where(x => x.CattleId == LastCattleScore.CattleId).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault();
                                if (OldCattleScore == null)
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(LastCattleScore.CattleId);
                                    Cattle.milkAvg = 0;
                                    Cattle.milkAvgDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                else
                                {
                                    CattleTbl Cattle = mContext.Get<CattleTbl>(OldCattleScore.CattleId);
                                    Cattle.milkAvg = OldCattleScore.value;
                                    Cattle.milkAvgDate = DateTime.Now;
                                    mContext.Update(Cattle);
                                    mContext.Flush();
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
            }
            Context.Close(mContext);
            return Json("");
        }

        private static List<CattleEvent> CattleEventList = new List<DomainClass.CattleEvent>();

        public async Task<PartialViewResult> CattleEvent(int CattleID)
        {
            List<CattleEvent> model = new List<DomainClass.CattleEvent>();

            ISession mContext = Context.Open();
            IList<CattleHeatStateTbl> _CattleHeatStateTbl = mContext.QueryOver<CattleHeatStateTbl>().Where(x => x.cattleId == CattleID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.date).Desc.List<CattleHeatStateTbl>();
            IList<CattleHealthStateTbl> _CattleHealthStateTbl = mContext.QueryOver<CattleHealthStateTbl>().Where(x => x.cattleId == CattleID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.date).Desc.List<CattleHealthStateTbl>();
            IList<CattleFertilityStateTbl> _CattleFertilityStateTbl = mContext.QueryOver<CattleFertilityStateTbl>().Where(x => x.cattleId == CattleID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.date).Desc.List<CattleFertilityStateTbl>();
            IList<CattleVetTbl> _CattleVetTbl = mContext.QueryOver<CattleVetTbl>().Where(x => x.cattleId == CattleID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.date).Desc.List<CattleVetTbl>();

            List<CattleHerds> _CattleHerdsList = mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            _CattleHerdsList.AddRange(mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == -1).List().ToList());

            List<CattleGroupTbl> _CattleGroupTbl = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            _CattleGroupTbl.AddRange(mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == -1).List().ToList());

            IList<FreeStallTbl> _FreeStallTbl = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List();
            IList<CattleTransfer> _CattleTransfer = mContext.QueryOver<CattleTransfer>().Where(x => x.CattleID == CattleID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.date).Desc.List();

            Context.Close(mContext);
            for (int i = 0; i < _CattleHeatStateTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleHeatStateTbl[i].ID;
                item.cattleId = CattleID;
                item.date = _CattleHeatStateTbl[i].date;
                item.description = _CattleHeatStateTbl[i].description;
                item.FarmID = Helper.Helper.getCurrentFarmId();
                item.value = Localization.getString("Heat_Event") + " _ " + Localization.getString(_CattleHeatStateTbl[i].Value);
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                UserInfo.NameFamily = _CattleHeatStateTbl[i].UserName;
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleHealthStateTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleHealthStateTbl[i].ID;
                item.cattleId = CattleID;
                item.date = _CattleHealthStateTbl[i].date;
                item.description = _CattleHealthStateTbl[i].description;
                item.FarmID = Helper.Helper.getCurrentFarmId();
                item.value = Localization.getString("HealthState") + " _ " + Localization.getString(_CattleHealthStateTbl[i].Value);
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                UserInfo.NameFamily = _CattleHealthStateTbl[i].UserName;
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleFertilityStateTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleFertilityStateTbl[i].ID;
                item.cattleId = CattleID;
                item.date = _CattleFertilityStateTbl[i].date;
                item.description = _CattleFertilityStateTbl[i].description;
                item.FarmID = Helper.Helper.getCurrentFarmId();
                item.value = Localization.getString("Fertility_Status") + " _ " + Localization.getString(_CattleFertilityStateTbl[i].Value);
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                UserInfo.NameFamily = _CattleFertilityStateTbl[i].UserName;
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleVetTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleVetTbl[i].ID;
                item.cattleId = CattleID;
                item.date = _CattleVetTbl[i].date;
                item.description = _CattleVetTbl[i].description;
                item.FarmID = Helper.Helper.getCurrentFarmId();
                item.value = Localization.getString("Vet") + " - " + Localization.getString(_CattleVetTbl[i].Status);
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                UserInfo.NameFamily = _CattleVetTbl[i].UserName;
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleHerdsList.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleHerdsList[i].ID;
                item.cattleId = -1;
                item.date = DateTime.Now;
                item.description = _CattleHerdsList[i].Description;
                item.FarmID = _CattleHerdsList[i].FarmID;
                item.value = _CattleHerdsList[i].name;
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleGroupTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleGroupTbl[i].ID;
                item.cattleId = -2;
                item.date = DateTime.Now;
                item.description = _CattleGroupTbl[i].Description;
                item.FarmID = _CattleGroupTbl[i].FarmID;
                item.value = _CattleGroupTbl[i].name;
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _FreeStallTbl.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _FreeStallTbl[i].ID;
                item.cattleId = -3;
                item.date = DateTime.Now;
                item.description = _FreeStallTbl[i].Description;
                item.FarmID = _FreeStallTbl[i].FarmID;
                item.value = _FreeStallTbl[i].name;
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                item.User = UserInfo;

                model.Add(item);
            }
            for (int i = 0; i < _CattleTransfer.Count; i++)
            {
                CattleEvent item = new DomainClass.CattleEvent();
                item.ID = _CattleTransfer[i].ID;
                item.cattleId = CattleID;
                item.date = _CattleTransfer[i].date;
                if (_CattleTransfer[i].OldTopicName.Equals("NaN"))
                {
                    item.description = "دام به " + " " + _CattleTransfer[i].NewTopicName + " " + " انتفال یافت." + "\n\r\n " + _CattleTransfer[i].Description;
                }
                else
                {
                    item.description = "" + "دام از " + " " + _CattleTransfer[i].OldTopicName + " " + " به " + " " + _CattleTransfer[i].NewTopicName + " " + " انتفال یافت." + "\n\r\n " + _CattleTransfer[i].Description;
                }
                item.FarmID = _CattleTransfer[i].FarmID;
                item.value = _CattleTransfer[i].Topic;
                SmartCattleUser UserInfo = new SmartCattleUser();
                UserInfo.Email = Helper.Helper.getCurrentUserEmail();
                UserInfo.NameFamily = _CattleTransfer[i].UserName;
                item.User = UserInfo;

                model.Add(item);
            }

            CattleEventList = model.OrderByDescending(x => x.date).ToList();
            for (int i = 0; i < CattleEventList.Count; i++)
            {
                CattleEventList[i].IDIndex = i;
            }
            ViewBag.cattleId = CattleID;
            return PartialView(CattleEventList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SetEvent(int cattleId, CattleEvent_Category cattleEvent_Category, Heat_Event_Subcategory heat_Event_Subcategory,
            HealthState_Subcategory healthState_Subcategory, Fertility_Status_Subcategory fertility_Status_Subcategory, Vet_Subcategory vet_Subcategory,
            String Herd_Subcategory, String Group_Subcategory, String FreeStall_Subcategory, String description, String EventDate)
        {
            int CurrentUser = Helper.Helper.getCurrentUserId();
            Core.ActionMessage _ActionMessage = new Core.ActionMessage();
            _ActionMessage.title = "title";
            _ActionMessage.content = "content";
            _ActionMessage.value = "value";
            _ActionMessage.type = Core.messageType.success;

            SaveEvent(cattleId, cattleEvent_Category, heat_Event_Subcategory,
            healthState_Subcategory, fertility_Status_Subcategory, vet_Subcategory,
            Herd_Subcategory, Group_Subcategory, FreeStall_Subcategory, description, EventDate);

            ViewBag.CattleId = cattleId;
            return Json(_ActionMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SetEventList(String SelectedCattles, CattleEvent_Category cattleEvent_Category, Heat_Event_Subcategory heat_Event_Subcategory,
            HealthState_Subcategory healthState_Subcategory, Fertility_Status_Subcategory fertility_Status_Subcategory, Vet_Subcategory vet_Subcategory,
            String Herd_Subcategory, String Group_Subcategory, String FreeStall_Subcategory, String description, String EventDate)
        {
            SelectedCattles = SelectedCattles.Replace("CattleNumber_", "");
            if(!SelectedCattles.Equals(""))
            {
                String[] SplitedCattles = SelectedCattles.Split(',');
                foreach (var cattle in SplitedCattles)
                {
                    try
                    {
                        int cattleId = Convert.ToInt32(cattle);
                        SaveEvent(cattleId, 
                            cattleEvent_Category, 
                            heat_Event_Subcategory, 
                            healthState_Subcategory, 
                            fertility_Status_Subcategory, 
                            vet_Subcategory,
                            Herd_Subcategory, 
                            Group_Subcategory, 
                            FreeStall_Subcategory, 
                            description, EventDate);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }

            return Json("");
        }

        private void SaveEvent(int cattleId, CattleEvent_Category cattleEvent_Category, Heat_Event_Subcategory heat_Event_Subcategory,
            HealthState_Subcategory healthState_Subcategory, Fertility_Status_Subcategory fertility_Status_Subcategory, Vet_Subcategory vet_Subcategory,
            String Herd_Subcategory, String Group_Subcategory, String FreeStall_Subcategory, String description, String EventDate)
        {
            DateTime EventDateTime = DateTime.Now;
            if (EventDate != null)
            {
                EventDate = EventDate.Replace("۰", "0");
                EventDate = EventDate.Replace("۱", "1");
                EventDate = EventDate.Replace("۲", "2");
                EventDate = EventDate.Replace("۳", "3");
                EventDate = EventDate.Replace("۴", "4");
                EventDate = EventDate.Replace("۵", "5");
                EventDate = EventDate.Replace("۶", "6");
                EventDate = EventDate.Replace("۷", "7");
                EventDate = EventDate.Replace("۸", "8");
                EventDate = EventDate.Replace("۹", "9");
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("fa-IR"))
                {
                    String[] SplitedEventDate = EventDate.Split(' ')[0].Split('/');
                    String[] SplitedEventDateHour = EventDate.Split(' ')[1].Split(':');

                    EventDateTime = DateHelper.toGeorjian(
                        Convert.ToInt16(SplitedEventDate[0]),
                        Convert.ToInt16(SplitedEventDate[1]),
                        Convert.ToInt16(SplitedEventDate[2]),
                        Convert.ToInt16(SplitedEventDateHour[0]),
                        Convert.ToInt16(SplitedEventDateHour[1]),
                        0);
                }
            }

            switch (cattleEvent_Category)
            {
                case CattleEvent_Category.Select:

                    break;

                case CattleEvent_Category.Heat_Event:
                    switch (heat_Event_Subcategory)
                    {
                        case Heat_Event_Subcategory.Select:

                            break;

                        case Heat_Event_Subcategory.Suspicious:
                            ISession mContext = Context.Open();
                            mContext.Clear();
                            CattleHeatStateTbl _CattleHeatStateTbl = new CattleHeatStateTbl();
                            _CattleHeatStateTbl.Status = "Suspicious";
                            _CattleHeatStateTbl.Value = "Suspicious";
                            _CattleHeatStateTbl.description = description;
                            _CattleHeatStateTbl.cattleId = cattleId;
                            _CattleHeatStateTbl.date = EventDateTime;
                            _CattleHeatStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHeatStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleHeatStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleHeatStateTbl);

                            mContext.Clear();
                            CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.heatStatus = "Suspicious";
                            _cattle.heatStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);

                            break;

                        case Heat_Event_Subcategory.Heat:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleHeatStateTbl = new CattleHeatStateTbl();
                            _CattleHeatStateTbl.Status = "Heat";
                            _CattleHeatStateTbl.description = description;
                            _CattleHeatStateTbl.Value = "Heat";
                            _CattleHeatStateTbl.cattleId = cattleId;
                            _CattleHeatStateTbl.date = EventDateTime;
                            _CattleHeatStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHeatStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleHeatStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleHeatStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.heatStatus = "Heat";
                            _cattle.heatStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        case Heat_Event_Subcategory.Normal:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleHeatStateTbl = new CattleHeatStateTbl();
                            _CattleHeatStateTbl.Status = "Normal";
                            _CattleHeatStateTbl.description = description;
                            _CattleHeatStateTbl.Value = "Normal";
                            _CattleHeatStateTbl.cattleId = cattleId;
                            _CattleHeatStateTbl.date = EventDateTime;
                            _CattleHeatStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHeatStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleHeatStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleHeatStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.heatStatus = "Normal";
                            _cattle.heatStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        default:
                            break;
                    }
                    break;

                case CattleEvent_Category.HealthState:
                    switch (healthState_Subcategory)
                    {
                        case HealthState_Subcategory.Select:

                            break;

                        case HealthState_Subcategory.Suspicious:
                            ISession mContext = Context.Open();
                            mContext.Clear();
                            CattleHealthStateTbl _CattleHealthStateTbl = new CattleHealthStateTbl();
                            _CattleHealthStateTbl.Status = "Suspicious";
                            _CattleHealthStateTbl.description = description;
                            _CattleHealthStateTbl.Value = "Suspicious";
                            _CattleHealthStateTbl.cattleId = cattleId;
                            _CattleHealthStateTbl.date = EventDateTime;
                            _CattleHealthStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHealthStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            _CattleHealthStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            mContext.Save(_CattleHealthStateTbl);

                            mContext.Clear();
                            CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.healthStatus = "Suspicious";
                            _cattle.healthStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        case HealthState_Subcategory.Health:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleHealthStateTbl = new CattleHealthStateTbl();
                            _CattleHealthStateTbl.Status = "Health";
                            _CattleHealthStateTbl.description = description;
                            _CattleHealthStateTbl.Value = "Health";
                            _CattleHealthStateTbl.cattleId = cattleId;
                            _CattleHealthStateTbl.date = EventDateTime;
                            _CattleHealthStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHealthStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            _CattleHealthStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            mContext.Save(_CattleHealthStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.healthStatus = "Health";
                            _cattle.healthStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        case HealthState_Subcategory.Sick:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleHealthStateTbl = new CattleHealthStateTbl();
                            _CattleHealthStateTbl.Status = "Sick";
                            _CattleHealthStateTbl.description = description;
                            _CattleHealthStateTbl.Value = "Sick";
                            _CattleHealthStateTbl.cattleId = cattleId;
                            _CattleHealthStateTbl.date = EventDateTime;
                            _CattleHealthStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleHealthStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            _CattleHealthStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            mContext.Save(_CattleHealthStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.healthStatus = "Sick";
                            _cattle.healthStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        default:
                            break;
                    }
                    break;

                case CattleEvent_Category.Fertility_Status:
                    switch (fertility_Status_Subcategory)
                    {
                        case Fertility_Status_Subcategory.Select:
                            break;
                        case Fertility_Status_Subcategory.Open:
                            ISession mContext = Context.Open();
                            mContext.Clear();
                            CattleFertilityStateTbl _CattleFertilityStateTbl = new CattleFertilityStateTbl();
                            _CattleFertilityStateTbl.Status = "Open";
                            _CattleFertilityStateTbl.description = description;
                            _CattleFertilityStateTbl.Value = "Open";
                            _CattleFertilityStateTbl.cattleId = cattleId;
                            _CattleFertilityStateTbl.date = EventDateTime;
                            _CattleFertilityStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleFertilityStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleFertilityStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleFertilityStateTbl);

                            mContext.Clear();
                            CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.fertilityStatus = "Open";
                            _cattle.fertilityStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;
                        case Fertility_Status_Subcategory.Insemination:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleFertilityStateTbl = new CattleFertilityStateTbl();
                            _CattleFertilityStateTbl.Status = "Insemination";
                            _CattleFertilityStateTbl.description = description;
                            _CattleFertilityStateTbl.Value = "Insemination";
                            _CattleFertilityStateTbl.cattleId = cattleId;
                            _CattleFertilityStateTbl.date = EventDateTime;
                            _CattleFertilityStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleFertilityStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleFertilityStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleFertilityStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.fertilityStatus = "Insemination";
                            _cattle.fertilityStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;
                        case Fertility_Status_Subcategory.Pregnant:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleFertilityStateTbl = new CattleFertilityStateTbl();
                            _CattleFertilityStateTbl.Status = "Pregnant";
                            _CattleFertilityStateTbl.description = description;
                            _CattleFertilityStateTbl.Value = "Pregnant";
                            _CattleFertilityStateTbl.cattleId = cattleId;
                            _CattleFertilityStateTbl.date = EventDateTime;
                            _CattleFertilityStateTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleFertilityStateTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleFertilityStateTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleFertilityStateTbl);

                            mContext.Clear();
                            _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.fertilityStatus = "Pregnant";
                            _cattle.fertilityStatusDate = EventDateTime;
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;
                        default:
                            break;
                    }
                    break;

                case CattleEvent_Category.Vet:
                    switch (vet_Subcategory)
                    {
                        case Vet_Subcategory.Select:
                            break;

                        case Vet_Subcategory.Examination:
                            ISession mContext = Context.Open();
                            mContext.Clear();
                            CattleVetTbl _CattleVetTbl = new CattleVetTbl();
                            _CattleVetTbl.Status = "Examination";
                            _CattleVetTbl.description = description;
                            _CattleVetTbl.cattleId = cattleId;
                            _CattleVetTbl.date = EventDateTime;
                            _CattleVetTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleVetTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleVetTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleVetTbl);

                            Context.Close(mContext);
                            break;

                        case Vet_Subcategory.Treatment:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleVetTbl = new CattleVetTbl();
                            _CattleVetTbl.Status = "Treatment";
                            _CattleVetTbl.description = description;
                            _CattleVetTbl.cattleId = cattleId;
                            _CattleVetTbl.date = EventDateTime;
                            _CattleVetTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleVetTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleVetTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleVetTbl);

                            Context.Close(mContext);
                            break;

                        case Vet_Subcategory.Calving:
                            mContext = Context.Open();
                            mContext.Clear();
                            _CattleVetTbl = new CattleVetTbl();
                            _CattleVetTbl.Status = "Calving";
                            _CattleVetTbl.description = description;
                            _CattleVetTbl.cattleId = cattleId;
                            _CattleVetTbl.date = EventDateTime;
                            _CattleVetTbl.FarmID = Helper.Helper.getCurrentFarmId();
                            _CattleVetTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                            _CattleVetTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                            mContext.Save(_CattleVetTbl);

                            mContext.Clear();
                            CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                            _cattle.fertilityStatus = "Open";
                            _cattle.fertilityStatusDate = EventDateTime;
                            _cattle.lastCalvingDate = EventDateTime;
                            _cattle.lactationNumber = _cattle.lactationNumber + 1;
                            _cattle.CattleGroupId = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "تازه زا").Select(x => x.ID).SingleOrDefault<int>();
                            mContext.Update(_cattle);
                            mContext.Flush();

                            Context.Close(mContext);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            if (!Herd_Subcategory.Equals("Select"))
            {
                ISession mContext = Context.Open();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                CattleHerds _OldCattleHerds = mContext.QueryOver<CattleHerds>().Where(x => x.ID == _cattle.CattleHerd_ID).SingleOrDefault<CattleHerds>();
                CattleHerds _NewCattleHerds = mContext.QueryOver<CattleHerds>().Where(x => x.ID == Convert.ToInt32(Herd_Subcategory)).SingleOrDefault<CattleHerds>();
                CattleTransfer _CattleTransfer = new CattleTransfer()
                {
                    CattleID = cattleId,
                    Topic = "Herd_Subcategory",
                    Description = description,
                    NewTopicID = _NewCattleHerds.ID,
                    OldTopicID = _cattle.CattleHerd_ID,
                    NewTopicName = _NewCattleHerds.name,
                    OldTopicName = _OldCattleHerds == null ? "NaN" : _OldCattleHerds.name,
                    FarmID = Helper.Helper.getCurrentFarmId(),
                    UserName = Helper.Helper.getCurrentUserNameFamily(),
                    UserIdentity = Helper.Helper.getCurrentUserId(),
                    date = EventDateTime
                };
                mContext.Save(_CattleTransfer);
                _cattle.CattleHerd_ID = _NewCattleHerds.ID;
                mContext.Update(_cattle);
                mContext.Flush();
                Context.Close(mContext);
            }

            if (!Group_Subcategory.Equals("Select"))
            {
                ISession mContext = Context.Open();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                CattleGroupTbl _OldCattleGroup = mContext.QueryOver<CattleGroupTbl>().Where(x => x.ID == _cattle.CattleGroupId).SingleOrDefault<CattleGroupTbl>();
                CattleGroupTbl _NewCattleGroup = mContext.QueryOver<CattleGroupTbl>().Where(x => x.ID == Convert.ToInt32(Group_Subcategory)).SingleOrDefault<CattleGroupTbl>();
                CattleTransfer _CattleTransfer = new CattleTransfer()
                {
                    CattleID = cattleId,
                    Topic = "Group_Subcategory",
                    Description = description,
                    NewTopicID = _NewCattleGroup.ID,
                    OldTopicID = _cattle.CattleGroupId,
                    NewTopicName = _NewCattleGroup.name,
                    OldTopicName = _OldCattleGroup == null ? "NaN" : _OldCattleGroup.name,
                    FarmID = Helper.Helper.getCurrentFarmId(),
                    UserName = Helper.Helper.getCurrentUserNameFamily(),
                    UserIdentity = Helper.Helper.getCurrentUserId(),
                    date = EventDateTime
                };
                mContext.Save(_CattleTransfer);
                _cattle.CattleGroupId = _NewCattleGroup.ID;
                mContext.Update(_cattle);
                mContext.Flush();
                Context.Close(mContext);
            }

            if (!FreeStall_Subcategory.Equals("Select"))
            {
                ISession mContext = Context.Open();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                FreeStallTbl _OldCattleFreeStall = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == _cattle.FreeStallId).SingleOrDefault<FreeStallTbl>();
                FreeStallTbl _NewCattleFreeStall = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == Convert.ToInt32(FreeStall_Subcategory)).SingleOrDefault<FreeStallTbl>();
                CattleTransfer _CattleTransfer = new CattleTransfer()
                {
                    CattleID = cattleId,
                    Topic = "FreeStall_Subcategory",
                    Description = description,
                    NewTopicID = _NewCattleFreeStall.ID,
                    OldTopicID = _cattle.FreeStallId,
                    NewTopicName = _NewCattleFreeStall.name,
                    OldTopicName = _OldCattleFreeStall == null ? "NaN" : _OldCattleFreeStall.name,
                    FarmID = Helper.Helper.getCurrentFarmId(),
                    UserName = Helper.Helper.getCurrentUserNameFamily(),
                    UserIdentity = Helper.Helper.getCurrentUserId(),
                    date = EventDateTime
                };
                mContext.Save(_CattleTransfer);
                _cattle.FreeStallId = _NewCattleFreeStall.ID;
                mContext.Update(_cattle);
                mContext.Flush();
                Context.Close(mContext);
            }

            ViewBag.CattleId = cattleId;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UnSetEvent(int eventId)
        {
            ISession mContext = Context.Open();
            try
            {
                var item = CattleEventList[eventId];
                String Heat_Event = Localization.getString("Heat_Event");
                String Health_State = Localization.getString("HealthState");
                String Vet_State = Localization.getString("Vet");
                String Fertility_Status = Localization.getString("Fertility_Status");
                String Herd_Subcategory = ("Herd_Subcategory");
                String Group_Subcategory = ("Group_Subcategory");
                String FreeStall_Subcategory = ("FreeStall_Subcategory");

                if (item.value.Contains(Heat_Event))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleHeatStateTbl", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateCattleHeat(mContext, item.cattleId);
                }
                else if (item.value.Contains(Health_State))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleHealthStateTbl", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateCattleHealth(mContext, item.cattleId);
                }
                else if (item.value.Contains(Fertility_Status))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleFertilityStateTbl", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateFerility(mContext, item.cattleId);
                }
                else if (item.value.Contains(Vet_State))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleVetTbl", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    if (item.value.Contains(Localization.getString("Diagnosis")))
                    {
                        updateCattleHealth(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Examination")))
                    {
                        updateFerility(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Treatment")))
                    {
                        updateFerility(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Insemination")))
                    {
                        updateFerility(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Abortion")))
                    {
                        updateFerility(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Calving")))
                    {
                        updateCalving(mContext, item.cattleId);
                    }
                    else if (item.value.Contains(Localization.getString("Pregnancy")))
                    {
                        updateFerility(mContext, item.cattleId);
                    }
                }
                else if (item.value.Contains(Herd_Subcategory))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleTransfer", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateCattleHedr(mContext, item.cattleId);
                }
                else if (item.value.Contains(Group_Subcategory))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleTransfer", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateCattleGroup(mContext, item.cattleId);
                }
                else if (item.value.Contains(FreeStall_Subcategory))
                {
                    String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleTransfer", item.ID);
                    mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();

                    mContext.Clear();
                    updateCattleFreeStall(mContext, item.cattleId);
                }
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
            }

            Context.Close(mContext);
            return Json("");
        }

        private void updateCattleHealth(ISession mContext, int cattleId)
        {
            CattleHealthStateTbl LastItem = mContext.QueryOver<CattleHealthStateTbl>().Where(x => x.cattleId == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                if (_cattle.ID != 0)
                {
                    _cattle.healthStatus = LastItem.Status;
                    _cattle.healthStatusDate = LastItem.date;
                    mContext.Update(_cattle);
                    mContext.Flush();
                }
            }
            else
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.healthStatus = "NULL";
                _cattle.healthStatusDate = DateTime.Now;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        private void updateCattleHeat(ISession mContext, int cattleId)
        {
            CattleHeatStateTbl LastItem = mContext.QueryOver<CattleHeatStateTbl>().Where(x => x.cattleId == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                if (_cattle.ID != 0)
                {
                    _cattle.heatStatus = LastItem.Status;
                    _cattle.heatStatusDate = LastItem.date;
                    mContext.Update(_cattle);
                    mContext.Flush();
                }
            }
            else
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.heatStatus = "NULL";
                _cattle.heatStatusDate = DateTime.Now;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        private void updateFerility(ISession mContext, int cattleId)
        {
            CattleVetTbl LastItem = mContext.QueryOver<CattleVetTbl>().Where(x => x.cattleId == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                if (LastItem.Status.Contains(("Treatment")))
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                    if (_cattle.ID != 0)
                    {
                        _cattle.fertilityStatus = "NULL";
                        _cattle.fertilityStatusDate = LastItem.date;
                        mContext.Update(_cattle);
                        mContext.Flush();
                    }
                }
                else if (LastItem.Status.Contains(("Examination")))
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                    if (_cattle.ID != 0)
                    {
                        _cattle.fertilityStatus = "NULL";
                        _cattle.fertilityStatusDate = LastItem.date;
                        mContext.Update(_cattle);
                        mContext.Flush();
                    }
                }
                else if (LastItem.Status.Contains(("Diagnosis")))
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                    if (_cattle.ID != 0)
                    {
                        _cattle.fertilityStatus = "NULL";
                        _cattle.fertilityStatusDate = LastItem.date;
                        mContext.Update(_cattle);
                        mContext.Flush();
                    }
                }
                else if (LastItem.Status.Contains(("Calving")))
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                    if (_cattle.ID != 0)
                    {
                        _cattle.fertilityStatus = "Open";
                        _cattle.fertilityStatusDate = LastItem.date;
                        _cattle.lastCalvingDate = LastItem.date;
                        mContext.Update(_cattle);
                        mContext.Flush();
                    }
                }
                else
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                    if (_cattle.ID != 0)
                    {
                        _cattle.fertilityStatus = LastItem.Status;
                        _cattle.fertilityStatusDate = LastItem.date;
                        mContext.Update(_cattle);
                        mContext.Flush();
                    }
                }
            }
            else
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.fertilityStatus = "NULL";
                _cattle.fertilityStatusDate = DateTime.Now;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        private void updateCalving(ISession mContext, int cattleId)
        {
            CattleVetTbl LastItem = mContext.QueryOver<CattleVetTbl>().Where(x => x.cattleId == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.cattleId);
                if (_cattle != null)
                {
                    if (_cattle.ID != 0)
                    {
                        if (LastItem.Status.Contains(("Calving")))
                        {
                            _cattle.fertilityStatus = "Open";
                            _cattle.fertilityStatusDate = LastItem.date;
                            _cattle.lastCalvingDate = LastItem.date;
                            _cattle.lactationNumber = _cattle.lactationNumber == 0 ? 0 : _cattle.lactationNumber - 1;
                            _cattle.CattleGroupId = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "تازه زا").Select(x => x.ID).SingleOrDefault<int>();
                            mContext.Update(_cattle);
                            mContext.Flush();
                        }
                        else
                        {
                            _cattle.fertilityStatus = LastItem.Status;
                            _cattle.fertilityStatusDate = LastItem.date;
                            _cattle.lastCalvingDate = DateTime.ParseExact("1999/01/01", "yyyy/mm/dd", null);
                            _cattle.lactationNumber = _cattle.lactationNumber == 0 ? 0 : _cattle.lactationNumber - 1;
                            //_cattle.CattleGroupId = -1;
                            mContext.Update(_cattle);
                            mContext.Flush();
                            updateCattleGroup(mContext, cattleId);
                        }
                    }
                }
            }
            else
            {
                CattleFertilityStateTbl LastItem_CattleFertilityStateTbl = mContext.QueryOver<CattleFertilityStateTbl>().Where(x => x.cattleId == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
                if (LastItem_CattleFertilityStateTbl != null)
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                    if (_cattle != null)
                    {
                        if (_cattle.ID != 0)
                        {
                            _cattle.fertilityStatus = LastItem_CattleFertilityStateTbl.Status;
                            _cattle.fertilityStatusDate = DateTime.Now;
                            _cattle.lastCalvingDate = DateTime.ParseExact("1999/01/01", "yyyy/mm/dd", null);
                            _cattle.lactationNumber = _cattle.lactationNumber == 0 ? 0 : _cattle.lactationNumber - 1;
                            //_cattle.CattleGroupId = -1;
                            mContext.Update(_cattle);
                            mContext.Flush();
                            updateCattleGroup(mContext, cattleId);
                        }
                    }
                }
                else
                {
                    CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                    if (_cattle != null)
                    {
                        if (_cattle.ID != 0)
                        {
                            _cattle.fertilityStatus = "NULl";
                            _cattle.fertilityStatusDate = DateTime.Now;
                            _cattle.lastCalvingDate = DateTime.ParseExact("1999/01/01", "yyyy/mm/dd", null);
                            _cattle.lactationNumber = _cattle.lactationNumber == 0 ? 0 : _cattle.lactationNumber - 1;
                            //_cattle.CattleGroupId = -1;
                            mContext.Update(_cattle);
                            mContext.Flush();
                            updateCattleGroup(mContext, cattleId);
                        }
                    }
                }

            }
        }

        private void updateCattleHedr(ISession mContext, int cattleId)
        {
            CattleTransfer LastItem = mContext.QueryOver<CattleTransfer>().Where(x => x.Topic == "Herd_Subcategory").Where(x => x.CattleID == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.CattleID);
                if (_cattle.ID != 0)
                {
                    _cattle.CattleHerd_ID = LastItem.NewTopicID;
                    mContext.Update(_cattle);
                    mContext.Flush();
                }
            }
            else
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.CattleHerd_ID = -1;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        private void updateCattleGroup(ISession mContext, int cattleId)
        {
            CattleTransfer LastItem = mContext.QueryOver<CattleTransfer>().Where(x => x.Topic == "Group_Subcategory").Where(x => x.CattleID == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.CattleID);
                if (_cattle.ID != 0)
                {
                    _cattle.CattleGroupId = LastItem.NewTopicID;
                    mContext.Update(_cattle);
                    mContext.Flush();
                }
            }
            else
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.CattleGroupId = -1;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        private void updateCattleFreeStall(ISession mContext, int cattleId)
        {
            CattleTransfer LastItem = mContext.QueryOver<CattleTransfer>().Where(x => x.Topic == "FreeStall_Subcategory").Where(x => x.CattleID == cattleId).OrderBy(x => x.date).Desc().Take(1).SingleOrDefault();
            if (LastItem != null)
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(LastItem.CattleID);
                if (_cattle.ID != 0)
                {
                    _cattle.FreeStallId = LastItem.NewTopicID;
                    mContext.Update(_cattle);
                    mContext.Flush();
                }
            }
            else
            {
                mContext.Clear();
                CattleTbl _cattle = mContext.Get<CattleTbl>(cattleId);
                _cattle.FreeStallId = -1;
                mContext.Update(_cattle);
                mContext.Flush();
            }
        }

        [HttpPost]
        public String getMap(String MAC)
        {
            String retValueMain = "";
            try
            {
                var requestGetMap = (HttpWebRequest)WebRequest.Create("http://79.175.133.194:2222/getMaps?apiKey=a43f6670-9d37-11e7-ad9d-819a9b28ee42&spId=0&maps=[{\"mapSuperType\":\"zoning\",\"mapSubSuperType\":\"physical\"}]");
                var responseGetMap = (HttpWebResponse)requestGetMap.GetResponse();
                String rawJsonGetMap = new StreamReader(responseGetMap.GetResponseStream()).ReadToEnd();

                var requestGetZone = (HttpWebRequest)WebRequest.Create("http://79.175.133.194:2222/getZone?spId=0&apiKey=a43f6670-9d37-11e7-ad9d-819a9b28ee42&zoneId=0&limit=100&MAC=" + MAC);
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

        [HttpPost]
        public JsonResult getSpecTemperature(String MAC, String date_and_step, String cultureInfo)
        {
            List<getSpecTemperatureModel> SpecTemperatureModel = new List<getSpecTemperatureModel>();

            if (date_and_step != "")
            {
                String[] inputDateSplitedAndClear = Utility.DecodeUrlString(date_and_step).Replace("from=", "").Replace("to=", "").Replace("step=", "").Split('&');
                string fromDateStr = Utility.toEnglishNumber(inputDateSplitedAndClear[0]);
                String[] splitedFromDate = fromDateStr.Split('/');
                DateTime StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.PersianCalendar());
                if (cultureInfo == "fa-IR")
                {
                    StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.PersianCalendar());
                }
                else
                {
                    StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.GregorianCalendar());
                }

                string toDateStr = inputDateSplitedAndClear[1];
                String[] splitedToDate = toDateStr.Split('/');
                DateTime EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.PersianCalendar());
                if (cultureInfo == "fa-IR")
                {
                    EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.PersianCalendar());
                }
                else
                {
                    EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.GregorianCalendar());
                }

                int stepMinutes = 0;
                try
                {
                    stepMinutes = Convert.ToInt16(inputDateSplitedAndClear[2]);
                }
                catch (Exception ex)
                {
                    stepMinutes = 100;
                }

                if (StartDate < EndDate)
                {
                    SpecTemperatureModel = getSpecTemp(MAC, StartDate, EndDate, stepMinutes, cultureInfo);
                }
            }
            else
            {
                int stepMinutes = 100;
                DateTime DateTimeNow = DateTime.Now;
                DateTime AgoDateTimeNow = DateTimeNow.AddHours(-240);
                SpecTemperatureModel = getSpecTemp(MAC, AgoDateTimeNow, DateTimeNow, stepMinutes, cultureInfo);
            }

            return Json(SpecTemperatureModel, JsonRequestBehavior.AllowGet);
        }

        private List<getSpecTemperatureModel> getSpecTemp(String MAC, DateTime StartDate, DateTime EndDate, int stepMinute, String cultureInfo)
        {
            ISession mContext = Context.Open();

            List<getSpecTemperatureModel> retSpecTemperatureModel = new List<getSpecTemperatureModel>();

            var dateTimeStart = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, StartDate.Minute, StartDate.Second, 0, DateTimeKind.Unspecified);
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            String start = (dateTimeStart.ToUniversalTime() - epochStart).TotalMilliseconds.ToString();

            var dateTimeEnd = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndDate.Hour, EndDate.Minute, EndDate.Second, 0, DateTimeKind.Unspecified);
            var epochEnd = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            String end = (dateTimeEnd.ToUniversalTime() - epochEnd).TotalMilliseconds.ToString();

            int CurrentFarmId = Helper.Helper.getCurrentFarmId();
            SensorTbl SensorModel = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == CurrentFarmId && x.MacAddress == MAC).SingleOrDefault();

            X_CattleData cattleDataModel = new X_CattleData();
            cattleDataModel.FarmId = Helper.Helper.getCurrentFarmId();
            cattleDataModel.CattleId = SensorModel.cattleId;
            if (cultureInfo == "fa-IR")
            {
                String tmpDate = StartDate.ToString();
                ///////////////////////////////////////
                //           :-)))(((-:              //
                ///////////////////////////////////////
                if (tmpDate.Contains("1397"))
                {
                    cattleDataModel.StartTime = Utility.ConvertFromPersian(StartDate.ToString("yyyy/MM/dd HH:mm"));
                    cattleDataModel.EndTime = Utility.ConvertFromPersian(EndDate.ToString("yyyy/MM/dd HH:mm"));
                }
                else
                {
                    cattleDataModel.StartTime = StartDate.ToString("yyyy-MM-dd HH:mm");
                    cattleDataModel.EndTime = EndDate.ToString("yyyy-MM-dd HH:mm");
                }
            }
            else
            {
                cattleDataModel.StartTime = StartDate.ToString("yyyy-MM-dd HH:mm");
                cattleDataModel.EndTime = EndDate.ToString("yyyy-MM-dd HH:mm");
            }

            cattleDataModel.Step = 10;

            List<TempretureTbl> CattleTempModels = Temperature(cattleDataModel);

            DateTime LastSaveddate = StartDate;
            if (CattleTempModels != null)
            {
                for (int q = 0; q < CattleTempModels.Count; q++)
                {

                    String tmpDateStr = Utility.ConvertToPersianWithTime(CattleTempModels[q].date);
                    String[] splitedToDate = tmpDateStr.Split(' ');
                    String[] splitedYear = splitedToDate[0].Split('/');
                    String[] splitedTime = splitedToDate[1].Split(':');
                    DateTime tmpDate = new DateTime(
                        Convert.ToInt16(splitedYear[0]),
                        Convert.ToInt16(splitedYear[1]),
                        Convert.ToInt16(splitedYear[2]),
                        Convert.ToInt16(splitedTime[0]),
                        Convert.ToInt16(splitedTime[1]),
                        0, new System.Globalization.PersianCalendar());

                    getSpecTemperatureModel tmpSpecTemp = new getSpecTemperatureModel();
                    tmpSpecTemp._id = CattleTempModels[q].ID;
                    tmpSpecTemp.tObj = Math.Round(CattleTempModels[q].value, 1);
                    tmpSpecTemp.detectorTime = CattleTempModels[q].dateStr;
                    tmpSpecTemp.MAC = "Hello";

                    retSpecTemperatureModel.Add(tmpSpecTemp);
                }
            }
            Context.Close(mContext);
            return retSpecTemperatureModel;
        }

        class TimeBudgetPlotConfig
        {
            public static int Window = 6;
            public static double Period = 1;
        }

        [HttpPost]
        public JsonResult getSpecTimeBudget(String MAC, String date_and_step, String cultureInfo)
        {
            JsonResult retvalue = getSpecTimeBudgets(MAC, date_and_step, cultureInfo);
            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSpecTimeBudgets(String MAC, String date_step_cattleId, String cultureInfo)
        {
            ISession mContext = Context.Open();
            X_CattleSensorValues CattleSensorsModels = new X_CattleSensorValues();

            List<SensorTbl> obj = new List<SensorTbl>();
            int farmId = Helper.Helper.getCurrentFarmId();
            SensorTbl SensorModel = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == farmId && x.MacAddress == MAC).SingleOrDefault();
            Context.Close(mContext);
            if (date_step_cattleId != "")
            {
                try
                {
                    String[] inputDateSplitedAndClear = Utility.DecodeUrlString(date_step_cattleId).Replace("from=", "").Replace("to=", "").Replace("step=", "").Replace("cattleId=", "").Split('&');

                    string fromDateStr = Utility.toEnglishNumber(inputDateSplitedAndClear[0]);
                    String[] splitedFromDate = fromDateStr.Split('/');
                    DateTime StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.PersianCalendar());

                    string toDateStr = inputDateSplitedAndClear[1];
                    String[] splitedToDate = toDateStr.Split('/');
                    DateTime EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.PersianCalendar());
                    if (splitedToDate[0].Equals("1397"))
                    {
                        StartDate = DateHelper.toGeorjian(
                            Convert.ToInt16(splitedFromDate[0]), 
                            Convert.ToInt16(splitedFromDate[1]), 
                            Convert.ToInt16(splitedFromDate[2]));

                        EndDate = DateHelper.toGeorjian(
                            Convert.ToInt16(splitedToDate[0]), 
                            Convert.ToInt16(splitedToDate[1]), 
                            Convert.ToInt16(splitedToDate[2]));
                    }
                    else
                    {
                        StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.GregorianCalendar());
                        EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.GregorianCalendar());
                    }

                    int stepHours = 0;
                    try
                    {
                        stepHours = Convert.ToInt16(inputDateSplitedAndClear[2]);
                    }
                    catch (Exception ex)
                    {
                        stepHours = 12;
                    }

                    if (StartDate < EndDate)
                    {
                        X_CattleData x_cattle = new X_CattleData();
                        x_cattle.FarmId = Helper.Helper.getCurrentFarmId();
                        x_cattle.CattleId = SensorModel.cattleId;
                        x_cattle.Step = stepHours;
                        x_cattle.StartTime = StartDate.ToString("yyyy-MM-dd HH:mm");
                        x_cattle.EndTime = EndDate.ToString("yyyy-MM-dd HH:mm");

                        CattleSensorsModels = Budget(StartDate, EndDate, x_cattle.CattleId, x_cattle.Step);//1397/06/01 00:00
                        for (int i = 0; i < CattleSensorsModels.CattleActivity.Count; i++)
                        {
                            if (CattleSensorsModels.CattleActivity[i].Date.Contains("1397"))
                            {
                                String[] NewSplitedDate = CattleSensorsModels.CattleActivity[i].Date.Split(' ');
                                String[] NewYear = NewSplitedDate[0].Split('/');
                                String[] NewTime = NewSplitedDate[1].Split(':');
                                DateTime NewDate = DateHelper.toGeorjian(
                                        Convert.ToInt16(NewYear[0]),
                                        Convert.ToInt16(NewYear[1]),
                                        Convert.ToInt16(NewYear[2]),
                                        Convert.ToInt16(NewTime[0]),
                                        Convert.ToInt16(NewTime[1]),
                                        0);
                                CattleSensorsModels.CattleActivity[i].Date = NewDate.ToString("yyyy/MM/dd HH:mm");
                            }
                        }

                        if (CattleSensorsModels.CattleActivity == null)
                        {
                            CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                        }
                    }
                    else
                    {
                        return Json(CattleSensorsModels, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Cattle451: " + ex.Message);
                    String tmp = ex.Message;
                    if (CattleSensorsModels.CattleActivity == null)
                    {
                        CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                    }
                }
            }
            else
            {
                X_CattleData x_cattle = new X_CattleData();
                x_cattle.FarmId = Helper.Helper.getCurrentFarmId();
                x_cattle.CattleId = SensorModel.cattleId;
                x_cattle.Step = 1;
                x_cattle.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                x_cattle.EndTime = DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd HH:mm");

                CattleSensorsModels = Budget(DateTime.Now, DateTime.Now.AddHours(-12), x_cattle.CattleId, x_cattle.Step);
                if (CattleSensorsModels.CattleActivity == null)
                {
                    CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                }
            }
            double maxDrinking = 1;
            double maxEating = 1;
            double maxRuminating = 1;
            double maxSitting = 1;
            double maxStanding = 1;
            double maxWalking = 1;
            for (int i = 0; i < CattleSensorsModels.CattleActivity.Count; i++)
            {
                if (CattleSensorsModels.CattleActivity[i].drinking > maxDrinking)
                {
                    maxDrinking = CattleSensorsModels.CattleActivity[i].drinking;
                }
                if (CattleSensorsModels.CattleActivity[i].eating > maxEating)
                {
                    maxEating = CattleSensorsModels.CattleActivity[i].eating;
                }
                if (CattleSensorsModels.CattleActivity[i].ruminating > maxRuminating)
                {
                    maxRuminating = CattleSensorsModels.CattleActivity[i].ruminating;
                }
                if (CattleSensorsModels.CattleActivity[i].sitting > maxSitting)
                {
                    maxSitting = CattleSensorsModels.CattleActivity[i].sitting;
                }
                if (CattleSensorsModels.CattleActivity[i].standing > maxStanding)
                {
                    maxStanding = CattleSensorsModels.CattleActivity[i].standing;
                }
                if (CattleSensorsModels.CattleActivity[i].walking > maxWalking)
                {
                    maxWalking = CattleSensorsModels.CattleActivity[i].walking;
                }
            }
            for (int i = 0; i < CattleSensorsModels.CattleActivity.Count; i++)
            {
                CattleSensorsModels.CattleActivity[i].drinking = Math.Round(CattleSensorsModels.CattleActivity[i].drinking / maxDrinking, 2);
                CattleSensorsModels.CattleActivity[i].eating = Math.Round(CattleSensorsModels.CattleActivity[i].eating / maxEating, 2);
                CattleSensorsModels.CattleActivity[i].ruminating = Math.Round(CattleSensorsModels.CattleActivity[i].ruminating / maxRuminating, 2);
                CattleSensorsModels.CattleActivity[i].sitting = Math.Round(CattleSensorsModels.CattleActivity[i].sitting / maxSitting, 2);
                CattleSensorsModels.CattleActivity[i].standing = Math.Round(CattleSensorsModels.CattleActivity[i].standing / maxStanding, 2);
                CattleSensorsModels.CattleActivity[i].walking = Math.Round(CattleSensorsModels.CattleActivity[i].walking / maxWalking, 2);
            }
            CattleSensorsModels.CattleActivity = CattleSensorsModels.CattleActivity.OrderBy(x => x.Date).ToList();
            return Json(CattleSensorsModels, JsonRequestBehavior.AllowGet);
        }

        public X_CattleSensorValues Budget(DateTime minValueDateTime, DateTime maxValueDateTime, int CattleId, int Step)
        {
            List<ActivityStateTbl> obj = new List<ActivityStateTbl>();

            int tmpFarmId = Helper.Helper.getCurrentFarmId();
            int tmpCattleId = CattleId;
            ISession mContext = Context.Open();
            List<ActivityStateTbl> Values = mContext.QueryOver<ActivityStateTbl>()
                .Where(x => x.FarmID == tmpFarmId && x.cattleId == tmpCattleId && x.date >= minValueDateTime && x.date <= maxValueDateTime).List().ToList();
            Context.Close(mContext);

            X_CattleSensorValues items = new X_CattleSensorValues();
            List<X_CattleSensorValues.Activity> activityList = new List<X_CattleSensorValues.Activity>();
            X_CattleSensorValues.Activity activityItem = new X_CattleSensorValues.Activity();
            if (Values.Count != 0)
            {
                items.ID = 1;
                items.FarmId = Helper.Helper.getCurrentFarmId();
                items.CattleId = CattleId;
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
                //double totalStep = (double)(maxValueDateTime - minValueDateTime).TotalHours / (double)TimeBudgetPlotConfig.Period;
                while(minValueDateTime <= maxValueDateTime)
                {
                    List<DateTime> tmp = dateTimeList.FindAll(item => item >= minValueDateTime && item <= minValueDateTime.AddHours(TimeBudgetPlotConfig.Window));
                    if(tmp.Count > 100)
                    {
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
                    }
                    
                    AllTotal = (double)totalStanding + (double)totalWalking + (double)totalEating + (double)totalDrinking + (double)totalSitting + (double)totalRuminating;
                    if (AllTotal != 0)
                    {
                        activityItem = new X_CattleSensorValues.Activity();
                        activityItem.standing = (Math.Round(Convert.ToDouble(100 * ((double)totalStanding / (double)AllTotal)), 2));
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
                    minValueDateTime = minValueDateTime.AddHours(TimeBudgetPlotConfig.Period);
                }

                items.CattleActivity = activityList;
            }
            else
            {

            }
            return items;
        }

        public List<TempretureTbl> Temperature(X_CattleData CattleModel)
        {
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

            if (sqlModel.Count > 0)
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
            return retValueModel;
        }

        private String Num2Str(int input)
        {
            String retValue = "0";
            if (input <= 9)
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
        public JsonResult getSpecActivity(String MAC, String date_step_cattleId, String cultureInfo)
        {
            ISession mContext = Context.Open();
            X_CattleSensorValues CattleSensorsModels = new X_CattleSensorValues();

            List<SensorTbl> obj = new List<SensorTbl>();
            int farmId = Helper.Helper.getCurrentFarmId();
            SensorTbl SensorModel = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == farmId && x.MacAddress == MAC).SingleOrDefault();
            Context.Close(mContext);
            if (date_step_cattleId != "")
            {
                try
                {
                    String[] inputDateSplitedAndClear = Utility.DecodeUrlString(date_step_cattleId).Replace("from=", "").Replace("to=", "").Replace("step=", "").Replace("cattleId=", "").Split('&');

                    string fromDateStr = Utility.toEnglishNumber(inputDateSplitedAndClear[0]);
                    String[] splitedFromDate = fromDateStr.Split('/');
                    DateTime StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.PersianCalendar());

                    string toDateStr = inputDateSplitedAndClear[1];
                    String[] splitedToDate = toDateStr.Split('/');
                    DateTime EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.PersianCalendar());
                    if(splitedToDate[0].Equals("1397"))
                    {
                        StartDate = DateHelper.toGeorjian(
                            Convert.ToInt16(splitedFromDate[0]), 
                            Convert.ToInt16(splitedFromDate[1]), 
                            Convert.ToInt16(splitedFromDate[2]));

                        EndDate = DateHelper.toGeorjian(
                            Convert.ToInt16(splitedToDate[0]), 
                            Convert.ToInt16(splitedToDate[1]), 
                            Convert.ToInt16(splitedToDate[2]));
                    }
                    else
                    {
                        StartDate = new DateTime(Convert.ToInt16(splitedFromDate[0]), Convert.ToInt16(splitedFromDate[1]), Convert.ToInt16(splitedFromDate[2]), new System.Globalization.GregorianCalendar());
                        EndDate = new DateTime(Convert.ToInt16(splitedToDate[0]), Convert.ToInt16(splitedToDate[1]), Convert.ToInt16(splitedToDate[2]), new System.Globalization.GregorianCalendar());
                    }

                    int stepHours = 0;
                    try
                    {
                        stepHours = Convert.ToInt16(inputDateSplitedAndClear[2]);
                    }
                    catch (Exception ex)
                    {
                        stepHours = 12;
                    }

                    if (StartDate < EndDate)
                    {
                        X_CattleData x_cattle = new X_CattleData();
                        x_cattle.FarmId = Helper.Helper.getCurrentFarmId();
                        x_cattle.CattleId = SensorModel.cattleId;
                        x_cattle.Step = stepHours;
                        x_cattle.StartTime = StartDate.ToString("yyyy-MM-dd HH:mm");
                        x_cattle.EndTime = EndDate.ToString("yyyy-MM-dd HH:mm");
                        
                        CattleSensorsModels = Activity(StartDate, EndDate, x_cattle.CattleId, x_cattle.Step);
                        if (CattleSensorsModels.CattleActivity == null)
                        {
                            CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                        }
                    }
                    else
                    {
                        return Json(CattleSensorsModels, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Cattle451: " + ex.Message);
                    String tmp = ex.Message;
                    if (CattleSensorsModels.CattleActivity == null)
                    {
                        CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                    }
                }
            }
            else
            {
                X_CattleData x_cattle = new X_CattleData();
                x_cattle.FarmId = Helper.Helper.getCurrentFarmId();
                x_cattle.CattleId = SensorModel.cattleId;
                x_cattle.Step = 1;
                x_cattle.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                x_cattle.EndTime = DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd HH:mm");

                CattleSensorsModels = Activity(DateTime.Now, DateTime.Now.AddHours(-12), x_cattle.CattleId, x_cattle.Step);
                if (CattleSensorsModels.CattleActivity == null)
                {
                    CattleSensorsModels.CattleActivity = new List<X_CattleSensorValues.Activity>();
                }
            }
            
            return Json(CattleSensorsModels, JsonRequestBehavior.AllowGet);
        }

        public X_CattleSensorValues Activity(DateTime minValueDateTime, DateTime maxValueDateTime, int CattleId, int Step)
        {
            List<ActivityStateTbl> obj = new List<ActivityStateTbl>();

            int tmpFarmId = Helper.Helper.getCurrentFarmId();
            int tmpCattleId = CattleId;
            ISession mContext = Context.Open();
            List<ActivityStateTbl> Values = mContext.QueryOver<ActivityStateTbl>()
                .Where(x => x.FarmID == tmpFarmId && x.cattleId == tmpCattleId && x.date >= minValueDateTime && x.date <= maxValueDateTime).List().ToList();
            Context.Close(mContext);

            X_CattleSensorValues items = new X_CattleSensorValues();
            List<X_CattleSensorValues.Activity> activityList = new List<X_CattleSensorValues.Activity>();
            X_CattleSensorValues.Activity activityItem = new X_CattleSensorValues.Activity();
            if (Values.Count != 0)
            {
                items.ID = 1;
                items.FarmId = Helper.Helper.getCurrentFarmId();
                items.CattleId = CattleId;
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
                double totalStep = (double)(maxValueDateTime - minValueDateTime).TotalHours / (double)Step;
                for (int i = 0; i < totalStep; i++)
                {
                    DateTime nextValueDateTime = minValueDateTime.AddHours(Step);
                    List<DateTime> tmp = dateTimeList.FindAll(item => item >= minValueDateTime && item <= nextValueDateTime);
                    for (int n = 0; n < tmp.Count; n++)
                    {
                        int index = dateTimeList.IndexOf(tmp[n]);
                        //totalStanding += Values[index].Standing;
                        totalWalking += Values[index].Walking;
                        totalEating += Values[index].Eating;
                        //totalDrinking += Values[index].Drinking;
                        totalSitting += Values[index].Sitting;
                        totalRuminating += Values[index].Rumination;
                    }
                    AllTotal = (double)totalStanding + (double)totalWalking + (double)totalEating + (double)totalDrinking + (double)totalSitting + (double)totalRuminating;
                    if (AllTotal != 0)
                    {
                        activityItem = new X_CattleSensorValues.Activity();
                        //activityItem.standing = (Math.Round(Convert.ToDouble(100 * ((double)totalStanding / (double)AllTotal)), 2));
                        activityItem.walking = (Math.Round(Convert.ToDouble(100 * ((double)totalWalking / (double)AllTotal)), 2));
                        activityItem.eating = (Math.Round(Convert.ToDouble(100 * ((double)totalEating / (double)AllTotal)), 2));
                        //activityItem.drinking = (Math.Round(Convert.ToDouble(100 * ((double)totalDrinking / (double)AllTotal)), 2));
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
            return items;
        }

        private String Int2Str(int input)
        {
            String retValue = "";
            if(input < 10)
            {
                retValue = "0" + input.ToString();
            }
            else
            {
                retValue = input.ToString();
            }
            return retValue;
        }

        private enum AllFieldsname
        {
            page,
            CattleID_Input,
            CattleGroupID_Input,
            FreeStallID_Input,
            CattleDIM_Input,
            CattleLactationNumber_Input,

            CattleID_Label,
            CattleGroupID_Label,
            FreeStallID_Label,
            CattleDIM_Label,
            CattleLactationNumber_Label,
            FertilityState_Label,
            CattleHeatState_Label,
            CattleHealthState_Label
        }

        [AuthenticateFilter]
        public async Task<ActionResult> List()
        {
            ISession mContext = Context.Open();
            List<CattleGroupTbl> groups = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            ListModel model = new ListModel();

            using (SmartCattleContext context = new SmartCattleContext())
            {
                context.Configuration.LazyLoadingEnabled = true;
                int CurrentFarmId = Helper.Helper.getCurrentFarmId();
                mContext = Context.Open();

                IList<CattleTbl> cattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == CurrentFarmId).Take(ItemPerPage).List<CattleTbl>();
                model.CattleGroupList = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == CurrentFarmId).List<CattleGroupTbl>().ToList();
                model.CattleGroupList.AddRange(mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == -1).List<CattleGroupTbl>().ToList());
                model.FreeStallList = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == CurrentFarmId).List<FreeStallTbl>().ToList();

                model.total = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == CurrentFarmId).List().Count;
                model.pages = model.total / ItemPerPage;
                model.Cattles = cattles.ToList<CattleTbl>();
                if (model.total % ItemPerPage != 0)
                {
                    model.pages++;
                }

                model.current = 1;
                model.Groups = groups.ToList();

                ///////////////////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < model.Cattles.Count; i++)
                {
                    InitialCattleDim(ref model, i, mContext);
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////
                Context.Close(mContext);
            }
            return View(model);
        }

        public async Task<PartialViewResult> Advancefilter(String[] AllFields)
        {
            ListModel model = new ListModel();
            IList<CattleTbl> AllCattles = new List<CattleTbl>(); 
            bool hasExp = false;
            int page = Convert.ToInt32(AllFields[(int)AllFieldsname.page]);
            model.current = page;
            ISession mContext = null;
            try
            {
                mContext = Context.Open();
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
            }
            IQueryOver<CattleTbl, CattleTbl> query = mContext.QueryOver<CattleTbl>();

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Label].Replace(" ", "")))
                {
                    int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleID_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.animalNumber == animalNumber);
                            break;

                        case "برابر":
                            query = query.Where(x => x.animalNumber == animalNumber);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.animalNumber > animalNumber);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.animalNumber > animalNumber);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.animalNumber >= animalNumber);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.animalNumber >= animalNumber);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.animalNumber < animalNumber);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.animalNumber < animalNumber);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.animalNumber <= animalNumber);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.animalNumber <= animalNumber);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.animalNumber != animalNumber);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.animalNumber != animalNumber);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;

                        case "Sort ASC":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "Sort DESC":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;

                        case "به ترتیب نزولی":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "به ترتیب صعودی":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    query = query.Where(x => x.animalNumber == animalNumber);
                    hasExp = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Label].Replace(" ", "")))
                {
                    switch (AllFields[(int)AllFieldsname.CattleID_Label])
                    {
                        case "Sort ASC":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "Sort DESC":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;

                        case "به ترتیب نزولی":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "به ترتیب صعودی":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    //int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    //query = query.Where(x => x.animalNumber == animalNumber);
                    //hasExp = true;
                }
            }

            //if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleGroupID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleGroupID_Label].Replace(" ", "")))
                {
                    String GroupName = (AllFields[(int)AllFieldsname.CattleGroupID_Label]);
                    List<int> CattleGroupId = new List<int>();
                    try
                    {
                        CattleGroupId = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == GroupName).Select(x => x.ID).List<int>().ToList<int>();
                    }
                    catch (Exception ex)
                    {
                        String Ack = ex.Message;
                    }
                    if(CattleGroupId.Count == 0)
                    {
                        query = query.Where(x => x.CattleGroupId == 0);
                    }
                    else
                    {
                        query = query.Where(x => x.CattleGroupId == CattleGroupId[0]);
                    }
                    hasExp = true;
                }
                
            }

            //if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.FreeStallID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.FreeStallID_Label].Replace(" ", "")))
                {
                    String FreeStallName = AllFields[(int)AllFieldsname.FreeStallID_Label];
                    int FreeStallId = mContext.QueryOver<FreeStallTbl>().Where(x => x.name == FreeStallName).Select(x => x.ID).SingleOrDefault<int>();
                    query = query.Where(x => x.FreeStallId == FreeStallId);
                    hasExp = true;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleDIM_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleDIM_Label].Replace(" ", "")))
                {
                    int Dim = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleDIM_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleDIM_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.Dim == Dim);
                            break;

                        case "برابر":
                            query = query.Where(x => x.Dim == Dim);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.Dim > Dim);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.Dim > Dim);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.Dim >= Dim);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.Dim >= Dim);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.Dim < Dim);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.Dim < Dim);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.Dim <= Dim);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.Dim <= Dim);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.Dim != Dim);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.Dim != Dim);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int Dim = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleDIM_Input]);
                    query = query.Where(x => x.Dim == Dim);
                    hasExp = true;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleLactationNumber_Label].Replace(" ", "")))
                {
                    int lactationNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleLactationNumber_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.lactationNumber == lactationNumber);
                            break;

                        case "برابر":
                            query = query.Where(x => x.lactationNumber == lactationNumber);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.lactationNumber > lactationNumber);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.lactationNumber > lactationNumber);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.lactationNumber >= lactationNumber);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.lactationNumber >= lactationNumber);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.lactationNumber < lactationNumber);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.lactationNumber < lactationNumber);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.lactationNumber <= lactationNumber);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.lactationNumber <= lactationNumber);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.lactationNumber != lactationNumber);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.lactationNumber != lactationNumber);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int lactationNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]);
                    query = query.Where(x => x.lactationNumber == lactationNumber);
                    hasExp = true;
                }
            }

            int indexFerility = 11;
            if (!string.IsNullOrEmpty(AllFields[indexFerility].Replace(" ", "")))
            {
                String fertilityStatus = Convert.ToString(AllFields[indexFerility]);
                if(fertilityStatus.Replace(" ", "") != "")
                {
                    switch (fertilityStatus)
                    {
                        case "گوساله":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Calf" + "%"));
                            break;

                        case "خشک":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Dry" + "%"));
                            break;

                        case "تازه بارور شده":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Fresh" + "%"));
                            break;

                        case "تلیسه":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Heifer" + "%"));
                            break;

                        case "تلقیح":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Insemination" + "%"));
                            break;

                        case "باز":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Open" + "%"));
                            break;

                        case "آبستن":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Pregnant" + "%"));
                            break;

                        case "آماده":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Ready" + "%"));
                            break;

                        case "دیگر":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Other" + "%"));
                            break;

                        case "Calf":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Calf" + "%"));
                            break;

                        case "Dry off":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Dry" + "%"));
                            break;

                        case "Fresh":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Fresh" + "%"));
                            break;

                        case "Heifer":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Heifer" + "%"));
                            break;

                        case "Inseminated":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Insemination" + "%"));
                            break;

                        case "Open":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Open" + "%"));
                            break;

                        case "Pregnant":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Pregnant" + "%"));
                            break;

                        case "Ready":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Ready" + "%"));
                            break;

                        case "Other":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Other" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }

            int indexHeat = 12;
            if (!string.IsNullOrEmpty(AllFields[indexHeat].Replace(" ", "")))
            {
                String heatStatus = Convert.ToString(AllFields[indexHeat]);
                if(heatStatus.Replace(" ", "") != "")
                {
                    switch (heatStatus)
                    {
                        case "فحل":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "InHeat" + "%"));
                            break;

                        case "مستعد":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Potential" + "%"));
                            break;

                        case "مشكوك":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "InHeat":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "InHeat" + "%"));
                            break;

                        case "Potential":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Potential" + "%"));
                            break;

                        case "Suspicious":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Suspicious" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }

            int indexCattleHealthState = 13;
            if (!string.IsNullOrEmpty(AllFields[indexCattleHealthState].Replace(" ", "")))
            {
                String healthStatus = Convert.ToString(AllFields[indexCattleHealthState]);
                if(healthStatus.Replace(" ", "") != "")
                {
                    switch (healthStatus)
                    {
                        case "Suspicious":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "Sick":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Sick" + "%"));
                            break;

                        case "VerySick":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "VerySick" + "%"));
                            break;

                        case "NotMobility":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "NotMobility" + "%"));
                            break;

                        case "مشکوک":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "بيمار":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Sick" + "%"));
                            break;

                        case "بسیار بیمار":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "VerySick" + "%"));
                            break;

                        case "بی حرکت":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "NotMobility" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }
            
            if (hasExp)
            {
                try
                {
                    AllCattles = query/*.Skip((model.current - 1) * ItemPerPage)*/.Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Take(ItemPerPage).List<CattleTbl>();
                    model.total = query/*.Skip((model.current - 1) * ItemPerPage)*/.Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<CattleTbl>().Count();
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
            }
            else
            {
                AllCattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Skip((model.current - 1) * ItemPerPage).Take(ItemPerPage).List<CattleTbl>();
                model.total = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<CattleTbl>().Count();
            }

            Context.Close(mContext);

            model.pages = model.total / ItemPerPage;
            if (model.total % ItemPerPage != 0)
            {
                model.pages++;
            }
            if (model.pages < page)
            {
                page = 1;
            }

            try
            {
                model.groupId = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleGroupID_Input]);
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
                model.groupId = -1;
            }

            model.Cattles = new List<CattleTbl>();
            if (AllCattles != null)
            {
                for (int i = 0; i < AllCattles.Count; i++)
                {
                    CattleTbl tmp = new CattleTbl();
                    tmp.ID = AllCattles[i].ID;
                    tmp.animalNumber = AllCattles[i].animalNumber;
                    tmp.age = AllCattles[i].age;
                    tmp.FarmID = AllCattles[i].FarmID;
                    tmp.CattleGroupId = AllCattles[i].CattleGroupId;
                    tmp.fertilityStatus = AllCattles[i].fertilityStatus;
                    tmp.FreeStallId = AllCattles[i].FreeStallId;
                    tmp.healthStatus = AllCattles[i].healthStatus;
                    tmp.heatStatus = AllCattles[i].heatStatus;
                    tmp.lactationNumber = AllCattles[i].lactationNumber;
                    tmp.lastCalvingDate = AllCattles[i].lastCalvingDate;

                    try
                    {
                        model.Cattles.Add(tmp);
                    }
                    catch (Exception ex)
                    {
                        String Ack = ex.Message;
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < model.Cattles.Count; i++)
            {
                InitialCattleDim(ref model, i, mContext);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////
            return PartialView("filter", model);
        }

        public async Task<PartialViewResult> AdvanceEditGroupOfCattlesfilter(String[] AllFields)
        {
            ListModel model = new ListModel();
            IList<CattleTbl> AllCattles = new List<CattleTbl>();
            bool hasExp = false;
            int page = Convert.ToInt32(AllFields[(int)AllFieldsname.page]);
            model.current = page;
            ISession mContext = null;
            try
            {
                mContext = Context.Open();
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
            }
            IQueryOver<CattleTbl, CattleTbl> query = mContext.QueryOver<CattleTbl>();

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Label].Replace(" ", "")))
                {
                    int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleID_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.animalNumber == animalNumber);
                            break;

                        case "برابر":
                            query = query.Where(x => x.animalNumber == animalNumber);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.animalNumber > animalNumber);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.animalNumber > animalNumber);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.animalNumber >= animalNumber);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.animalNumber >= animalNumber);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.animalNumber < animalNumber);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.animalNumber < animalNumber);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.animalNumber <= animalNumber);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.animalNumber <= animalNumber);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.animalNumber != animalNumber);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.animalNumber != animalNumber);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;

                        case "Sort ASC":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "Sort DESC":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;

                        case "به ترتیب نزولی":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "به ترتیب صعودی":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    query = query.Where(x => x.animalNumber == animalNumber);
                    hasExp = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleID_Label].Replace(" ", "")))
                {
                    switch (AllFields[(int)AllFieldsname.CattleID_Label])
                    {
                        case "Sort ASC":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "Sort DESC":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;

                        case "به ترتیب نزولی":
                            query = query.OrderBy(x => x.animalNumber).Asc;
                            break;

                        case "به ترتیب صعودی":
                            query = query.OrderBy(x => x.animalNumber).Desc;
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    //int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleID_Input]);
                    //query = query.Where(x => x.animalNumber == animalNumber);
                    //hasExp = true;
                }
            }

            //if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleGroupID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleGroupID_Label].Replace(" ", "")))
                {
                    String GroupName = (AllFields[(int)AllFieldsname.CattleGroupID_Label]);
                    List<int> CattleGroupId = new List<int>();
                    try
                    {
                        CattleGroupId = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == GroupName).Select(x => x.ID).List<int>().ToList<int>();
                    }
                    catch (Exception ex)
                    {
                        String Ack = ex.Message;
                    }
                    if (CattleGroupId.Count == 0)
                    {
                        query = query.Where(x => x.CattleGroupId == 0);
                    }
                    else
                    {
                        query = query.Where(x => x.CattleGroupId == CattleGroupId[0]);
                    }
                    hasExp = true;
                }

            }

            try
            {
                //if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.FreeStallID_Input]))
                {
                    if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.FreeStallID_Label].Replace(" ", "")))
                    {
                        String FreeStallName = AllFields[(int)AllFieldsname.FreeStallID_Label];
                        int FreeStallId = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Where(x => x.name == FreeStallName).Select(x => x.ID).Take(1).SingleOrDefault<int>();
                        query = query.Where(x => x.FreeStallId == FreeStallId);
                        hasExp = true;
                    }
                }
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleDIM_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleDIM_Label].Replace(" ", "")))
                {
                    int Dim = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleDIM_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleDIM_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.Dim == Dim);
                            break;

                        case "برابر":
                            query = query.Where(x => x.Dim == Dim);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.Dim > Dim);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.Dim > Dim);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.Dim >= Dim);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.Dim >= Dim);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.Dim < Dim);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.Dim < Dim);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.Dim <= Dim);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.Dim <= Dim);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.Dim != Dim);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.Dim != Dim);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int Dim = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleDIM_Input]);
                    query = query.Where(x => x.Dim == Dim);
                    hasExp = true;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.CattleLactationNumber_Label].Replace(" ", "")))
                {
                    int lactationNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]);
                    switch (AllFields[(int)AllFieldsname.CattleLactationNumber_Label])
                    {
                        case "Equal":
                            query = query.Where(x => x.lactationNumber == lactationNumber);
                            break;

                        case "برابر":
                            query = query.Where(x => x.lactationNumber == lactationNumber);
                            break;

                        case "GreateThan":
                            query = query.Where(x => x.lactationNumber > lactationNumber);
                            break;

                        case "بزرگتر":
                            query = query.Where(x => x.lactationNumber > lactationNumber);
                            break;

                        case "GreateThanOrEqualTo":
                            query = query.Where(x => x.lactationNumber >= lactationNumber);
                            break;

                        case "بزرگتر یا مساوی":
                            query = query.Where(x => x.lactationNumber >= lactationNumber);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            query = query.Where(x => x.lactationNumber < lactationNumber);
                            break;

                        case "کوچکتر":
                            query = query.Where(x => x.lactationNumber < lactationNumber);
                            break;

                        case "LessThanOrEqualTo":
                            query = query.Where(x => x.lactationNumber <= lactationNumber);
                            break;

                        case "کوچکتر یا مساوی":
                            query = query.Where(x => x.lactationNumber <= lactationNumber);
                            break;

                        case "NotEqual":
                            query = query.Where(x => x.lactationNumber != lactationNumber);
                            break;

                        case "نابرابر":
                            query = query.Where(x => x.lactationNumber != lactationNumber);
                            break;

                        case "NotIsNull":
                            break;

                        case "ناتهی":
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    int lactationNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleLactationNumber_Input]);
                    query = query.Where(x => x.lactationNumber == lactationNumber);
                    hasExp = true;
                }
            }

            int indexFerility = 11;
            if (!string.IsNullOrEmpty(AllFields[indexFerility].Replace(" ", "")))
            {
                String fertilityStatus = Convert.ToString(AllFields[indexFerility]);
                if (fertilityStatus.Replace(" ", "") != "")
                {
                    switch (fertilityStatus)
                    {
                        case "گوساله":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Calf" + "%"));
                            break;

                        case "خشک":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Dry" + "%"));
                            break;

                        case "تازه بارور شده":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Fresh" + "%"));
                            break;

                        case "تلیسه":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Heifer" + "%"));
                            break;

                        case "تلقیح":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Insemination" + "%"));
                            break;

                        case "باز":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Open" + "%"));
                            break;

                        case "آبستن":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Pregnant" + "%"));
                            break;

                        case "آماده":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Ready" + "%"));
                            break;

                        case "دیگر":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Other" + "%"));
                            break;

                        case "Calf":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Calf" + "%"));
                            break;

                        case "Dry off":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Dry" + "%"));
                            break;

                        case "Fresh":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Fresh" + "%"));
                            break;

                        case "Heifer":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Heifer" + "%"));
                            break;

                        case "Inseminated":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Insemination" + "%"));
                            break;

                        case "Open":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Open" + "%"));
                            break;

                        case "Pregnant":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Pregnant" + "%"));
                            break;

                        case "Ready":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Ready" + "%"));
                            break;

                        case "Other":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.fertilityStatus).IsLike("%" + "Other" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }

            int indexHeat = 12;
            if (!string.IsNullOrEmpty(AllFields[indexHeat].Replace(" ", "")))
            {
                String heatStatus = Convert.ToString(AllFields[indexHeat]);
                if (heatStatus.Replace(" ", "") != "")
                {
                    switch (heatStatus)
                    {
                        case "فحل":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "InHeat" + "%"));
                            break;

                        case "مستعد":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Potential" + "%"));
                            break;

                        case "مشكوك":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "InHeat":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "InHeat" + "%"));
                            break;

                        case "Potential":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Potential" + "%"));
                            break;

                        case "Suspicious":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.heatStatus).IsLike("%" + "Suspicious" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }

            int indexCattleHealthState = 13;
            if (!string.IsNullOrEmpty(AllFields[indexCattleHealthState].Replace(" ", "")))
            {
                String healthStatus = Convert.ToString(AllFields[indexCattleHealthState]);
                if (healthStatus.Replace(" ", "") != "")
                {
                    switch (healthStatus)
                    {
                        case "Suspicious":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "Sick":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Sick" + "%"));
                            break;

                        case "VerySick":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "VerySick" + "%"));
                            break;

                        case "NotMobility":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "NotMobility" + "%"));
                            break;

                        case "Health":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Health" + "%"));
                            break;

                        case "مشکوک":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Suspicious" + "%"));
                            break;
                            
                        case "مشكوك":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Suspicious" + "%"));
                            break;

                        case "بيمار":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Sick" + "%"));
                            break;

                        case "بسیار بیمار":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "VerySick" + "%"));
                            break;

                        case "بی حرکت":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "NotMobility" + "%"));
                            break;

                        case "سلامت":
                            query = query.Where(Restrictions.On<CattleTbl>(x => x.healthStatus).IsLike("%" + "Health" + "%"));
                            break;
                    }
                    hasExp = true;
                }
            }

            if (hasExp)
            {
                try
                {
                    AllCattles = query/*.Skip((model.current - 1) * ItemPerPage)*/.Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Take(ItemPerPage).List<CattleTbl>();
                    model.total = query/*.Skip((model.current - 1) * ItemPerPage)*/.Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<CattleTbl>().Count();
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
            }
            else
            {
                AllCattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).Skip((model.current - 1) * ItemPerPage).Take(ItemPerPage).List<CattleTbl>();
                model.total = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<CattleTbl>().Count();
            }

            Context.Close(mContext);

            model.pages = model.total / ItemPerPage;
            if (model.total % ItemPerPage != 0)
            {
                model.pages++;
            }
            if (model.pages < page)
            {
                page = 1;
            }

            try
            {
                model.groupId = Convert.ToInt32(AllFields[(int)AllFieldsname.CattleGroupID_Input]);
            }
            catch (Exception ex)
            {
                String Ack = ex.Message;
                model.groupId = -1;
            }

            model.Cattles = new List<CattleTbl>();
            if (AllCattles != null)
            {
                for (int i = 0; i < AllCattles.Count; i++)
                {
                    CattleTbl tmp = new CattleTbl();
                    tmp.ID = AllCattles[i].ID;
                    tmp.animalNumber = AllCattles[i].animalNumber;
                    tmp.age = AllCattles[i].age;
                    tmp.FarmID = AllCattles[i].FarmID;
                    tmp.CattleGroupId = AllCattles[i].CattleGroupId;
                    tmp.fertilityStatus = AllCattles[i].fertilityStatus;
                    tmp.FreeStallId = AllCattles[i].FreeStallId;
                    tmp.healthStatus = AllCattles[i].healthStatus;
                    tmp.heatStatus = AllCattles[i].heatStatus;
                    tmp.lactationNumber = AllCattles[i].lactationNumber;
                    tmp.lastCalvingDate = AllCattles[i].lastCalvingDate;

                    try
                    {
                        model.Cattles.Add(tmp);
                    }
                    catch (Exception ex)
                    {
                        String Ack = ex.Message;
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < model.Cattles.Count; i++)
            {
                InitialCattleDim(ref model, i, mContext);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////

            return PartialView("filter_edit_group", model);
        }

        private CattleDetailModel getDetail(int CattleId)
        {
            CattleDetailModel Model = new CattleDetailModel();
            using (SmartCattleContext context = new SmartCattleContext())
            {
                CattleTbl cattle = new CattleTbl();
                ISession mContext = Context.Open();
                IList<CattlesScoreTbl> CattlesScoreList = new List<CattlesScoreTbl>();
                try
                {
                    InitialCattleList(CattleId, ref cattle, ref Model, mContext);
                }
                catch (Exception ex)
                {
                    String Ack = ex.Message;
                    Context.Close(mContext);
                }

                CattlesScoreTbl tmpCattlesScoreTbl = new CattlesScoreTbl();
                if (cattle != null)
                {
                    tmpCattlesScoreTbl.BodyCondition = cattle.Body_Condition_Score;
                    tmpCattlesScoreTbl.BodyConditionDate = cattle.Body_Condition_ScoreDate;
                    tmpCattlesScoreTbl.Cleanliness = cattle.Cleanliness;
                    tmpCattlesScoreTbl.CleanlinessDate = cattle.CleanlinessDate;
                    tmpCattlesScoreTbl.Hock = cattle.Hock;
                    tmpCattlesScoreTbl.HockDate = cattle.HockDate;
                    tmpCattlesScoreTbl.Mobility = cattle.Mobility;
                    tmpCattlesScoreTbl.MobilityDate = cattle.MobilityDate;
                    tmpCattlesScoreTbl.Manure = cattle.Manure;
                    tmpCattlesScoreTbl.ManureDate = cattle.ManureDate;
                    tmpCattlesScoreTbl.Rumen = cattle.Rumen;
                    tmpCattlesScoreTbl.RumenDate = cattle.RumenDate;
                    tmpCattlesScoreTbl.Teat = cattle.Teat;
                    tmpCattlesScoreTbl.TeatDate = cattle.TeatDate;
                }

                Model.CattlesScore = tmpCattlesScoreTbl;
            }
            return Model;
        }

        private void InitialCattleList(int CattleId, ref CattleTbl cattle, ref CattleDetailModel Model, ISession mContext)
        {
            cattle = mContext.Get<CattleTbl>(CattleId);
            if (cattle != null)
            {
                Model.cattle = cattle;
                InitialCattleDim(cattle, ref Model, mContext);
                
                if (cattle.fertilityStatus.Equals("Open") || cattle.fertilityStatus.Equals("Insemination"))
                {
                    if (cattle.lactationNumber == 0)
                    {
                        DateTime CattleBirthday = cattle.birthDate;
                        Model.OpenDays = (DateTime.Now - CattleBirthday).Days;
                    }
                    else
                    {
                        DateTime lastCalvingDate = cattle.lastCalvingDate;
                        Model.OpenDays = (DateTime.Now - lastCalvingDate).Days;
                    }
                }
                else if (cattle.fertilityStatus.Equals("Pregnant"))
                {
                    Model.OpenDays = 0;
                }
                Context.Close(mContext);
            }
            else
            {

            }
        }

        private void InitialCattleDim(ref ListModel model, int i, ISession mContext)
        {
            if (model.Cattles[i].lactationNumber == 0)
            {
                model.Cattles[i].Dim = 0;
            }
            else
            {
                int NotDryGroupID = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "خشک").Select(x => x.ID).SingleOrDefault<int>();
                int RemoveableGroupID = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "حذفی").Select(x => x.ID).SingleOrDefault<int>();
                if(model.Cattles[i].CattleGroupId == 0)
                {
                    DateTime lastCalvingDate = model.Cattles[i].lastCalvingDate;
                    model.Cattles[i].Dim = (DateTime.Now - lastCalvingDate).Days;
                }
                else
                {
                    if (model.Cattles[i].CattleGroupId == NotDryGroupID || model.Cattles[i].CattleGroupId == RemoveableGroupID)
                    {
                        model.Cattles[i].Dim = 0;
                    }
                    else
                    {
                        DateTime lastCalvingDate = model.Cattles[i].lastCalvingDate;
                        model.Cattles[i].Dim = (DateTime.Now - lastCalvingDate).Days;
                    }
                }
            }
            if(model.Cattles[i] != null)
            {
                if (model.Cattles[i].ID != 0)
                {
                    mContext.Clear();
                    CattleTbl _Cattle = mContext.Get<CattleTbl>(model.Cattles[i].ID);
                    _Cattle.Dim = model.Cattles[i].Dim;
                    mContext.Update(_Cattle);
                    mContext.Flush();
                }
            }
        }

        private void InitialCattleDim(CattleTbl cattle, ref CattleDetailModel Model, ISession mContext)
        {
            if (cattle.lactationNumber == 0)
            {
                Model.cattle.Dim = 0;
            }
            else
            {
                int NotDryGroupID = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "خشک").Select(x => x.ID).SingleOrDefault<int>();
                int RemoveableGroupID = mContext.QueryOver<CattleGroupTbl>().Where(x => x.name == "حذفی").Select(x => x.ID).SingleOrDefault<int>();
                if (cattle.CattleGroupId == 0)
                {
                    DateTime lastCalvingDate = cattle.lastCalvingDate;
                    Model.cattle.Dim = (DateTime.Now - lastCalvingDate).Days;
                }
                else
                {
                    if (cattle.CattleGroupId == NotDryGroupID || cattle.CattleGroupId == RemoveableGroupID)
                    {
                        Model.cattle.Dim = 0;
                    }
                    else
                    {
                        DateTime lastCalvingDate = cattle.lastCalvingDate;
                        Model.cattle.Dim = (DateTime.Now - lastCalvingDate).Days;
                    }
                }
            }
            if (cattle != null)
            {
                if (cattle.ID != 0)
                {
                    mContext.Clear();
                    CattleTbl _Cattle = mContext.Get<CattleTbl>(cattle.ID);
                    _Cattle.Dim = cattle.Dim;
                    mContext.Update(_Cattle);
                    mContext.Flush();
                }
            }
        }

        [AuthenticateFilter]
        public async Task<PartialViewResult> Detail(int CattleId)
        {
            CattleDetailModel Model = new CattleDetailModel();
            Model = getDetail(CattleId);
            Model.ActiveEditMode = false;
            return PartialView(Model);
        }

        [HttpPost]
        [AuthenticateFilter]
        public async Task<PartialViewResult> MakeEditable(int CattleId, bool ActiveEditMode)
        {
            CattleDetailModel Model = new CattleDetailModel();
            Model = getDetail(CattleId);
            Model.ActiveEditMode = ActiveEditMode;
            return PartialView("CattleDetailTable", Model);
        }

        [AuthenticateFilter]
        public async Task<PartialViewResult> RefreshDetail(int CattleId)
        {
            CattleDetailModel Model = new CattleDetailModel();
            Model = getDetail(CattleId);
            Model.ActiveEditMode = false;
            return PartialView("CattleDetailTable", Model);
        }

        [HttpPost]
        public async Task<PartialViewResult> SaveEditedCattle(String[] AllEditedCattle)
        {
            CattleDetailModel Model = new CattleDetailModel();

            String CattleId = AllEditedCattle[(int)eAllEditedCattle.CattleId];
            String txtCattleName = AllEditedCattle[(int)eAllEditedCattle.txtCattleName];
            String txtCattleSex = AllEditedCattle[(int)eAllEditedCattle.txtCattleSex];
            String txtCattleGenetics_type_num = AllEditedCattle[(int)eAllEditedCattle.txtCattleGenetics_type_num];
            String txtCattleBirthDate = AllEditedCattle[(int)eAllEditedCattle.txtCattleBirthDate];
            String txtCattleMotherID = AllEditedCattle[(int)eAllEditedCattle.txtCattleMotherID];
            String txtCattleLactationNumber = AllEditedCattle[(int)eAllEditedCattle.txtCattleLactationNumber];
            String txtCattleLastCalvingDate = AllEditedCattle[(int)eAllEditedCattle.txtCattleLastCalvingDate];

            txtCattleBirthDate = txtCattleBirthDate.Replace("۰", "0");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۱", "1");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۲", "2");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۳", "3");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۴", "4");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۵", "5");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۶", "6");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۷", "7");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۸", "8");
            txtCattleBirthDate = txtCattleBirthDate.Replace("۹", "9");

            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۰", "0");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۱", "1");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۲", "2");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۳", "3");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۴", "4");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۵", "5");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۶", "6");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۷", "7");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۸", "8");
            txtCattleLastCalvingDate = txtCattleLastCalvingDate.Replace("۹", "9");

            DateTime CattleBirthday = DateTime.Now;
            bool f_CattleBirthday = false;
            if (txtCattleBirthDate != "")
            {
                CattleBirthday = DateHelper.ConvertToGeorginDate(txtCattleBirthDate);
                f_CattleBirthday = true;
            }

            DateTime CattleLastCalvingDate = DateTime.Now;
            bool f_txtCattleLastCalvingDate = false;
            if (txtCattleLastCalvingDate != "")
            {
                CattleLastCalvingDate = DateHelper.ConvertToGeorginDate(txtCattleLastCalvingDate);
                f_txtCattleLastCalvingDate = true;
            }

            ISession mContext = Context.Open();
            CattleTbl _Cattle = mContext.QueryOver<CattleTbl>().Where(x => x.ID == Convert.ToInt32(CattleId)).SingleOrDefault();
            if(_Cattle != null)
            {
                _Cattle.Name = txtCattleName;
                _Cattle.Sex = txtCattleSex;
                _Cattle.Genetics_type_num = txtCattleGenetics_type_num;
                if(f_CattleBirthday)
                {
                    _Cattle.birthDate = CattleBirthday;
                }
                if (f_txtCattleLastCalvingDate)
                {
                    _Cattle.lastCalvingDate = CattleLastCalvingDate;
                }
                _Cattle.MotherID = Convert.ToInt32(txtCattleMotherID);
                _Cattle.lactationNumber = Convert.ToInt32(txtCattleLactationNumber);

                mContext.Update(_Cattle);
                mContext.Flush();
            }
            Context.Close(mContext);

            Model = getDetail(Convert.ToInt32(CattleId));
            Model.ActiveEditMode = false;
            return PartialView("CattleDetailTable", Model);
        }

        public class EditGroupOfCattleView
        {
            public int CattleId { get; set; }
            public int CattleNumber { get; set; }
            public String MAC { get; set; }
            public String Herd { get; set; }
            public int CattleHerd_ID { get; set; }
            public String FreeStall { get; set; }
            public int FreeStallId { get; set; }
            public String Group { get; set; }
            public int CattleGroupId { get; set; }
            public int LactationNumber { get; set; }
            public int Dim { get; set; }
            public String HeatStatus { get; set; }
            public String HealthState { get; set; }
            public String FertilityStatus { get; set; }
            public bool Checked { get; set; }
        }

        public ActionResult EditGroupOfCattle()
        {
            int CurrentFarmId = Helper.Helper.getCurrentFarmId();

            ISession mContext = Context.Open();
            List<CattleGroupTbl> groups = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            ListModel model = new ListModel();
            List<CattleHerds> _CattleHerdsList = mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == CurrentFarmId).List().ToList();
            _CattleHerdsList.AddRange(mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == -1).List().ToList());

            List<CattleGroupTbl> _CattleGroupTbl = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == CurrentFarmId).List().ToList();
            _CattleGroupTbl.AddRange(mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == -1).List().ToList());

            List<FreeStallTbl> _FreeStallTbl = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == CurrentFarmId).List().ToList();

            model.CattleHerdsList = _CattleHerdsList;
            model.CattleGroupTbl = _CattleGroupTbl;
            model.FreeStallTbl = _FreeStallTbl;

            using (SmartCattleContext context = new SmartCattleContext())
            {
                context.Configuration.LazyLoadingEnabled = true;
                mContext = Context.Open();

                IList<CattleTbl> cattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == CurrentFarmId).Take(ItemPerPage).List<CattleTbl>();
                model.CattleGroupList = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == CurrentFarmId).List<CattleGroupTbl>().ToList();
                model.CattleGroupList.AddRange(mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == -1).List<CattleGroupTbl>().ToList());
                model.FreeStallList = mContext.QueryOver<FreeStallTbl>().Where(x => x.FarmID == CurrentFarmId).List<FreeStallTbl>().ToList();

                model.total = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == CurrentFarmId).List().Count;
                model.pages = model.total / ItemPerPage;
                model.Cattles = cattles.ToList<CattleTbl>();
                if (model.total % ItemPerPage != 0)
                {
                    model.pages++;
                }

                model.current = 1;
                model.Groups = groups.ToList();

                ///////////////////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < model.Cattles.Count; i++)
                {
                    InitialCattleDim(ref model, i, mContext);
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////
                Context.Close(mContext);
            }
            return View(model);
        }

        public enum eAllEditedCattle
        {
            CattleId,
            txtCattleName,
            txtCattleSex,
            txtCattleGenetics_type_num,
            txtCattleBirthDate,
            txtCattleMotherID,
            txtCattleLactationNumber,
            txtCattleLastCalvingDate
        }

        public ArithmeticFilerItem stringToEnum(string value)
        {
            switch (value)
            {
                case "Equal": return ArithmeticFilerItem.Equal;
                case "GreateThan": return ArithmeticFilerItem.GreateThan;
                case "GreateThanOrEqualTo": return ArithmeticFilerItem.GreateThanOrEqualTo;
                case "IsNull": return ArithmeticFilerItem.IsNull;
                case "LessThan": return ArithmeticFilerItem.LessThan;
                case "LessThanOrEqualTo": return ArithmeticFilerItem.LessThanOrEqualTo;
                case "NotEqual": return ArithmeticFilerItem.NotEqual;
                case "NotIsNull": return ArithmeticFilerItem.NotIsNull;
                default: return ArithmeticFilerItem.Equal;
            }
        }
    }

    #region enum Types and models

    public class ListModel
    {
        public string value, date, filterName;
        public List<CattleGroupTbl> Groups;
        public List<CattleTbl> Cattles;
        public List<CattleGroupTbl> CattleGroupList { get; set; }
        public List<FreeStallTbl> FreeStallList { get; set; }
        public List<CattleHerds> CattleHerdsList { get; set; }
        public List<CattleGroupTbl> CattleGroupTbl { get; set; }
        public List<FreeStallTbl> FreeStallTbl { get; set; }

        public int pages, current, total, groupId;
        public CattleFilterField? field;
        public FilterType? filterType;
        public HeatState? heatState;
        public HealthState? healthState;
        public FertilityStates? fertilitySate;
    }

    public class CattleDetailModel
    {
        public CattleTbl cattle { get; set; }
        public CattlesScoreTbl CattlesScore { get; set; }
        public CattleMilking CurrentMilking { get; set; }
        public CattleMilking PreviousMilking { get; set; }
        public DateTime LastCalving { get; set; }
        public DateTime LastOpen { get; set; }
        public DateTime LastHeatStatus { get; set; }
        public DateTime LastHormoneTreatment { get; set; }
        public DateTime LastInsemination { get; set; }
        public DateTime LastETFlushed { get; set; }
        public DateTime LastDryOff { get; set; }
        public DateTime LastPregnacyCheck { get; set; }
        public DateTime LastDoNotBreed { get; set; }
        public Sensor Sensor { get; set; } 
        public string groupName { set; get; }
        public String Sex { get; set; }
        public int MotherID { get; set; }
        public int OpenDays { get; set; }
        public bool ActiveEditMode { get; set; }
    }

    public enum CattleFilterField
    {
        AnimalNumber,
        FreeStallNumber,
        Dim,
        LactationNumber,
        FertilityState,
        InHeat,
        Potential,
        HeatSuspicious,
        Sick,
        VerySick,
        NoMovement,
        HealthSuspicious,
        HealthState,
        Eating,
        Rumintation,
        DaysSinceLastBreeding
    }
    public enum FilterType
    {
        Arithmetic,
        Fertility,
        Heat,
        Health
    }
    public enum ArithmeticFilerItem
    {
        Equal,
        NotEqual,
        GreateThan,
        LessThan,
        GreateThanOrEqualTo,
        LessThanOrEqualTo,
        NotIsNull,
        IsNull
    }
    #endregion

    #region hepler calsses
    public class  CattleFilterResult
    {         
        public IEnumerable<CattleTbl> Cattles { set; get; }
        public int Total { set; get; }
    } 

    public class ExpressionRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ExpressionRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
    }

    public static class LambdaExtensions
    {

    }
    #endregion
}
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.CustomFilters;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public class test
        {
            public static int Cattle_Freestall_Id { get; set; }
        }

        private int ItemPerPage = 15;

        public class NotificationsList
        {
            public int ID { get; set; }
            public String uId { get; set; }
            public String Topic { get; set; }
            public String Comment { get; set; }
            public int FarmID { get; set; }
            public String RoleName { get; set; }
            public String CreatedDate { get; set; }
            public String Status { get; set; }
            public String NotificationType { get; set; }
            public int Snooze { get; set; }
            public String TagName { get; set; }
            public String SnoozeMsg { get; set; }
            public String Username { get; set; }

            public int Cattle_Freestall_Id { get; set; }
            public String NotificationGroup { get; set; }
            public String Act { get; set; }
            public String DeactiveAt { get; set; }
            public String ActDate { get; set; }

            public String ActionComment { get; set; }
            public String Color { get; set; }

            public int current;
            public int pages;
            public int total;
        }

        private static class StatusColor
        {
            public static String SensorOn = "darksalmon";
            public static String Deactive = "lavender";
            public static String Acted = "lightgreen";
            public static String NActed = "lightsalmon";
            public static String SensorOff = "wheat";
        }

        public class NotificationStatus
        {
            public static class Act
            {
                public static String Acted = "YES";
                public static String NotActed = "NO";
            }
        }

        [AuthenticateFilter]
        public ActionResult List()
        {
            List<NotificationsList> retNotificationList = new List<NotificationsList>();
            retNotificationList = CollectNotificationList();
            return View(retNotificationList);
        }

        private List<NotificationsList> CollectNotificationList()
        {
            List<NotificationsList> retNotificationList = new List<NotificationsList>();
            ISession mContext = Context.Open();
            List<NotificationsTable> AllNotification = new List<NotificationsTable>();// mContext.QueryOver<NotificationsTable>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.uId).Desc.List<NotificationsTable>();
            List<NotificationsTable> _tmpAllNotification = new List<NotificationsTable>();

            List<int> AllCattles = mContext.QueryOver<NotificationsTable>().SelectList(item => item.SelectGroup(m => m.Cattle_Freestall_Id)).List<int>().ToList<int>();
            foreach (var cattle in AllCattles)
            {
                List<NotificationsTable> CattleNotifications = mContext.QueryOver<NotificationsTable>()
                    .Where(x => x.FarmID == Helper.Helper.getCurrentFarmId())
                    .Where(x => x.Cattle_Freestall_Id == cattle)
                    .OrderBy(x => x.CreatedDate).Desc.List().ToList();

                if (CattleNotifications.Count != 0)
                {
                    _tmpAllNotification.Add(CattleNotifications[0]);
                    AllNotification.Add(CattleNotifications[0]);
                    for (int i = 1; i < CattleNotifications.Count; i++)
                    {
                        if (CattleNotifications[i].Act.Equals(NotificationStatus.Act.Acted))
                        {
                            AllNotification.Add(CattleNotifications[i]);
                        }
                    }
                }
            }

            List<String> AlluID = new List<String>();
            String lastID = "NaN";
            for (int i = 0; i < AllNotification.Count; i++)
            {
                if (!AllNotification[i].uId.Equals(lastID))
                {
                    lastID = AllNotification[i].uId;
                    AlluID.Add(AllNotification[i].uId);
                }
            }
            for (int i = 0; i < AlluID.Count; i++)
            {
                mContext = Context.Open();
                IList<NotificationsTable> LastNotification = mContext.QueryOver<NotificationsTable>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId() && x.uId == AlluID[i]).OrderBy(x => x.ID).Desc.List<NotificationsTable>();
                if (LastNotification.Count != 0)
                {
                    NotificationsList item = new NotificationsList()
                    {
                        ID = LastNotification[0].ID,
                        uId = LastNotification[0].uId,
                        Topic = LastNotification[0].Topic,
                        Comment = LastNotification[0].Comment,
                        FarmID = LastNotification[0].FarmID,
                        RoleName = LastNotification[0].RoleName,
                        CreatedDate = LastNotification[0].CreatedDate.ToString(),
                        Status = LastNotification[0].Status,
                        NotificationType = LastNotification[0].NotificationType,
                        Snooze = LastNotification[0].Snooze,
                        TagName = LastNotification[0].TagName,
                        SnoozeMsg = LastNotification[0].SnoozeMsg,
                        Username = LastNotification[0].Username,
                        Cattle_Freestall_Id = LastNotification[0].Cattle_Freestall_Id,
                        NotificationGroup = LastNotification[0].NotificationGroup,
                        Act = LastNotification[0].Act,
                        DeactiveAt = LastNotification[0].DeactiveAt.ToString(),
                        ActDate = LastNotification[0].ActDate.ToString(),
                        ActionComment = LastNotification[0].ActionComment
                    };
                    if (item.Status == "ACTIVE")
                    {
                        item.Status = "1";
                    }
                    else if (item.Status == "DEACTIVE")
                    {
                        item.Status = "0";
                    }

                    if (item.CreatedDate.Equals("0001-01-01T00:00:00"))
                    {
                        item.CreatedDate = "";
                    }
                    if (item.DeactiveAt.Equals("0001-01-01T00:00:00"))
                    {
                        item.DeactiveAt = "";
                    }
                    if (item.ActDate.Equals("0001-01-01T00:00:00"))
                    {
                        item.ActDate = "";
                    }

                    if (LastNotification[0].Deactive != null)
                    {
                        if (LastNotification[0].Deactive != "")
                        {
                            item.Color = StatusColor.Deactive;
                        }
                        else
                        {
                            //؟
                        }
                    }
                    else if (LastNotification[0].Deactive == null)
                    {
                        if (item.Status == "1")
                        {
                            if (item.Act == "YES")
                            {
                                item.Color = StatusColor.Acted;
                            }
                            else
                            {
                                item.Color = StatusColor.SensorOn;
                            }
                        }
                        else if (item.Status == "0")
                        {
                            item.Color = StatusColor.SensorOff;
                        }
                    }

                    item.total = AlluID.Count;
                    item.pages = item.total / ItemPerPage;
                    if (item.total % ItemPerPage != 0)
                    {
                        item.pages++;
                    }
                    item.current = 1;

                    retNotificationList.Add(item);
                }
                Context.Close(mContext);
            }

            return retNotificationList;
        }

        private List<NotificationsList> CreateNotificationList(IList<NotificationsTable> Notifications)
        {
            List<NotificationsList> NotificationList = new List<NotificationsList>();
            if (Notifications.Count != 0)
            {
                for (int i = 0; i < Notifications.Count; i++)
                {
                    NotificationsList item = new NotificationsList()
                    {
                        ID = Notifications[i].ID,
                        uId = Notifications[i].uId,
                        Topic = Notifications[i].Topic,
                        Comment = Notifications[i].Comment,
                        FarmID = Notifications[i].FarmID,
                        RoleName = Notifications[i].RoleName,
                        CreatedDate = Notifications[i].CreatedDate.ToString(),
                        Status = Notifications[i].Status,
                        NotificationType = Notifications[i].NotificationType,
                        Snooze = Notifications[i].Snooze,
                        TagName = Notifications[i].TagName,
                        SnoozeMsg = Notifications[i].SnoozeMsg,
                        Username = Notifications[i].Username,
                        Cattle_Freestall_Id = Notifications[i].Cattle_Freestall_Id,
                        NotificationGroup = Notifications[i].NotificationGroup,
                        Act = Notifications[i].Act,
                        DeactiveAt = Notifications[i].DeactiveAt.ToString(),
                        ActDate = Notifications[i].ActDate.ToString(),
                        ActionComment = Notifications[i].ActionComment
                    };
                    if (item.Status == "ACTIVE")
                    {
                        item.Status = "1";
                    }
                    else if (item.Status == "DEACTIVE")
                    {
                        item.Status = "0";
                    }

                    if (item.CreatedDate.Equals("0001-01-01T00:00:00"))
                    {
                        item.CreatedDate = "";
                    }
                    if (item.DeactiveAt.Equals("0001-01-01T00:00:00"))
                    {
                        item.DeactiveAt = "";
                    }
                    if (item.ActDate.Equals("0001-01-01T00:00:00"))
                    {
                        item.ActDate = "";
                    }

                    if (Notifications[i].Deactive != null)
                    {
                        if (Notifications[i].Deactive != "")
                        {
                            item.Color = StatusColor.Deactive;
                        }
                        else
                        {
                            //؟
                        }
                    }
                    else if (Notifications[i].Deactive == null)
                    {
                        if (item.Status == "1")
                        {
                            if (item.Act == "YES")
                            {
                                item.Color = StatusColor.Acted;
                            }
                            else
                            {
                                item.Color = StatusColor.SensorOn;
                            }
                        }
                        else if (item.Status == "0")
                        {
                            item.Color = StatusColor.SensorOff;
                        }
                    }

                    item.total = Notifications.Count;
                    item.pages = item.total / ItemPerPage;
                    if (item.total % ItemPerPage != 0)
                    {
                        item.pages++;
                    }
                    item.current = 1;
                    NotificationList.Add(item);
                }
            }
            return NotificationList;
        }

        public String LoadDetail(String ID)
        {
            String ret = "";
            int tmpID = Convert.ToInt32(ID.Replace("RowId", ""));
            List<NotificationDetailModel> _NotificationDetailModel = new List<NotificationDetailModel>();

            ISession mContext = Context.Open();
            IList<NotificationsTable> currentNotification = mContext.QueryOver<NotificationsTable>().Where(x => x.ID == tmpID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<NotificationsTable>();
            Context.Close(mContext);

            if (currentNotification.Count != 0)
            {
                mContext = Context.Open();
                IList<NotificationsTable> AllSubNotification = mContext.QueryOver<NotificationsTable>().Where(x => x.Cattle_Freestall_Id == currentNotification[0].Cattle_Freestall_Id).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List<NotificationsTable>();
                Context.Close(mContext);
                for (int i = 0; i < AllSubNotification.Count; i++)
                {
                    NotificationDetailModel item = new NotificationDetailModel();

                    item.ID = (i + 1).ToString();
                    item.ActComment = AllSubNotification[i].ActionComment;
                    item.Status = AllSubNotification[i].Status;
                    item.Act = AllSubNotification[i].Act;
                    item.CreateDate = AllSubNotification[i].CreatedDate.ToString();
                    if (AllSubNotification[i].DeactiveAt.ToString().Equals("0001-01-01T00:00:00"))
                    {
                        item.DeactiveAt = "";
                    }
                    else
                    {
                        item.DeactiveAt = AllSubNotification[i].DeactiveAt.ToString();
                    }
                    if (AllSubNotification[i].Username != null)
                    {
                        item.Username = AllSubNotification[i].Username.ToString();
                    }
                    else
                    {
                        item.Username = "";
                    }
                    if (AllSubNotification[i].ActDate.ToString().Equals("0001-01-01T00:00:00"))
                    {
                        item.ActDate = "";
                    }
                    else
                    {
                        item.ActDate = AllSubNotification[i].ActDate.ToString();
                    }
                    if(AllSubNotification[i].Status.Equals("ACTIVE"))
                    {
                        AllSubNotification[i].Status = "1";
                    }
                    else
                    {
                        AllSubNotification[i].Status = "0";
                    }
                    _NotificationDetailModel.Add(item);
                }
            }
            ret = JsonConvert.SerializeObject(_NotificationDetailModel);
            return ret;
        }

        public class NotificationSettingViewModel
        {
            public List<RolesList_UserTbl> userRoles { get; set; }
            public List<CattleNotificationsSetting> cattle_notifications { get; set; }
            public List<FreeStallNotificationsSetting> freestall_notifications { get; set; }
        }

        public ActionResult Settings()
        {
            //PRMContext mContext = new PRMContext();
            ISession mContext = Context.Open();
            var userRoles = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
            var cattle_notifications = mContext.QueryOver<CattleNotificationsSetting>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
            var freestall_notifications = mContext.QueryOver<FreeStallNotificationsSetting>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            NotificationSettingViewModel VM = VM = new NotificationSettingViewModel
            {
                userRoles = userRoles,
                cattle_notifications = cattle_notifications,
                freestall_notifications = freestall_notifications
            };

            return View(VM);
        }

        enum CattleNotificationFileds
        {
            CattleTopic,
            CattleMessage,
            CattleGroup,
            CattleRole,
            CattlePeroid,
            CattleWindow,
            cattleTempMIN,
            cattleTempMAX,
            cattleSittingMIN,
            cattleSittingMAX,
            cattleWalkibngMIN,
            cattleWalkibngMAX,
            cattleRuminationMIN,
            cattleRuminationMAX,
            cattleDrinkingMIN,
            cattleDrinkingMAX,
            cattleEattingMIN,
            cattleEattingMAX,
            cattleStandingMIN,
            cattleStandingMAX,
            CattleSnoozeTime
        }

        enum FreeStallNotificationFields
        {
            FreeStallTopic,
            FreeStallMessage,
            FreeStallGroup,
            FreeStallRole,
            FreeStallPeroid,
            FreeStallWindow,
            FreeStallTempMIN,
            FreeStallTempMAX,
            FreeStallHumgMIN,
            FreeStallHumgMAX,
            FreeStallTHIMIN,
            FreeStallTHIMAX,
            FreeStallSnooze
        }

        public String SaveCattleSetting(String[] Settings)
        {
            String CattleTopic = Settings[(int)CattleNotificationFileds.CattleTopic];
            String CattleMessage = Settings[(int)CattleNotificationFileds.CattleMessage];
            String CattleGroup = Settings[(int)CattleNotificationFileds.CattleGroup];
            String CattleRole = Settings[(int)CattleNotificationFileds.CattleRole];
            String CattlePeroid = Settings[(int)CattleNotificationFileds.CattlePeroid].Replace("h", "");
            String CattleWindow = Settings[(int)CattleNotificationFileds.CattleWindow].Replace("h", "");
            String cattleTempMIN = Settings[(int)CattleNotificationFileds.cattleTempMIN];
            String cattleTempMAX = Settings[(int)CattleNotificationFileds.cattleTempMAX];
            String cattleSittingMIN = Settings[(int)CattleNotificationFileds.cattleSittingMIN];
            String cattleSittingMAX = Settings[(int)CattleNotificationFileds.cattleSittingMAX];
            String cattleWalkibngMIN = Settings[(int)CattleNotificationFileds.cattleWalkibngMIN];
            String cattleWalkibngMAX = Settings[(int)CattleNotificationFileds.cattleWalkibngMAX];
            String cattleRuminationMIN = Settings[(int)CattleNotificationFileds.cattleRuminationMIN];
            String cattleRuminationMAX = Settings[(int)CattleNotificationFileds.cattleRuminationMAX];
            String cattleDrinkingMIN = Settings[(int)CattleNotificationFileds.cattleDrinkingMIN];
            String cattleDrinkingMAX = Settings[(int)CattleNotificationFileds.cattleDrinkingMAX];
            String cattleEattingMIN = Settings[(int)CattleNotificationFileds.cattleEattingMIN];
            String cattleEattingMAX = Settings[(int)CattleNotificationFileds.cattleEattingMAX];
            String cattleStandingMIN = Settings[(int)CattleNotificationFileds.cattleStandingMIN];
            String cattleStandingMAX = Settings[(int)CattleNotificationFileds.cattleStandingMAX];
            String CattleSnoozeTime = Settings[(int)CattleNotificationFileds.CattleSnoozeTime];

            ISession mContext = Context.Open();
            CattleNotificationsSetting _CattleNotificationsSetting = new CattleNotificationsSetting()
            {
                FarmId = Helper.Helper.getCurrentFarmId(),
                Topic = CattleTopic,
                Comment = CattleMessage,
                GroupName = CattleGroup,
                Roles = CattleRole,
                PeroidTime = Convert.ToInt16(CattlePeroid),
                WindowTime = Convert.ToInt16(CattleWindow),
                CattleTempMin = Convert.ToDouble(cattleTempMIN),
                CattleTempMax = Convert.ToDouble(cattleTempMAX),
                SittingMin = Convert.ToDouble(cattleSittingMIN),
                SittingMax = Convert.ToDouble(cattleSittingMAX),
                WalkingMin = Convert.ToDouble(cattleWalkibngMIN),
                WalkingMax = Convert.ToDouble(cattleWalkibngMAX),
                RuminationMin = Convert.ToDouble(cattleRuminationMIN),
                RuminationMax = Convert.ToDouble(cattleRuminationMAX),
                DrinkingMin = Convert.ToDouble(cattleDrinkingMIN),
                DrinkingMax = Convert.ToDouble(cattleDrinkingMAX),
                EatingMin = Convert.ToDouble(cattleEattingMIN),
                EatingMax = Convert.ToDouble(cattleEattingMAX),
                StandingMin = Convert.ToDouble(cattleStandingMIN),
                StandingMax = Convert.ToDouble(cattleStandingMAX),
                CreateDate = DateTime.Now,
                ActivationState = "Active",
                SnoozeTime = CattleSnoozeTime
            };
            mContext.Save(_CattleNotificationsSetting);
            Context.Close(mContext);
                
            return "OK";
        }

        public String SaveFreeStallSetting(String[] Settings)
        {
            String FreeStallTopic = Settings[(int)FreeStallNotificationFields.FreeStallTopic];
            String FreeStallMessage = Settings[(int)FreeStallNotificationFields.FreeStallMessage];
            String FreeStallGroup = Settings[(int)FreeStallNotificationFields.FreeStallGroup];
            String FreeStallRole = Settings[(int)FreeStallNotificationFields.FreeStallRole];
            String FreeStallPeroid = Settings[(int)FreeStallNotificationFields.FreeStallPeroid].Replace("h", "");
            String FreeStallWindow = Settings[(int)FreeStallNotificationFields.FreeStallWindow].Replace("h", "");
            String FreeStallTempMIN = Settings[(int)FreeStallNotificationFields.FreeStallTempMIN];
            String FreeStallTempMAX = Settings[(int)FreeStallNotificationFields.FreeStallTempMAX];
            String FreeStallHumgMIN = Settings[(int)FreeStallNotificationFields.FreeStallHumgMIN];
            String FreeStallHumgMAX = Settings[(int)FreeStallNotificationFields.FreeStallHumgMAX];
            String FreeStallTHIMIN = Settings[(int)FreeStallNotificationFields.FreeStallTHIMIN];
            String FreeStallTHIMAX = Settings[(int)FreeStallNotificationFields.FreeStallTHIMAX];
            String FreeStallSnooze = Settings[(int)FreeStallNotificationFields.FreeStallSnooze];

            ISession mContext = Context.Open();
            FreeStallNotificationsSetting _FreeStallNotificationsSetting = new FreeStallNotificationsSetting()
            {
                FarmId = Helper.Helper.getCurrentFarmId(),
                Topic = FreeStallTopic,
                Comment = FreeStallMessage,
                GroupName = FreeStallGroup,
                Roles = FreeStallRole,
                PeroidTime = Convert.ToInt16(FreeStallPeroid),
                WindowTime = Convert.ToInt16(FreeStallWindow),
                TempMin = Convert.ToDouble(FreeStallTempMIN),
                TempMax = Convert.ToDouble(FreeStallTempMAX),
                HumMin = Convert.ToDouble(FreeStallHumgMIN),
                HumMax = Convert.ToDouble(FreeStallHumgMAX),
                THIMin = Convert.ToDouble(FreeStallTHIMIN),
                THIMax = Convert.ToDouble(FreeStallTHIMAX),
                CreateDate = DateTime.Now,
                ActivationState = "Active",
                SnoozeTime = FreeStallSnooze
            };
            mContext.Save(_FreeStallNotificationsSetting);
            Context.Close(mContext);

            return "OK";
        }

        public ActionResult UpdateCattleSetting(String ID)
        {
            NotificationSettingViewModel retValue = new NotificationSettingViewModel();
            if (ID != null)
            {
                ISession mContext = Context.Open();
                long tmpID = Convert.ToInt32(ID);
                retValue.cattle_notifications = mContext.QueryOver<CattleNotificationsSetting>().Where(x => x.ID == tmpID).Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
                retValue.freestall_notifications = new List<FreeStallNotificationsSetting>();
                retValue.userRoles = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
                Context.Close(mContext);
                return View(retValue);

            }
            else
            {
                return RedirectToAction("Settings");
            }
        }

        public String UpdateCattleNotificationSetting(String[] Settings, String ID)
        {
            int tmpID = Convert.ToInt32(ID);
            String CattleTopic = Settings[(int)CattleNotificationFileds.CattleTopic];
            String CattleMessage = Settings[(int)CattleNotificationFileds.CattleMessage];
            String CattleGroup = Settings[(int)CattleNotificationFileds.CattleGroup];
            String CattleRole = Settings[(int)CattleNotificationFileds.CattleRole];
            String CattlePeroid = Settings[(int)CattleNotificationFileds.CattlePeroid].Replace("h", "");
            String CattleWindow = Settings[(int)CattleNotificationFileds.CattleWindow].Replace("h", "");
            String cattleTempMIN = Settings[(int)CattleNotificationFileds.cattleTempMIN];
            String cattleTempMAX = Settings[(int)CattleNotificationFileds.cattleTempMAX];
            String cattleSittingMIN = Settings[(int)CattleNotificationFileds.cattleSittingMIN];
            String cattleSittingMAX = Settings[(int)CattleNotificationFileds.cattleSittingMAX];
            String cattleWalkibngMIN = Settings[(int)CattleNotificationFileds.cattleWalkibngMIN];
            String cattleWalkibngMAX = Settings[(int)CattleNotificationFileds.cattleWalkibngMAX];
            String cattleRuminationMIN = Settings[(int)CattleNotificationFileds.cattleRuminationMIN];
            String cattleRuminationMAX = Settings[(int)CattleNotificationFileds.cattleRuminationMAX];
            String cattleDrinkingMIN = Settings[(int)CattleNotificationFileds.cattleDrinkingMIN];
            String cattleDrinkingMAX = Settings[(int)CattleNotificationFileds.cattleDrinkingMAX];
            String cattleEattingMIN = Settings[(int)CattleNotificationFileds.cattleEattingMIN];
            String cattleEattingMAX = Settings[(int)CattleNotificationFileds.cattleEattingMAX];
            String cattleStandingMIN = Settings[(int)CattleNotificationFileds.cattleStandingMIN];
            String cattleStandingMAX = Settings[(int)CattleNotificationFileds.cattleStandingMAX];
            String CattleSnoozeTime = Settings[(int)CattleNotificationFileds.CattleSnoozeTime];

            ISession mContext = Context.Open();
            mContext.Clear();

            CattleNotificationsSetting _CattleNotificationSetting = mContext.Get<CattleNotificationsSetting>(tmpID);
            _CattleNotificationSetting.FarmId = Helper.Helper.getCurrentFarmId();
            _CattleNotificationSetting.Topic = CattleTopic;
            _CattleNotificationSetting.Comment = CattleMessage;
            _CattleNotificationSetting.GroupName = CattleGroup;
            _CattleNotificationSetting.Roles = CattleRole;
            _CattleNotificationSetting.PeroidTime = Convert.ToInt16(CattlePeroid);
            _CattleNotificationSetting.WindowTime = Convert.ToInt16(CattleWindow);
            _CattleNotificationSetting.CattleTempMin = Convert.ToDouble(cattleTempMIN);
            _CattleNotificationSetting.CattleTempMax = Convert.ToDouble(cattleTempMAX);
            _CattleNotificationSetting.SittingMin = Convert.ToDouble(cattleSittingMIN);
            _CattleNotificationSetting.SittingMax = Convert.ToDouble(cattleSittingMAX);
            _CattleNotificationSetting.WalkingMin = Convert.ToDouble(cattleWalkibngMIN);
            _CattleNotificationSetting.WalkingMax = Convert.ToDouble(cattleWalkibngMAX);
            _CattleNotificationSetting.RuminationMin = Convert.ToDouble(cattleRuminationMIN);
            _CattleNotificationSetting.RuminationMax = Convert.ToDouble(cattleRuminationMAX);
            _CattleNotificationSetting.DrinkingMin = Convert.ToDouble(cattleDrinkingMIN);
            _CattleNotificationSetting.DrinkingMax = Convert.ToDouble(cattleDrinkingMAX);
            _CattleNotificationSetting.EatingMin = Convert.ToDouble(cattleEattingMIN);
            _CattleNotificationSetting.EatingMax = Convert.ToDouble(cattleEattingMAX);
            _CattleNotificationSetting.StandingMin = Convert.ToDouble(cattleStandingMIN);
            _CattleNotificationSetting.StandingMax = Convert.ToDouble(cattleStandingMAX);
            _CattleNotificationSetting.ActivationState = "Active";
            _CattleNotificationSetting.SnoozeTime = CattleSnoozeTime;
            mContext.Update(_CattleNotificationSetting);
            mContext.Flush();
            Context.Close(mContext);
            
            return "OK";
        }

        public ActionResult UpdateFreeStallSetting(String ID)
        {
            NotificationSettingViewModel retValue = new NotificationSettingViewModel();
            if (ID != null)
            {
                ISession mContext = Context.Open();
                long tmpID = Convert.ToInt32(ID);
                retValue.cattle_notifications = new List<CattleNotificationsSetting>();
                retValue.freestall_notifications = mContext.QueryOver<FreeStallNotificationsSetting>().Where(x => x.ID == tmpID).List().ToList();
                retValue.userRoles = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
                Context.Close(mContext);
                return View(retValue);
            }
            else
            {
                return RedirectToAction("Settings");
            }
            
        }

        public String UpdateFreeStallNotificationSetting(String[] Settings, String ID)
        {
            int tmpID = Convert.ToInt32(ID);
            String FreeStallTopic = Settings[(int)FreeStallNotificationFields.FreeStallTopic];
            String FreeStallMessage = Settings[(int)FreeStallNotificationFields.FreeStallMessage];
            String FreeStallGroup = Settings[(int)FreeStallNotificationFields.FreeStallGroup];
            String FreeStallRole = Settings[(int)FreeStallNotificationFields.FreeStallRole];
            String FreeStallPeroid = Settings[(int)FreeStallNotificationFields.FreeStallPeroid].Replace("h", "");
            String FreeStallWindow = Settings[(int)FreeStallNotificationFields.FreeStallWindow].Replace("h", "");
            String FreeStallTempMIN = Settings[(int)FreeStallNotificationFields.FreeStallTempMIN];
            String FreeStallTempMAX = Settings[(int)FreeStallNotificationFields.FreeStallTempMAX];
            String FreeStallHumgMIN = Settings[(int)FreeStallNotificationFields.FreeStallHumgMIN];
            String FreeStallHumgMAX = Settings[(int)FreeStallNotificationFields.FreeStallHumgMAX];
            String FreeStallTHIMIN = Settings[(int)FreeStallNotificationFields.FreeStallTHIMIN];
            String FreeStallTHIMAX = Settings[(int)FreeStallNotificationFields.FreeStallTHIMAX];
            String FreeStallSnooze = Settings[(int)FreeStallNotificationFields.FreeStallSnooze];

            ISession mContext = Context.Open();
            mContext.Clear();

            FreeStallNotificationsSetting _FreeStallNotificationsSetting = mContext.Get<FreeStallNotificationsSetting>(tmpID);
            _FreeStallNotificationsSetting.FarmId = Helper.Helper.getCurrentFarmId();
            _FreeStallNotificationsSetting.Topic = FreeStallTopic;
            _FreeStallNotificationsSetting.Comment = FreeStallMessage;
            _FreeStallNotificationsSetting.GroupName = FreeStallGroup;
            _FreeStallNotificationsSetting.Roles = FreeStallRole;
            _FreeStallNotificationsSetting.PeroidTime = Convert.ToInt16(FreeStallPeroid);
            _FreeStallNotificationsSetting.WindowTime = Convert.ToInt16(FreeStallWindow);
            _FreeStallNotificationsSetting.TempMin = Convert.ToDouble(FreeStallTempMIN);
            _FreeStallNotificationsSetting.TempMax = Convert.ToDouble(FreeStallTempMAX);
            _FreeStallNotificationsSetting.HumMin = Convert.ToDouble(FreeStallHumgMIN);
            _FreeStallNotificationsSetting.HumMax = Convert.ToDouble(FreeStallHumgMAX);
            _FreeStallNotificationsSetting.THIMin = Convert.ToDouble(FreeStallTHIMIN);
            _FreeStallNotificationsSetting.THIMax = Convert.ToDouble(FreeStallTHIMAX);
            _FreeStallNotificationsSetting.ActivationState = "Active";
            _FreeStallNotificationsSetting.SnoozeTime = FreeStallSnooze;

            mContext.Update(_FreeStallNotificationsSetting);
            mContext.Flush();
            Context.Close(mContext);
                
            return "OK";
        }

        public String RemoveCattleSetting(String ID)
        {
            ISession mContext = Context.Open();

            long tmpID = Convert.ToInt32(ID);
            String deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleNotificationsSetting", tmpID);
            mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();
            Context.Close(mContext);

            return "OK";
        }

        public String RemoveFreestallSetting(String ID)
        {
            long tmpID = Convert.ToInt32(ID);
            ISession mContext = Context.Open();
            String deleteAll = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.FreeStallNotificationsSetting", tmpID);
            mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();
            Context.Close(mContext);
            return "OK";
        }

        public String ChangeCattleNotiActivation(String ID, String Status)
        {
            ISession mContext = Context.Open();

            int tmpID = Convert.ToInt16(ID);
            bool tmpStatus = Convert.ToBoolean(Status);
            if (tmpStatus)
            {
                CattleNotificationsSetting _FreeStallNotificationsSetting = mContext.Get<CattleNotificationsSetting>(tmpID);
                _FreeStallNotificationsSetting.ActivationState = "Active";
                _FreeStallNotificationsSetting.CreateDate = DateTime.Now;
                mContext.Update(_FreeStallNotificationsSetting);
                mContext.Flush();
            }
            else
            {
                CattleNotificationsSetting _FreeStallNotificationsSetting = mContext.Get<CattleNotificationsSetting>(tmpID);
                _FreeStallNotificationsSetting.ActivationState = "Deactive";
                _FreeStallNotificationsSetting.CreateDate = DateTime.Now;
                mContext.Update(_FreeStallNotificationsSetting);
                mContext.Flush();
            }
            Context.Close(mContext);

            return "OK";
        }

        public String ChangeFreeStallNotiActivation(String ID, String Status)
        {
            ISession mContext = Context.Open();
            mContext.Clear();
            int tmpID = Convert.ToInt16(ID);
            bool tmpStatus = Convert.ToBoolean(Status);
            if (tmpStatus)
            {
                FreeStallNotificationsSetting _FreeStallNotificationsSetting = mContext.Get<FreeStallNotificationsSetting>(tmpID);
                _FreeStallNotificationsSetting.ActivationState = "Active";
                _FreeStallNotificationsSetting.CreateDate = DateTime.Now;
                mContext.Update(_FreeStallNotificationsSetting);
                mContext.Flush();
            }
            else
            {
                FreeStallNotificationsSetting _FreeStallNotificationsSetting = mContext.Get<FreeStallNotificationsSetting>(tmpID);
                _FreeStallNotificationsSetting.ActivationState = "Deactive";
                _FreeStallNotificationsSetting.CreateDate = DateTime.Now;
                mContext.Update(_FreeStallNotificationsSetting);
                mContext.Flush();
            }
            Context.Close(mContext);

            return "OK";
        }

        [AuthenticateFilter]
        public String SaveAction(String ID, String Msg)
        {
            String ret = "NaN";
            int tmpID = Convert.ToInt32(ID.Replace("RowId", ""));
            int CurrentUserId = Helper.Helper.getCurrentUserId();
            bool isStaff = Helper.Helper.isStaff();
            if(!isStaff)
            {
                ISession mContext = Context.Open();
                IList<UserInfo> currentUser = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).List<UserInfo>();
                Context.Close(mContext);

                if (currentUser.Count != 0)
                {
                    mContext = Context.Open();
                    NotificationsTable currentNotification = mContext.Load<NotificationsTable>(tmpID);

                    if (currentNotification != null)
                    {
                        try
                        {
                            NotificationsTable newNotification = new NotificationsTable();
                            String tmpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));

                            newNotification.Topic = currentNotification.Topic;
                            newNotification.Comment = currentNotification.Comment;
                            newNotification.FarmID = currentNotification.FarmID;
                            newNotification.RoleName = currentNotification.RoleName;
                            newNotification.CreatedDate = DateTime.Now;
                            newNotification.Status = currentNotification.Status;
                            newNotification.NotificationType = currentNotification.NotificationType;
                            newNotification.TagName = currentNotification.TagName;
                            newNotification.Cattle_Freestall_Id = currentNotification.Cattle_Freestall_Id;
                            newNotification.uId = currentNotification.uId;
                            newNotification.NotificationGroup = currentNotification.NotificationGroup;
                            //newNotification.DeactiveAt = nulldate;
                            newNotification.Username = currentUser[0].Name + " " + currentUser[0].Family;
                            newNotification.Act = "YES";
                            newNotification.ActDate = DateTime.Now;
                            newNotification.ActionComment = Msg;
                            mContext.Save(newNotification);
                            mContext.Flush();
                            ret = "OK";
                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));
                        }
                        catch (Exception ex)
                        {
                            ret = ex.Message;
                        }
                    }
                    Context.Close(mContext);
                }
            }
            else
            {
                ret = "STAFF";
            }
            
            return ret;
        }

        [AuthenticateFilter]
        public String DeactiveAction(String ID)
        {
            String ret = "NaN";
            int tmpID = Convert.ToInt32(ID.Replace("RowId", ""));
            int CurrentUserId = Helper.Helper.getCurrentUserId();
            bool isStaff = Helper.Helper.isStaff();
            if (!isStaff)
            {
                ISession mContext = Context.Open();
                IList<UserInfo> currentUser = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).List<UserInfo>();

                if (currentUser.Count != 0)
                {
                    NotificationsTable currentNotification = mContext.Load<NotificationsTable>(tmpID);

                    if (currentNotification != null)
                    {
                        try
                        {
                            NotificationsTable newNotification = new NotificationsTable();
                            String tmpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));

                            newNotification.Topic = currentNotification.Topic;
                            newNotification.Comment = currentNotification.Comment;
                            newNotification.FarmID = currentNotification.FarmID;
                            newNotification.RoleName = currentNotification.RoleName;
                            newNotification.CreatedDate = currentNotification.CreatedDate;
                            newNotification.Status = "";
                            newNotification.uId = currentNotification.uId;
                            newNotification.NotificationType = currentNotification.NotificationType;
                            newNotification.TagName = currentNotification.TagName;
                            newNotification.Cattle_Freestall_Id = currentNotification.Cattle_Freestall_Id;
                            newNotification.NotificationGroup = currentNotification.NotificationGroup;
                            newNotification.Deactive = "DEACTIVE";
                            newNotification.DeactiveAt = DateTime.Now;
                            newNotification.Username = currentUser[0].Name + " " + currentUser[0].Family;
                            newNotification.Act = "";

                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));

                            newNotification.ActionComment = Localization.getString("This_msg_created_by_system_deactive");

                            try
                            {
                                mContext.Save(newNotification);
                                mContext.Flush();
                                ret = "OK";
                            }
                            catch (Exception ex)
                            {
                                ret = ex.Message;
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            ret = ex.Message;
                        }
                    }
                }
                Context.Close(mContext);
            }
            else
            {
                ret = "STAFF";
            }

            return ret;
        }

        [AuthenticateFilter]
        public String ActiveAgainAction(String ID)
        {
            String ret = "NaN";
            int tmpID = Convert.ToInt32(ID.Replace("RowId", ""));
            int CurrentUserId = Helper.Helper.getCurrentUserId();
            bool isStaff = Helper.Helper.isStaff();
            if (!isStaff)
            {
                ISession mContext = Context.Open();
                IList<UserInfo> currentUser = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).List<UserInfo>();

                if (currentUser.Count != 0)
                {
                    NotificationsTable currentNotification = mContext.Load<NotificationsTable>(tmpID);
                    if (currentNotification != null)
                    {
                        try
                        {
                            NotificationsTable newNotification = new NotificationsTable();
                            String tmpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "en", "us"));

                            newNotification.Topic = currentNotification.Topic;
                            newNotification.Comment = currentNotification.Comment;
                            newNotification.FarmID = currentNotification.FarmID;
                            newNotification.RoleName = currentNotification.RoleName;
                            newNotification.CreatedDate = currentNotification.CreatedDate;
                            newNotification.Status = "ACTIVE";
                            newNotification.uId = currentNotification.uId;
                            newNotification.NotificationType = currentNotification.NotificationType;
                            newNotification.TagName = currentNotification.TagName;
                            newNotification.Cattle_Freestall_Id = currentNotification.Cattle_Freestall_Id;
                            newNotification.NotificationGroup = currentNotification.NotificationGroup;
                            newNotification.Username = currentUser[0].Name + " " + currentUser[0].Family;
                            newNotification.Act = "";
                            newNotification.ActDate = DateTime.Now;
                            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "fa", "IR"));
                            newNotification.ActionComment = Localization.getString("This_msg_created_by_system_alarm_active_again");

                            mContext.Save(newNotification);
                            mContext.Flush();
                            ret = "OK";
                        }
                        catch (Exception ex)
                        {
                            ret = ex.Message;
                        }
                    }
                    Context.Close(mContext);
                }
            }
            else
            {
                ret = "STAFF";
            }

            return ret;
        }

        public String CheckDeactive(String ID)
        {
            String ret = "NaN";
            int tmpID = Convert.ToInt32(ID.Replace("RowId", ""));

            ISession mContext = Context.Open();
            NotificationsTable _notification = mContext.QueryOver<NotificationsTable>().Where(x => x.ID == tmpID).SingleOrDefault<NotificationsTable>();
            Context.Close(mContext);

            if (_notification != null)
            {
                if(_notification.Deactive != null)
                {
                    ret = "DEACTIVE";
                }
                else if (_notification.Deactive == "")
                {
                    ret = "DEACTIVE";
                }
                else
                {
                    ret = "SUSPPEND";
                }
            }
            else
            {
                ret = "NULL";
            }

            return ret;
        }

        public String SnoozeMessage(String ID)
        {
            String retValue = "NaN";
            int intID = Convert.ToInt32(ID);
            ISession mContext = Context.Open();
            IList<NotificationsTable> NotificatinList = mContext.QueryOver<NotificationsTable>().Where(x => x.ID == intID).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List();
            Context.Close(mContext);
            if(NotificatinList.Count != 0)
            {
                if(NotificatinList[0].Status == "DEACTIVE")
                {

                }
                else
                {
                    String tmpRetValue = NotificatinList[0].SnoozeMsg;
                    if (tmpRetValue == null)
                    {

                    }
                    else if (tmpRetValue == "")
                    {

                    }
                    else
                    {
                        retValue = tmpRetValue;
                    }
                }
            }
            return retValue;
        }

        private class NotificationDetailModel
        {
            public String ID { get; set; }
            public String ActComment { get; set; }
            public String Status { get; set; }
            public String Act { get; set; }
            public String CreateDate { get; set; }
            public String DeactiveAt { get; set; }
            public String Username { get; set; }
            public String ActDate { get; set; }
        }

        private enum AllFieldsname
        {
            NotificationID_Input,
            NotificationCattleFreestallID_Input,
            NotificationType_Input,
            NotificationTopic_Input,
            NotificationMessage_Input,
            NotificationEventDate_Input,
            NotificationState_Input,
            NotificationAct_Input,
            NotificationUsername_Input,
            NotificationActComment_Input,
            NotificationActDate_Input,
            NotificationDeactiveDate_Input,

            NotificationID_Label,
            NotificationCattleFreestallID_Label,
            NotificationType_Label,
            NotificationTopic_Label,
            NotificationMessage_Label,
            NotificationEventDate_Label,
            NotificationState_Label,
            NotificationAct_Label,
            NotificationUsername_Label,
            NotificationActComment_Label,
            NotificationActDate_Label,
            NotificationDeactiveDate_Label
        }

        public async Task<PartialViewResult> Advancefilter(String[] AllFields)
        {
            List<NotificationsList> _NotificationList = new List<NotificationsList>();
            ISession mContext = null;
            try
            {
                mContext = Context.Open();
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
            }
            IQueryOver<NotificationsTable, NotificationsTable> query = mContext.QueryOver<NotificationsTable>();
            bool hasExp = false;

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationID_Label]))
                {
                    int ID = 0;
                    switch (AllFields[(int)AllFieldsname.NotificationID_Label])
                    {
                        case "Equal":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID == ID);
                            break;

                        case "برابر":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID == ID);
                            break;

                        case "GreateThan":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID > ID);
                            break;

                        case "بزرگتر":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID > ID);
                            break;

                        case "GreateThanOrEqualTo":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID >= ID);
                            break;

                        case "بزرگتر یا مساوی":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID >= ID);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID < ID);
                            break;

                        case "کوچکتر":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID < ID);
                            break;

                        case "LessThanOrEqualTo":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID <= ID);
                            break;

                        case "کوچکتر یا مساوی":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID <= ID);
                            break;

                        case "NotEqual":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID != ID);
                            break;

                        case "نابرابر":
                            ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                            query = query.Where(x => x.ID != ID);
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
                    int ID = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationID_Input]);
                    query = query.Where(x => x.ID == ID);
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Label]))
                {
                    int Cattle_Freestall_Id = 0;
                    switch (AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Label])
                    {
                        case "Equal":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id == Cattle_Freestall_Id);
                            break;

                        case "برابر":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id == Cattle_Freestall_Id);
                            break;

                        case "GreateThan":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id > Cattle_Freestall_Id);
                            break;

                        case "بزرگتر":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id > Cattle_Freestall_Id);
                            break;

                        case "GreateThanOrEqualTo":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id >= Cattle_Freestall_Id);
                            break;

                        case "بزرگتر یا مساوی":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id >= Cattle_Freestall_Id);
                            break;

                        case "IsNull":
                            break;

                        case "تهی":
                            break;

                        case "LessThan":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id < Cattle_Freestall_Id);
                            break;

                        case "کوچکتر":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id < Cattle_Freestall_Id);
                            break;

                        case "LessThanOrEqualTo":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id <= Cattle_Freestall_Id);
                            break;

                        case "کوچکتر یا مساوی":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id <= Cattle_Freestall_Id);
                            break;

                        case "NotEqual":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id != Cattle_Freestall_Id);
                            break;

                        case "نابرابر":
                            Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                            query = query.Where(x => x.Cattle_Freestall_Id != Cattle_Freestall_Id);
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
                    int Cattle_Freestall_Id = Convert.ToInt32(AllFields[(int)AllFieldsname.NotificationCattleFreestallID_Input]);
                    query = query.Where(x => x.Cattle_Freestall_Id == Cattle_Freestall_Id);
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationType_Label]))
            {
                switch (AllFields[(int)AllFieldsname.NotificationType_Label])
                {
                    case "CATTLE":
                        query = query.Where(x => x.NotificationType == "CATTLE");
                        break;

                    case "دام":
                        query = query.Where(x => x.NotificationType == "CATTLE");
                        break;

                    case "FREESTALL":
                        query = query.Where(x => x.NotificationType == "FREESTALL");
                        break;

                    case "بهاربند":
                        query = query.Where(x => x.NotificationType == "FREESTALL");
                        break;
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationTopic_Input]))
            {
                try
                {
                    String Topic = Convert.ToString(AllFields[(int)AllFieldsname.NotificationTopic_Input]);
                    query = query.Where(Restrictions.On<NotificationsTable>(x => x.Topic).IsLike("%" + Topic + "%"));
                }
                catch (Exception ex)
                {
                    String Ack = ex.Message;
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationMessage_Input]))
            {
                try
                {
                    String Comment = Convert.ToString(AllFields[(int)AllFieldsname.NotificationMessage_Input]);
                    query = query.Where(Restrictions.On<NotificationsTable>(x => x.Comment).IsLike("%" + Comment + "%"));
                }
                catch (Exception ex)
                {
                    String Ack = ex.Message;
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationEventDate_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationEventDate_Label]))
                {
                    DateTime CreatedDate = DateTime.Now;
                    switch (AllFields[(int)AllFieldsname.NotificationEventDate_Label])
                    {
                        case "Equal":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate == CreatedDate);
                            break;

                        case "برابر":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate == CreatedDate);
                            break;

                        case "GreateThan":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate > CreatedDate);
                            break;

                        case "بزرگتر":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate > CreatedDate);
                            break;

                        case "GreateThanOrEqualTo":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate >= CreatedDate);
                            break;

                        case "بزرگتر یا مساوی":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate >= CreatedDate);
                            break;

                        case "LessThan":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate < CreatedDate);
                            break;

                        case "کوچکتر":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate < CreatedDate);
                            break;

                        case "LessThanOrEqualTo":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate <= CreatedDate);
                            break;

                        case "کوچکتر یا مساوی":
                            CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                            query = query.Where(x => x.CreatedDate <= CreatedDate);
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    DateTime CreatedDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationEventDate_Input]);
                    query = query.Where(x => x.CreatedDate == CreatedDate);
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationState_Label]))
            {
                switch (AllFields[(int)AllFieldsname.NotificationState_Label])
                {
                    case "1":
                        query = query.Where(x => x.Status == "ACTIVE");
                        break;

                    case "0":
                        query = query.Where(x => x.Status == "DEACTIVE");
                        break;
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationAct_Label]))
            {
                switch (AllFields[(int)AllFieldsname.NotificationAct_Label])
                {
                    case "YES":
                        query = query.Where(x => x.Act == "YES");
                        break;

                    case "NO":
                        query = query.Where(x => x.Act == "NO");
                        break;
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationUsername_Input]))
            {
                try
                {
                    String Username = Convert.ToString(AllFields[(int)AllFieldsname.NotificationUsername_Input]);
                    query = query.Where(Restrictions.On<NotificationsTable>(x => x.Username).IsLike("%" + Username + "%"));
                }
                catch (Exception ex)
                {
                    String Ack = ex.Message;
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationActComment_Input]))
            {
                try
                {
                    String ActionComment = Convert.ToString(AllFields[(int)AllFieldsname.NotificationUsername_Input]);
                    query = query.Where(Restrictions.On<NotificationsTable>(x => x.ActionComment).IsLike("%" + ActionComment + "%"));
                }
                catch (Exception ex)
                {
                    String Ack = ex.Message;
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationActDate_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationActDate_Label]))
                {
                    DateTime ActDate = DateTime.Now;
                    switch (AllFields[(int)AllFieldsname.NotificationActDate_Label])
                    {
                        case "Equal":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate == ActDate);
                            break;

                        case "برابر":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate == ActDate);
                            break;

                        case "GreateThan":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate > ActDate);
                            break;

                        case "بزرگتر":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate > ActDate);
                            break;

                        case "GreateThanOrEqualTo":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate >= ActDate);
                            break;

                        case "بزرگتر یا مساوی":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate >= ActDate);
                            break;

                        case "LessThan":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate < ActDate);
                            break;

                        case "کوچکتر":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate < ActDate);
                            break;

                        case "LessThanOrEqualTo":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate <= ActDate);
                            break;

                        case "کوچکتر یا مساوی":
                            ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                            query = query.Where(x => x.ActDate <= ActDate);
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    DateTime ActDate = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationActDate_Input]);
                    query = query.Where(x => x.ActDate == ActDate);
                }
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]))
            {
                if (!string.IsNullOrEmpty(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Label]))
                {
                    DateTime DeactiveAt = DateTime.Now;
                    switch (AllFields[(int)AllFieldsname.NotificationDeactiveDate_Label])
                    {
                        case "Equal":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt == DeactiveAt);
                            break;

                        case "برابر":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt == DeactiveAt);
                            break;

                        case "GreateThan":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt > DeactiveAt);
                            break;

                        case "بزرگتر":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt > DeactiveAt);
                            break;

                        case "GreateThanOrEqualTo":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt >= DeactiveAt);
                            break;

                        case "بزرگتر یا مساوی":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt >= DeactiveAt);
                            break;

                        case "LessThan":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt < DeactiveAt);
                            break;

                        case "کوچکتر":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt < DeactiveAt);
                            break;

                        case "LessThanOrEqualTo":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt <= DeactiveAt);
                            break;

                        case "کوچکتر یا مساوی":
                            DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                            query = query.Where(x => x.DeactiveAt <= DeactiveAt);
                            break;
                    }
                    hasExp = true;
                }
                else
                {
                    DateTime DeactiveAt = Convert.ToDateTime(AllFields[(int)AllFieldsname.NotificationDeactiveDate_Input]);
                    query = query.Where(x => x.DeactiveAt == DeactiveAt);
                }
                hasExp = true;
            }

            if (hasExp)
            {
                IList<NotificationsTable> NotificationList_UNREAD = null;
                try
                {
                    NotificationList_UNREAD = query.Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.CreatedDate).Desc.List<NotificationsTable>();
                    _NotificationList = CreateNotificationList(NotificationList_UNREAD);
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
            }
            else
            {
                IList<NotificationsTable> NotificationList_UNREAD = null;
                NotificationList_UNREAD = mContext.QueryOver<NotificationsTable>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.CreatedDate).Desc.List<NotificationsTable>();
                _NotificationList = CollectNotificationList();// CreateNotificationList(NotificationList_UNREAD);
            }
            Context.Close(mContext);

            return PartialView("NotificationFilter", _NotificationList);
        }

    }
}
using Elmah;
using LinqToExcel;
using Microsoft.AspNet.Identity.Owin;
using NHibernate;
using ServiceStack.DataAnnotations;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Web.Areas.APIs.Models;
using SmartCattle.Web.CustomFilters;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartCattle.Web.Controllers
{
    [Authorize]
    public class SettingController : BaseController
    {
        // GET: Setting
        [AuthenticateFilter]
        public ActionResult Index()
        {
            return View();
        }
        #region Import Data
        [HttpGet]
        public ActionResult ImportCattles()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ImportCattles(HttpPostedFileBase FileUpload , string SoftwareType)
        //{  
        //    List<string> data = new List<string>();
        //    if (FileUpload != null)
        //    {
        //        if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //        {
        //            string filename = FileUpload.FileName;
        //            string targetpath = Server.MapPath("~/ImportedExcel/");
        //            FileUpload.SaveAs(targetpath + filename);
        //            string pathToExcelFile = targetpath + filename;
        //            var connectionString = "";
        //            if (filename.EndsWith(".xls"))
        //            {
        //                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
        //            }
        //            else if (filename.EndsWith(".xlsx"))
        //            {
        //                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
        //            }
        //           return ImportData(pathToExcelFile, SoftwareType);
        //        }
        //        else
        //        {
        //            data.Add("Only Excel file format is allowed");
        //            data.ToArray();
        //            return Json(data, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        if (FileUpload == null) data.Add("Please choose Excel file");
        //        data.ToArray();
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public FertilityStates stringToFertilityEnum(string value)
        {
            switch (value)
            {
                case "Abort": return FertilityStates.Abort;
                case "BClf": return FertilityStates.BClf;
                case "Calf": return FertilityStates.Calf;
                case "Cull": return FertilityStates.Cull;
                case "Dry": return FertilityStates.Dry;
                case "Early": return FertilityStates.Early;
                case "Fresh": return FertilityStates.Fresh;
                case "Heifer": return FertilityStates.Heifer;
                case "Insem": return FertilityStates.Insem;
                case "Lead": return FertilityStates.Lead;
                case "Open": return FertilityStates.Open;
                case "Other": return FertilityStates.Other;
                case "Preg": return FertilityStates.Preg;
                case "Preg2": return FertilityStates.Preg2;
                case "Ready": return FertilityStates.Ready;
                case "TBCul": return FertilityStates.TBCul;
                case "گاو ماده - تلقيح شده": return FertilityStates.Insem;
                case "گاو ماده - آبستن": return FertilityStates.Preg;
                case "گاو ماده - تازه زا": return FertilityStates.Fresh;
                case "گاو ماده - تحت درمان": return FertilityStates.Treated;
                case "گاو ماده - خشك آبستن": return FertilityStates.DryPreg;
                case "گاو ماده - خشك غيرآبستن":return FertilityStates.DryNonPreg;
                default: return FertilityStates.Other;
            }
        }

        public FertilityStates ModiramFertilityEnum(string status, string inseminationStatus)
        {
            if (status== "آبستن" && inseminationStatus== "تلقيح شده")
            {
                return FertilityStates.InsemPreg;
            }
            else if (status == "آبستن" && inseminationStatus == "آماده تلقيح")
            {
                return FertilityStates.ReadyPreg;
            }
            else if (status == "آبستن" && inseminationStatus == "غيرآماده تلقيح")
            {
                return FertilityStates.NotReadyPreg;
            }
            else if (status == "غيرآبستن" && inseminationStatus == "تلقيح شده")
            {
                return FertilityStates.InsemNonPreg;
            }
            else if (status == "غيرآبستن" && inseminationStatus == "آماده تلقيح")
            {
                return FertilityStates.ReadyNonPreg;
            }
            else if (status == "غيرآبستن" && inseminationStatus == "تلقيح شده")
            {
                return FertilityStates.InsemNonPreg;
            }
            else
            {
                return FertilityStates.Other;
            }          
        }

        public DateTime stringToDate(string date , string softwareType)
        {
            try
            {
                if (softwareType == "WestFalia")
                {
                    if (!string.IsNullOrEmpty(date))
                    {
                        string[] _date = date.Split('/');
                        DateTime Date = new DateTime(int.Parse(_date[2]), int.Parse(_date[1]), int.Parse(_date[0]));
                        return Date;
                    }
                    return DateTime.Now;
                }
                else if(softwareType=="Baniasadi")
                {
                    if (!string.IsNullOrEmpty(date))
                    {
                        string[] _date = date.Split('/');
                        PersianCalendar pc = new PersianCalendar();

                        DateTime Date = new DateTime(int.Parse(_date[0]) + 1300, int.Parse(_date[1]), int.Parse(_date[2]), pc);
                        return Date;
                    }
                    return DateTime.Now;
                }
                else if (softwareType == "Modiran")
                {
                    if (!string.IsNullOrEmpty(date))
                    {
                        string[] _date = date.Split('/');
                        PersianCalendar pc = new PersianCalendar();

                        DateTime Date = new DateTime(int.Parse(_date[0]), int.Parse(_date[1]), int.Parse(_date[2]), pc);
                        return Date;
                    }
                    return DateTime.Now;
                }
                return DateTime.Now;
            }
            catch(Exception exp)
            {
                ErrorSignal.FromCurrentContext().Raise(exp);
                return DateTime.Now;
            }
        }
               
        //public JsonResult ImportData(string pathToExcelFile , string softwareType)
        //{

        //    var excelFile = new ExcelQueryFactory(pathToExcelFile);
        //    SmartCattleContext context = new SmartCattleContext();
        //    List<Cattle> CattleList;
        //    List<CattleFrtilityState> FertilityStatesList = new List<CattleFrtilityState>();
        //    List<string> data = new List<string>();

        //    string sheetName = "Sheet1"; 
        //    var cattles = from a in excelFile.Worksheet<CattleExcel>(sheetName) select a;
        //    IEnumerable<object> freestalls = excelFile.Worksheet<FreeStallExcel>(sheetName).Select(p => p.Group).Distinct().AsEnumerable(); //from a in excelFile.Worksheet<FreeStallExcel>(sheetName) select a;
            
        //    //var existFreestalls = context.FreeStalls.Where(f => f.FarmID == farmID);
           
        //    //try
        //    //{
        //    //    if(softwareType=="Modiran")
        //    //    {
        //    //        if (existFreestalls != null && existFreestalls.Count() > 0)
        //    //        { freestalls = freestalls.Where(fr => !(existFreestalls.Select(f => f.name).Contains((string)fr))); }

        //    //        int counter = 0;
        //    //        foreach (var f in freestalls)
        //    //        {
        //    //            FreeStall stall = new FreeStall();
        //    //            stall.FarmID = farmID;
        //    //            stall.UserId = userID;
        //    //            //stall.code = ++counter;
        //    //            stall.name =(string)f;
        //    //            context.FreeStalls.Add(stall); 
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        if (existFreestalls != null && existFreestalls.Count() > 0)
        //    //        //{ freestalls = freestalls.Where(fr => !(existFreestalls.Select(f => f.code).Contains(Convert.ToInt32(fr)))); }

        //    //        foreach (var f in freestalls)
        //    //        {
        //    //            FreeStall stall = new FreeStall();
        //    //            stall.FarmID = farmID;
        //    //            stall.UserId = userID;
        //    //            //stall.code = Convert.ToInt32(f);
        //    //            stall.name = f.ToString();
        //    //            context.FreeStalls.Add(stall);
        //    //        }
        //    //    }              
        //    //    context.saveChanges();
        //    //    FreeStallList = context.FreeStalls.ToList();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ErrorSignal.FromCurrentContext().Raise(ex);
        //    //    data.Add("Error in import group"+ex.Message);
        //    //    data.ToArray();
        //    //    return Json(data, JsonRequestBehavior.AllowGet);
        //    //}

        //    CattleList = new List<Cattle>();
        //    var existCattle = context.Cattles.Where(c => c.FarmID == farmID);
        //    #region WestFalia
        //    if (softwareType == "WestFalia")
        //    {
        //        foreach (var a in cattles)
        //        {
        //            try
        //            {
        //                Cattle cattle = new Cattle();
        //                cattle.milkAvg = a.AvgMilk;
        //                cattle.animalNumber = a.Body;
        //                cattle.Dim = a.DIM;
        //                cattle.preg = a.Preg;
        //                //cattle.FreeStallId = FreeStallList.FirstOrDefault(f => f.code == Convert.ToInt32(a.Group)).ID;
        //                cattle.lactationNumber = a.Lactation;
        //                cattle.fertilityStatus = stringToFertilityEnum(a.Status);
        //                cattle.InseminationCount = a.InsCount;
        //                cattle.lastCalvingDate = stringToDate(a.CalveDate, "WestFalia");
        //                cattle.lastInseminationDate = stringToDate(a.InsemDate, "WestFalia");
        //                cattle.birthDate = stringToDate(a.BirthDate, "WestFalia");
        //                cattle.age = DateHelper.calculateAge(cattle.birthDate);
        //                cattle.FarmID = farmID;
        //                cattle.UserId = userID;
        //                CattleList.Add(cattle);

        //                cattle.FertilityStates.Add(new CattleFrtilityState()
        //                {
        //                    date = DateTime.Now,
        //                    FarmID = farmID,
        //                    UserId = userID,
        //                    value = stringToFertilityEnum(a.Status),
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                data.Add("Error in import cattle");
        //                data.ToArray();
        //                return Json(data, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    #endregion
        //    #region Baniasadi
        //    else if (softwareType=="Baniasadi")
        //    {
        //        foreach (var a in cattles)
        //        {
        //            try
        //            {
        //                Cattle cattle = new Cattle();
        //                cattle.milkAvg = a.AvgMilk;
        //                cattle.animalNumber = a.Body;
        //                cattle.Dim = a.DIM;
        //                cattle.preg = a.Preg;
        //                //cattle.FreeStallId = FreeStallList.FirstOrDefault(f => f.code ==(Convert.ToInt32(a.Group))).ID;
        //                cattle.lactationNumber = a.Lactation;
        //                cattle.fertilityStatus = stringToFertilityEnum(a.Status); 
        //                cattle.lastCalvingDate = stringToDate(a.CalveDate, "Baniasadi");
        //                cattle.lastInseminationDate = stringToDate(a.InsemDate, "Baniasadi");
        //                cattle.birthDate = stringToDate(a.BirthDate, "Baniasadi");
        //                cattle.age = DateHelper.calculateAge(cattle.birthDate);
        //                cattle.FarmID = farmID;
        //                cattle.UserId = userID;
        //                CattleList.Add(cattle);

        //                cattle.FertilityStates.Add(new CattleFrtilityState()
        //                {
        //                    date = DateTime.Now,
        //                    FarmID = farmID,
        //                    UserId = userID,
        //                    value = stringToFertilityEnum(a.Status),
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorSignal.FromCurrentContext().Raise(ex);
        //                data.Add("Error in import cattle");
        //                data.ToArray();
        //                return Json(data, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    #endregion
        //    #region Modiran
        //    else if (softwareType == "Modiran")
        //    {
        //        foreach (var a in cattles)
        //        {
        //            try
        //            {
        //                Cattle cattle = new Cattle();
        //                cattle.milkAvg = a.AvgMilk;
        //                cattle.animalNumber = a.Body;
        //                cattle.Dim = a.DIM;
        //                cattle.preg = a.Preg;
        //                //cattle.FreeStallId = FreeStallList.FirstOrDefault(f => f.name ==(string)a.Group).ID;
        //                cattle.lactationNumber = a.Lactation;
        //                cattle.fertilityStatus = ModiramFertilityEnum(a.Status,a.InseminationStatus);
        //                cattle.lastCalvingDate = stringToDate(a.CalveDate, "Modiran");
        //                cattle.lastInseminationDate = stringToDate(a.InsemDate, "Modiran");
        //                cattle.birthDate = stringToDate(a.BirthDate, "Modiran");
        //                cattle.age = DateHelper.calculateAge(cattle.birthDate);
        //                cattle.FarmID = farmID;
        //                cattle.UserId = userID;
        //                CattleList.Add(cattle);

        //                cattle.FertilityStates.Add(new CattleFrtilityState()
        //                {
        //                    date = DateTime.Now,
        //                    FarmID = farmID,
        //                    UserId = userID,
        //                    value = stringToFertilityEnum(a.Status),
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                data.Add("Error in import cattle");
        //                data.ToArray();
        //                return Json(data, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    #endregion
        //    try
        //    {
        //        context.Cattles.AddRange(CattleList);
        //        context.saveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //        data.Add("Error in import cattle");
        //        data.ToArray();
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }

        //    if ((System.IO.File.Exists(pathToExcelFile)))
        //    {
        //        System.IO.File.Delete(pathToExcelFile);
        //    }
        //    return Json("success", JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Notification
        //[HttpGet]
        //public ActionResult AddNotification()
        //{
        //    List<Notification> notifications;
        //    using (SmartCattleContext db = new SmartCattleContext())
        //    {
        //        notifications = db.Notifications.Where(f => f.FarmId == farmID).ToList();
        //    }
        //    return View(notifications);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddNotification(AddNotificationModel model)
        //{
        //    if (string.IsNullOrEmpty(model.Title))
        //    {
        //        ViewBag.notifTitle = "fill this filed";
        //        return View();
        //    }
        //    List<Notification> notifications = new List<Notification>();
        //    using (SmartCattleContext db = new SmartCattleContext())
        //    {
        //        try
        //        {
        //            Notification obj = new Notification();
        //            obj.Content = model.Content;
        //            obj.FarmId = farmID;
        //            obj.UserId = userID;
        //            obj.Rule = model.Rule;
        //            obj.Title = model.Title;
        //            db.Notifications.Add(obj);
        //            db.saveChanges();

        //            notifications = db.Notifications.Where(n => n.FarmId == farmID).ToList();

        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorSignal.FromCurrentContext().Raise(ex);
        //        }
        //    }
        //    return View(notifications);
        //}

        [HttpGet]
        public ActionResult ManageNotifications()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult RemoveNotification(int id)
        //{
        //    using (SmartCattleContext db = new SmartCattleContext())
        //    {
        //        try
        //        {
        //            var notif = db.Notifications.FirstOrDefault(n => n.ID == id);
        //            db.Notifications.Remove(notif);
        //            if (db.saveChanges() > 0)
        //            {
        //                return Json(new ActionMessage() { type = messageType.success });
        //            }
        //            return Json(new ActionMessage() { type = messageType.error });
        //        }
        //        catch
        //        {
        //            return Json(new ActionMessage() { type = messageType.error });
        //        }
        //    }
        //}
        #endregion

        #region Role Notification
        //[HttpGet]
        //public ActionResult AddRoleNotification()
        //{
        //    RoleNotificationModel model = new RoleNotificationModel();
        //    var rolemanager= HttpContext.GetOwinContext().Get<SmartCattleRoleManager>();
        //    SmartCattleContext db = new SmartCattleContext();            
        //    model.rolenotifications=db.RoleNotifications.Include("Role").Where(f => f.FarmId == farmID).ToList();
        //    model.roles = rolemanager.Roles.Where(r => r.FarmID == farmID).ToList();
        //    model.notifications = db.Notifications.Where(n => n.FarmId == farmID).ToList();           
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddRoleNotification(AddRoleNotificationModel model)
        //{            
        //    RoleNotificationModel viewModel = new RoleNotificationModel();
        //    var rolemanager = HttpContext.GetOwinContext().Get<SmartCattleRoleManager>();
        //    SmartCattleContext db = new SmartCattleContext();
            
        //        try
        //        {
        //            RoleNotification obj = new RoleNotification();                   
        //            obj.FarmId = farmID;
        //            obj.UserId = userID;
        //            obj.Maskable = model.Maskable;
        //            obj.priority = model.Priority;
        //            obj.NotificationId = model.NotificationId;
        //            obj.RoleId = model.RoleId;
        //            db.RoleNotifications.Add(obj);
        //            db.saveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorSignal.FromCurrentContext().Raise(ex);
        //        }

        //    viewModel.rolenotifications = db.RoleNotifications.Include("Role").Where(f => f.FarmId == farmID).ToList();
        //    viewModel.roles = rolemanager.Roles.Where(r => r.FarmID == farmID).ToList();
        //    viewModel.notifications = db.Notifications.Where(n => n.FarmId == farmID).ToList();

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult RemoveRoleNotification(int id)
        //{
        //    using (SmartCattleContext db = new SmartCattleContext())
        //    {
        //        try
        //        {
        //            var notif = db.RoleNotifications.FirstOrDefault(n => n.ID == id);
        //            db.RoleNotifications.Remove(notif);
        //            if (db.saveChanges() > 0)
        //            {
        //                return Json(new ActionMessage() { type = messageType.success });
        //            }
        //            return Json(new ActionMessage() { type = messageType.error });
        //        }
        //        catch
        //        {
        //            return Json(new ActionMessage() { type = messageType.error });
        //        }
        //    }
        //}

        public ActionResult ACL()
        {
            var controllers = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t)).Select(t => t);
            return View(controllers);
        }
        #endregion

        #region Access Control List
        //[CheckRole]
        //public ActionResult Privilage()
        //{
        //    SmartCattleContext db = new SmartCattleContext();
        //    PrivilageViewModel VM = new Controllers.PrivilageViewModel();
        //    VM.FarmRoles = db.UserRoles.ToList().Where(u => u.FarmID == farmID).ToList();
        //    VM.RolePermissions = db.RolePermissions.ToList();
        //    return View(VM);
        //}

        public ActionResult CreateAccountWizard()
        {
            bool f_find = true;
            int FarmId = Helper.Helper.getCurrentFarmId();
            AccountView _AccountView = new AccountView();
            _AccountView.DefinedFarmAccountList = new List<UserInfoView>();

            ISession mContext = Context.Open();
            List<RolesList_UserTbl> RoleNameList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == FarmId).List().ToList();
            List<FarmTbl> FarmList = new List<FarmTbl>();
            if (Helper.Helper.getCurrentFarmId() == -1)
            {
                String AccessFarmList = mContext.QueryOver<UserInfo>().Where(x => x.ID == Helper.Helper.getCurrentUserId()).Select(x => x.FarmIdList).SingleOrDefault<String>();
                if(AccessFarmList != null || AccessFarmList != "")
                {
                    String[] SplitedAccessFarmList = AccessFarmList.Split(',');
                    if(SplitedAccessFarmList.Length != 0)
                    {
                        for (int i = 0; i < SplitedAccessFarmList.Length; i++)
                        {
                            int iAccessFarmId = Convert.ToInt16(SplitedAccessFarmList[i]);
                            FarmTbl tmpUserInfoList = mContext.QueryOver<FarmTbl>().Where(x => x.ID == iAccessFarmId).SingleOrDefault<FarmTbl>();
                            if(tmpUserInfoList != null)
                            {
                                FarmList.Add(tmpUserInfoList);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
            List<UserInfo> StaffInfoListTmp = mContext.QueryOver<UserInfo>().Where(x => x.FarmId == -1).List().ToList();
            List<UserInfoView> _UserInfoView = new List<UserInfoView>();
            foreach (var item in StaffInfoListTmp)
            {
                UserInfoView StaffInfoList = new UserInfoView()
                {
                    CreateDate = item.CreateDate,
                    Email = item.Email, 
                    Family = item.Family,
                    FarmId = item.FarmId,
                    FarmIdList = item.FarmIdList,
                    FarmName = "Nothing",
                    ID = item.ID,
                    Name = item.Name,
                    Password = item.Password,
                    RoleId = item.RoleId,
                    RoleName = item.RoleName
                };
                _UserInfoView.Add(StaffInfoList);
            }

            int CurrentUserID = Helper.Helper.getCurrentUserId();
            if(CurrentUserID != 0)
            {
                if (Helper.Helper.getCurrentFarmId() != -1)
                {
                    String CurrentUserRole = Helper.Helper.getCurrentRoleuId();
                    int CurrentFarmId = Helper.Helper.getCurrentFarmId();

                    String CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();

                    String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
                    foreach (var user in UserInfoList)
                    {
                        String NewUserPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == user.RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
                        if (NewUserPermission != null)
                        {
                            String[] NewUserPermissionList = NewUserPermission.Split(',');
                            if (SplitedCurrentUserPermissionList.Length >= NewUserPermissionList.Length)
                            {
                                foreach (var ActionController in NewUserPermissionList)
                                {
                                    if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    String FarmName = "Nothing";
                                    if (user.FarmId != -1)
                                    {
                                        FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == user.FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                                    }
                                    if (FarmName != null)
                                    {
                                        UserInfoView tmpView = new UserInfoView()
                                        {
                                            CreateDate = user.CreateDate,
                                            RoleName = user.RoleName,
                                            RoleId = user.RoleId,
                                            Password = user.Password,
                                            Name = user.Name,
                                            ID = user.ID,
                                            Email = user.Email,
                                            Family = user.Family,
                                            FarmId = user.FarmId,
                                            FarmIdList = user.FarmIdList,
                                            FarmName = FarmName
                                        };
                                        _AccountView.DefinedFarmAccountList.Add(tmpView);
                                    }
                                }
                            }
                        }
                    }

                    f_find = true;
                    List<RolesList_UserTbl> AccessRoleList = new List<RolesList_UserTbl>();
                    for (int i = 0; i < RoleNameList.Count; i++)
                    {
                        String tmpPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleNameList[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
                        if(tmpPermission != null)
                        {
                            String[] SplitedtmpPermission = tmpPermission.Split(',');
                            
                            if (SplitedCurrentUserPermissionList.Length >= SplitedtmpPermission.Length)
                            {
                                foreach (var ActionController in SplitedtmpPermission)
                                {
                                    if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    AccessRoleList.Add(RoleNameList[i]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }

                    _AccountView.permissionList = AccessRoleList;
                    _AccountView.FarmList = FarmList;
                    _AccountView.DefinedSystem_StaffList = _UserInfoView;
                    Context.Close(mContext);
                }
                else
                {
                    String CurrentUserRole = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserID).Select(x => x.RoleId).SingleOrDefault<String>();
                    String CurrentUserPermissionList = "";
                    if(Helper.Helper.getCurrentFarmId() == -1)
                    {
                        CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    }
                    else
                    {
                        CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    }

                    String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');

                    String FarmListNotSplited = mContext.QueryOver<UserInfo>().Where(x => x.ID == Helper.Helper.getCurrentUserId()).Select(x => x.FarmIdList).SingleOrDefault<String>();
                    if(FarmListNotSplited != null)
                    {
                        String[] SplitedFarmList = FarmListNotSplited.Split(',');
                        for (int i = 0; i < SplitedFarmList.Length; i++)
                        {
                            int SelectedFarmId = Convert.ToInt16(SplitedFarmList[i]);
                            List<UserInfo> _UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.FarmId == SelectedFarmId).List().ToList();
                            foreach (var user in _UserInfoList)
                            {
                                String NewUserPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == user.RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
                                if (NewUserPermission != null)
                                {
                                    String[] NewUserPermissionList = NewUserPermission.Split(',');
                                    if (SplitedCurrentUserPermissionList.Length >= NewUserPermissionList.Length)
                                    {
                                        foreach (var ActionController in NewUserPermissionList)
                                        {
                                            if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                            {
                                                f_find = false;
                                            }
                                        }
                                        if (f_find)
                                        {
                                            ////////////////////////////////////////////////////
                                            String FarmName = "Nothing";
                                            if (user.FarmId != -1)
                                            {
                                                FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == user.FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                                            }
                                            if(FarmName != null)
                                            {
                                                UserInfoView tmpView = new UserInfoView()
                                                {
                                                    CreateDate = user.CreateDate,
                                                    RoleName = user.RoleName,
                                                    RoleId = user.RoleId,
                                                    Password = user.Password,
                                                    Name = user.Name,
                                                    ID = user.ID,
                                                    Email = user.Email,
                                                    Family = user.Family,
                                                    FarmId = user.FarmId,
                                                    FarmIdList = user.FarmIdList,
                                                    FarmName = FarmName
                                                };
                                                _AccountView.DefinedFarmAccountList.Add(tmpView);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        List<UserInfo> WithoutFarm = mContext.QueryOver<UserInfo>().Where(x => x.FarmId == -3).List().ToList();
                        foreach (var user in WithoutFarm)
                        {
                            UserInfoView tmpView = new UserInfoView()
                            {
                                CreateDate = user.CreateDate,
                                RoleName = user.RoleName,
                                RoleId = user.RoleId,
                                Password = user.Password,
                                Name = user.Name,
                                ID = user.ID,
                                Email = user.Email,
                                Family = user.Family,
                                FarmId = user.FarmId,
                                FarmIdList = user.FarmIdList,
                                FarmName = "Un-Assigned"
                            };
                            _AccountView.DefinedFarmAccountList.Add(tmpView);
                        }
                        //List<UserInfo> _UknownUserInfoList = mContext.QueryOver<UserInfo>()/*.Where(x => x.FarmId == -3)*/.List().ToList();
                        //foreach (var user in _UknownUserInfoList)
                        //{
                        //    String FarmName = "Nothing";
                        //    if (user.FarmId != -1)
                        //    {
                        //        FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == user.FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                        //    }
                        //    if(FarmName != null)
                        //    {
                        //        UserInfoView tmpView = new UserInfoView()
                        //        {
                        //            CreateDate = user.CreateDate,
                        //            RoleName = user.RoleName,
                        //            RoleId = user.RoleId,
                        //            Password = user.Password,
                        //            Name = user.Name,
                        //            ID = user.ID,
                        //            Email = user.Email,
                        //            Family = user.Family,
                        //            FarmId = user.FarmId,
                        //            FarmIdList = user.FarmIdList,
                        //            FarmName = FarmName
                        //        };
                        //        _AccountView.DefinedFarmAccountList.Add(tmpView);
                        //    }
                        //}
                    }
                    else
                    {

                    }

                    ///////////////////////////////////
                    f_find = true;
                    List<UserInfoView> AccessUserStaff = new List<UserInfoView>();
                    //String CurrentUserRole = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserID).Select(x => x.Role).SingleOrDefault<String>();
                    //String CurrentUserPermissionList = mContext.QueryOver<UserRoles>().Where(x => x.Name == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    //String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
                    foreach (var user in _UserInfoView)
                    {
                        String NewUserPermission = "";
                        if (Helper.Helper.getCurrentFarmId() == -1)
                        {
                            NewUserPermission = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == user.RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
                        }
                        else
                        {
                            NewUserPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == user.RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
                        }

                        if (NewUserPermission != null)
                        {
                            String[] NewUserPermissionList = NewUserPermission.Split(',');
                            if (SplitedCurrentUserPermissionList.Length > NewUserPermissionList.Length)
                            {
                                foreach (var ActionController in NewUserPermissionList)
                                {
                                    if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    String FarmName = "Nothing";
                                    if (user.FarmId != -1)
                                    {
                                        FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == user.FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                                    }
                                    UserInfoView tmpView = new UserInfoView()
                                    {
                                        CreateDate = user.CreateDate,
                                        Email = user.Email,
                                        Family = user.Family,
                                        FarmId = user.FarmId,
                                        FarmIdList = user.FarmIdList,
                                        FarmName = FarmName,
                                        ID = user.ID,
                                        Name = user.Name,
                                        Password = user.Password,
                                        RoleId = user.RoleId,
                                        RoleName = user.RoleName
                                    };
                                    AccessUserStaff.Add(tmpView);
                                }
                            }
                        }
                    }
                    //////////////////////////////////
                    _AccountView.permissionList = RoleNameList;
                    _AccountView.FarmList = FarmList;
                    _AccountView.DefinedSystem_StaffList = AccessUserStaff;

                    Context.Close(mContext);
                }
            }
            else
            {

            }
            
            return View(_AccountView);
        }

        public class AccountView
        {
            public List<RolesList_UserTbl> permissionList { get; set; }
            public List<FarmTbl> FarmList { get; set; }
            public List<UserInfoView> DefinedFarmAccountList { get; set; }
            public List<UserInfoView> DefinedSystem_StaffList { get; set; }
        }

        public class UserInfoView
        {
            public virtual int ID { get; set; }
            public virtual String Name { get; set; }
            public virtual String Family { get; set; }
            public virtual String Email { get; set; }
            public virtual String Password { get; set; }
            public virtual String RoleId { get; set; }
            public virtual String RoleName { get; set; }
            public virtual int FarmId { get; set; }
            public virtual String FarmName { get; set; }
            public virtual String FarmIdList { get; set; }
            public virtual DateTime CreateDate { get; set; }
        }

        [HttpPost]
        public JsonResult LoadAllRoleOfFarm(String FarmId)
        {
            List<String> retValue = new List<string>();

            if (!FarmId.Equals("NaN"))
            {
                bool f_find = true;
                int currentUserId = Helper.Helper.getCurrentUserId();

                ISession mContext = Context.Open();
                List<RolesList_UserTbl> RoleNameList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Convert.ToInt16(FarmId)).List<RolesList_UserTbl>().ToList();
                String CurrentUserRole = mContext.QueryOver<UserInfo>().Where(x => x.ID == currentUserId).Select(x => x.RoleId).SingleOrDefault<String>();

                String CurrentUserPermissionList = "";
                if(Helper.Helper.getCurrentFarmId() == -1)
                {
                    CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                }
                else
                {
                    CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                }
                Context.Close(mContext);

                String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
                foreach (var BasePermission in RoleNameList)
                {
                    String[] SplitedBasePermission = BasePermission.Permissions.Split(',');
                    String RoleUid = BasePermission.uId;

                    if (SplitedCurrentUserPermissionList.Length >= SplitedBasePermission.Length)
                    {
                        foreach (var ActionController in SplitedBasePermission)
                        {
                            if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                            {
                                f_find = false;
                            }
                        }
                        if (f_find)
                        {
                            retValue.Add(BasePermission.uId);
                            retValue.Add(BasePermission.jName);
                        }
                    }
                }
            }

            return Json(retValue);
        }

        [HttpPost]
        public JsonResult LoadRoleOfStaff(String FarmId)
        {
            bool f_find = true;
            int currentUserId = Helper.Helper.getCurrentUserId();
            List<String> retValue = new List<string>();

            ISession mContext = Context.Open();
            List<RolesList_StaffTbl> RoleNameList = mContext.QueryOver<RolesList_StaffTbl>().List<RolesList_StaffTbl>().ToList();
            String CurrentUserRole = mContext.QueryOver<UserInfo>().Where(x => x.ID == currentUserId).Select(x => x.RoleId).SingleOrDefault<String>();

            String CurrentUserPermissionList = "";
            if (Helper.Helper.getCurrentFarmId() == -1)
            {
                CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
            }
            else
            {
                CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
            }

            Context.Close(mContext);

            String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
            foreach (var BasePermission in RoleNameList)
            {
                String[] SplitedBasePermission = BasePermission.Permissions.Split(',');
                if (SplitedCurrentUserPermissionList.Length >= SplitedBasePermission.Length)
                {
                    foreach (var ActionController in SplitedBasePermission)
                    {
                        if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                        {
                            f_find = false;
                        }
                    }
                    if (f_find)
                    {
                        retValue.Add(BasePermission.uId);
                    }
                }
            }
            return Json(retValue);
        }

        [HttpPost]
        public String SaveAccount(String txtFirstName, String txtLastName, String txtEmail, String txtPassword, String txtConfirmPassword, String SelectedFarmType, String SelectedFarmName, String SelectedRoleName, String FarmList)
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            if(txtPassword.Equals(txtConfirmPassword))
            {
                if (SelectedFarmName != null)
                {
                    if (SelectedFarmName.Equals("NaN"))
                    {
                        if(SelectedFarmType.Equals("SystemRole_Staff"))
                        {
                            List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                            if (_UserInfo.Count == 0)
                            {
                                mContext.Clear();

                                String FarmIdList = FarmList.Replace("onoffswitch", "");
                                FarmIdList = FarmIdList.Remove(FarmIdList.Length - 1, 1);
                                String StaffPermission = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == SelectedRoleName).Select(x => x.Permissions).SingleOrDefault<String>();
                                if(StaffPermission != null)
                                {
                                    UserInfo newUser = new UserInfo()
                                    {
                                        Name = txtFirstName,
                                        Family = txtLastName,
                                        Email = txtEmail,
                                        Password = txtPassword,
                                        RoleName = SelectedRoleName,
                                        RoleId = Encryption.GenerateAlarmUId(),
                                        FarmId = -1,
                                        FarmIdList = FarmIdList,
                                        CreateDate = DateTime.Now
                                    };
                                    mContext.Save(newUser);
                                    retValue = "SAVE";
                                }
                                else
                                {
                                    retValue = "NaNROLE";
                                }
                            }
                            else
                            {
                                retValue = "SIMILAR_EMAIL";
                            }
                        }
                        else
                        {
                            List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                            if (_UserInfo.Count == 0)
                            {
                                mContext.Clear();
                                UserInfo newUser = new UserInfo()
                                {
                                    Name = txtFirstName,
                                    Family = txtLastName,
                                    Email = txtEmail,
                                    Password = txtPassword,
                                    RoleName = "NaN",
                                    RoleId = "NaN",
                                    FarmId = -3,
                                    CreateDate = DateTime.Now
                                };
                                mContext.Save(newUser);
                                retValue = "SAVE";
                            }
                            else
                            {
                                retValue = "SIMILAR_EMAIL";
                            }
                        }
                    }
                    else
                    {
                        int ChoosenFarm = Convert.ToInt16(SelectedFarmName);
                        List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                        if (_UserInfo.Count == 0)
                        {
                            mContext.Clear();
                            String Permissions = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == SelectedRoleName).Select(x => x.Permissions).SingleOrDefault<String>();
                            if(Permissions != null)
                            {
                                UserInfo newUser = new UserInfo()
                                {
                                    Name = txtFirstName,
                                    Family = txtLastName,
                                    Email = txtEmail,
                                    Password = txtPassword,
                                    RoleName = SelectedRoleName,
                                    RoleId = Encryption.GenerateAlarmUId(),
                                    FarmId = ChoosenFarm,
                                    CreateDate = DateTime.Now
                                };
                                mContext.Save(newUser);
                                retValue = "SAVE";
                            }
                            else
                            {
                                retValue = "NaNROLE";
                            }
                        }
                        else
                        {
                            retValue = "SIMILAR_EMAIL";
                        }
                    }
                }
                else
                {
                    if(SelectedFarmName != "")
                    {
                        List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                        if (_UserInfo.Count == 0)
                        {
                            RolesList_UserTbl ChoosenRole = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).Where(x => x.uId == SelectedRoleName).SingleOrDefault();
                            if(ChoosenRole != null)
                            {
                                mContext.Clear();
                                UserInfo newUser = new UserInfo()
                                {
                                    Name = txtFirstName,
                                    Family = txtLastName,
                                    Email = txtEmail,
                                    Password = txtPassword,
                                    RoleName = ChoosenRole.jName,
                                    RoleId = ChoosenRole.uId,
                                    FarmId = Helper.Helper.getCurrentFarmId(),
                                    CreateDate = DateTime.Now
                                };
                                mContext.Save(newUser);
                                retValue = "SAVE";
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            retValue = "SIMILAR_EMAIL";
                        }
                    }
                    else
                    {
                        retValue = "NaNROLE";
                    }
                }
            }
            else
            {
                retValue = "MISSMATCH";
            }

            Context.Close(mContext);
            return retValue;
        }

        [HttpPost]
        public String RemoveDefineFarmAcc(String AccountID)
        {
            ISession mContext = Context.Open();
            String RemoveStaffRole = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.UserInfo", AccountID);
            mContext.CreateSQLQuery(RemoveStaffRole).ExecuteUpdate();
            Context.Close(mContext);
            return "OK";
        }

        public ActionResult EditDefineFarmAcc(String jq)
        {
            ISession mContext = Context.Open();
            Boolean f_find = true;
            List<RolesList_UserTbl> AccessRoleList = new List<RolesList_UserTbl>();
            String CurrentUserRole = Helper.Helper.getCurrentRoleuId();
            int CurrentFarmId = Helper.Helper.getCurrentFarmId();
            String CurrentUserPermissionList = "NaN";
            if(CurrentFarmId == -1)
            {
                CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
            }
            else
            {
                CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
            }
            
            String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
            int FarmId = Helper.Helper.getCurrentFarmId();

            mContext.Clear();
            UserInfo _userInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == Convert.ToInt16(jq)).SingleOrDefault();
            if (_userInfo == null)
            {
                _userInfo = new UserInfo();
            }

            List<RolesList_UserTbl> RoleNameList = mContext.QueryOver<RolesList_UserTbl>().List().ToList();
            for (int i = 0; i < RoleNameList.Count; i++)
            {
                String tmpPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleNameList[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
                if (tmpPermission != null)
                {
                    String[] SplitedtmpPermission = tmpPermission.Split(',');

                    if (SplitedCurrentUserPermissionList.Length >= SplitedtmpPermission.Length)
                    {
                        foreach (var ActionController in SplitedtmpPermission)
                        {
                            if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                            {
                                f_find = false;
                            }
                        }
                        if (f_find)
                        {
                            AccessRoleList.Add(RoleNameList[i]);
                        }
                    }
                }
                else
                {

                }
            }

            EditFarmAccountClass retValue = new EditFarmAccountClass();
            retValue.AccessRoleList = new List<RolesList_UserTbl>();
            retValue.Account = new UserInfo();
            retValue.FarmList = new List<FarmTbl>();
            List<FarmTbl> FarmList = new List<FarmTbl>();

            if(CurrentFarmId == -1)
            {
                int CurrentUserId = Helper.Helper.getCurrentUserId();
                UserInfo _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).SingleOrDefault();
                if(_UserInfo != null)
                {
                    if(_UserInfo.FarmIdList != null)
                    {
                        if(_UserInfo.FarmIdList != "")
                        {
                            String[] SplitedUserInfo = _UserInfo.FarmIdList.Split(',');
                            for (int i = 0; i < SplitedUserInfo.Length; i++)
                            {
                                FarmTbl _farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == Convert.ToInt16(SplitedUserInfo[i])).SingleOrDefault();
                                if (_farm != null)
                                {
                                    FarmList.Add(_farm);
                                }
                            }
                        }
                    }
                }
            }

            retValue.AccessRoleList = AccessRoleList;
            retValue.Account = _userInfo;
            retValue.FarmList = FarmList;

            Context.Close(mContext);

            return View(retValue);
        }

        [HttpPost]
        public String RemoveDefineStaffAcc(String AccountID)
        {
            return "";
        }

        [HttpPost]
        public String EditDefineStaffAcc(String AccountID)
        {
            return "";
        }

        [HttpPost]
        public String UpdateFarmAccount(int divAccountID, String txtFirstName, String txtLastName, String txtEmail, String txtPassword, String txtConfirmPassword, String SelectedFarmType, String SelectedFarmName, String SelectedRoleName, String FarmList)
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            if (txtPassword.Equals(txtConfirmPassword))
            {
                if (SelectedFarmName != null)
                {
                    if (SelectedFarmName.Equals("NaN"))
                    {
                        if (SelectedFarmType.Equals("SystemRole_Staff"))
                        {
                            List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                            if (_UserInfo.Count == 0)
                            {
                                mContext.Clear();

                                String FarmIdList = FarmList.Replace("onoffswitch", "");
                                FarmIdList = FarmIdList.Remove(FarmIdList.Length - 1, 1);
                                String StaffPermission = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == SelectedRoleName).Select(x => x.Permissions).SingleOrDefault<String>();
                                if (StaffPermission != null)
                                {
                                    UserInfo newUser = new UserInfo()
                                    {
                                        Name = txtFirstName,
                                        Family = txtLastName,
                                        Email = txtEmail,
                                        Password = txtPassword,
                                        RoleName = SelectedRoleName,
                                        RoleId = Encryption.GenerateAlarmUId(),
                                        FarmId = -1,
                                        FarmIdList = FarmIdList,
                                        CreateDate = DateTime.Now
                                    };
                                    mContext.Save(newUser);
                                    retValue = "SAVE";
                                }
                                else
                                {
                                    retValue = "NaNROLE";
                                }
                            }
                            else
                            {
                                retValue = "NOT_SIMILAR_EMAIL";
                            }
                        }
                        else
                        {
                            List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                            if (_UserInfo.Count == 0)
                            {
                                mContext.Clear();
                                UserInfo newUser = new UserInfo()
                                {
                                    Name = txtFirstName,
                                    Family = txtLastName,
                                    Email = txtEmail,
                                    Password = txtPassword,
                                    RoleId = "NaN",
                                    RoleName = "NaN",
                                    FarmId = -3,
                                    CreateDate = DateTime.Now
                                };
                                mContext.Save(newUser);
                                retValue = "SAVE";
                            }
                            else
                            {
                                retValue = "NOT_SIMILAR_EMAIL";
                            }
                        }
                    }
                    else
                    {
                        int ChoosenFarm = Convert.ToInt16(SelectedFarmName);
                        UserInfo _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == divAccountID).SingleOrDefault();
                        if (_UserInfo != null)
                        {
                            mContext.Clear();
                            RolesList_UserTbl _UserRoles = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == SelectedRoleName).SingleOrDefault();
                            if (_UserRoles != null)
                            {
                                _UserInfo.Name = txtFirstName;
                                _UserInfo.Family = txtLastName;
                                _UserInfo.Email = txtEmail;
                                _UserInfo.Password = txtPassword;
                                _UserInfo.RoleName = _UserRoles.jName;
                                _UserInfo.RoleId = SelectedRoleName;
                                _UserInfo.FarmId = ChoosenFarm;
                                _UserInfo.CreateDate = DateTime.Now;
                                
                                mContext.Update(_UserInfo);
                                mContext.Flush();
                                retValue = "SAVE";
                            }
                            else
                            {
                                retValue = "NaNROLE";
                            }
                        }
                        else
                        {
                            retValue = "NOT_SIMILAR_EMAIL";
                        }
                    }
                }
                else
                {
                    if (SelectedFarmName != "")
                    {
                        List<UserInfo> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                        if (_UserInfo.Count != 0)
                        {
                            RolesList_UserTbl ChoosenRole = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).Where(x => x.uId == SelectedRoleName).SingleOrDefault();
                            if (ChoosenRole != null)
                            {
                                mContext.Clear();
                                List<UserInfo> newUserList = mContext.QueryOver<UserInfo>().Where(x => x.Email == txtEmail).List().ToList();
                                for (int i = 0; i < newUserList.Count; i++)
                                {
                                    newUserList[i].Name = txtFirstName;
                                    newUserList[i].Family = txtLastName;
                                    newUserList[i].Email = txtEmail;
                                    newUserList[i].Password = txtPassword;
                                    if (newUserList[i].ID == Convert.ToInt16(divAccountID))
                                    {
                                        newUserList[i].RoleName = ChoosenRole.jName;
                                        newUserList[i].RoleId = ChoosenRole.uId;
                                    }
                                    newUserList[i].FarmId = Helper.Helper.getCurrentFarmId();
                                    newUserList[i].CreateDate = DateTime.Now;

                                    mContext.Update(newUserList[i]);
                                    mContext.Flush();
                                }

                                retValue = "SAVE";
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            retValue = "NOT_SIMILAR_EMAIL";
                        }
                    }
                    else
                    {
                        retValue = "NaNROLE";
                    }
                }
            }
            else
            {
                retValue = "MISSMATCH";
            }

            Context.Close(mContext);
            return retValue;
        }

        public JsonResult PermissionsList(String ACK)
        {
            List<ActionControllerListTbl> permissionList = new List<ActionControllerListTbl>();
            ISession mContext = Context.Open();
            if(Helper.Helper.getCurrentFarmId() == -1)
            {
                permissionList = mContext.QueryOver<ActionControllerListTbl>().List().ToList();
            }
            else
            {
                String CurrentRoleName = Helper.Helper.getCurrentRoleuId();
                if(CurrentRoleName == null)
                {
                    String NeededRoleName = "FarmManager_" + Helper.Helper.getCurrentFarmId().ToString();
                    int CurrentFarmId = Helper.Helper.getCurrentFarmId();
                    List<UserInfo> Permissions_of_FarmManager = mContext.QueryOver<UserInfo>().Where(x => x.RoleId == NeededRoleName).Where(x => x.FarmId == CurrentFarmId).List().ToList();
                    if(Permissions_of_FarmManager.Count != 0)
                    {
                        String Permissions_of_FarmManager_Permissions = Helper.Helper.getPermissionList(Permissions_of_FarmManager[0].RoleId);

                        if (Permissions_of_FarmManager_Permissions != "")
                        {
                            String[] SplitedPermission = Permissions_of_FarmManager_Permissions.Split(',');
                            foreach (var item in SplitedPermission)
                            {
                                String[] SplitedAction = item.Split('-');
                                ActionControllerListTbl _UserPermissions = new ActionControllerListTbl();
                                _UserPermissions.Action = SplitedAction[1];
                                _UserPermissions.Controller = SplitedAction[0];
                                _UserPermissions.Comment = "Nothing Comment yet";
                                permissionList.Add(_UserPermissions);
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    RolesList_UserTbl _UserRoles = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentRoleName).SingleOrDefault<RolesList_UserTbl>();
                    if (_UserRoles != null)
                    {
                        String[] SplitedPermission = _UserRoles.Permissions.Split(',');
                        foreach (var item in SplitedPermission)
                        {
                            String[] SplitedAction = item.Split('-');
                            ActionControllerListTbl _UserPermissions = new ActionControllerListTbl();
                            _UserPermissions.Action = SplitedAction[1];
                            _UserPermissions.Controller = SplitedAction[0];
                            _UserPermissions.Comment = "Nothing Comment yet";
                            permissionList.Add(_UserPermissions);
                        }
                    }
                    else
                    {

                    }
                }
            }
            Context.Close(mContext);
            
            var ignoredList = Helper.Helper.GetIgnorePrivilegeList();
            var returnList = new Tuple<List<ActionControllerListTbl>, List<Tuple<string, string>>>(permissionList, ignoredList);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public string DeleteRole(String RoleName)
        {
            ISession mContext = Context.Open();
            String deleteAll = string.Format("DELETE FROM {0} where Name = '{1}'", "SmartCattle.UserRoles", RoleName);
            mContext.CreateSQLQuery(deleteAll).ExecuteUpdate();
            Context.Close(mContext);
            //mContext.UsersRole.Delete().Where(x => x.Name == RoleName).ToList();
            return "OK";
        }

        public string MenuView()
        {
            return "OK";
        }

        [ChildActionOnly]
        public ActionResult GetHtmlPage(string path)
        {
            return new FilePathResult(path, "text/html");
        }

        //public PartialViewResult RolePermissions(string roleId)
        //{
        //    SmartCattleContext db = new SmartCattleContext();
        //    ViewBag.roleId = roleId;
        //    return PartialView(db.RolePermissions.ToList());
        //}

        //[HttpPost]
        //[CheckRole]
        //[ValidateAntiForgeryToken]
        //public ActionResult setAccess(string roleId, string permissions ,string Edit)
        //{
        //    SmartCattleContext db = new SmartCattleContext();
        //    PrivilageViewModel VM = new PrivilageViewModel();
        //    var granted = db.RolePermissions.Where(r => r.UserRoles.Select(u => u.Id).Contains(roleId)).Select(r=>r.ID.ToString());
        //    try
        //    {
        //        ActionMessage msg = new ActionMessage();
        //        if (permissions.Length > 0)
        //        {
        //            string[] permissionList = permissions.Split(',');
        //            foreach (var item in granted.Except(permissionList))
        //            {
        //                int id = int.Parse(item);
        //                var RP = db.RolePermissions.Where(r => r.ID == id).FirstOrDefault();
        //                var UR = db.UserRoles.Where(r => r.Id == roleId).FirstOrDefault();
        //                RP.UserRoles.Remove(UR); 
        //            }
        //            foreach (var item in permissionList.Except(granted))
        //            {
        //                int id = int.Parse(item);
        //                var RP = db.RolePermissions.Where(r => r.ID == id).FirstOrDefault();
        //                var UR = db.UserRoles.Where(r => r.Id == roleId).FirstOrDefault();
        //                RP.UserRoles.Add(UR);
        //            }
        //            db.saveChanges();
        //        }
        //    }
        //    catch(Exception exp)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(exp);
        //    }
        //    return RedirectToAction("Privilage");
        //}
        #endregion
    }

    public class FreeStallExcel
    {
        public object Group { get; set; }
    }
   
    public class AddNotificationModel
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [Required]
        public RuleTypes Rule { get; set; }
    }
    public class RoleNotificationModel
    {
       public List<RoleNotification> rolenotifications;
       public List<UserRole> roles;
       public List<Notification> notifications;
    }

    public class AddRoleNotificationModel
    { 
        public string RoleId { get; set; }
        public int NotificationId { get; set; }
        public int Priority { get; set; }
        public bool Maskable { get; set; }
    }

    public class PrivilageViewModel
    {
        public List<RolePermission> RolePermissions { get; set; }
        public List<UserRole> FarmRoles { get; set; }
    }

    public class EditFarmAccountClass
    {
        public List<RolesList_UserTbl> AccessRoleList;
        public UserInfo Account;
        public List<FarmTbl> FarmList;
    }

}
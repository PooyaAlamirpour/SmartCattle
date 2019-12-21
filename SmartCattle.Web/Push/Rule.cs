using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Elmah;

namespace SmartCattle.Web.Push
{
    public class Rule:IDisposable
    {
        //SmartCattleContext db;                               // Database Context
        DateTime noticePeriod = DateTime.Now.AddHours(-1);  // only last hour new datas consider in notification rules
        public Rule()
        { 
            //db = new SmartCattleContext();
            //db.Configuration.LazyLoadingEnabled = true;
            //db.Configuration.ProxyCreationEnabled = true;
        }
        public void RunRules()
        {
           // THIRule();
           // ActivityRule();
           //TempratureRule();
        }
        #region THI Rule
        /// <summary>
        /// the rule for THI notifications
        /// </summary>
        /// <param name="startStress"></param>
        /// <param name="midStressMin"></param>
        /// <param name="midStressMax"></param>
        /// <param name="severeStressMin"></param>
        /// <param name="severeStressMax"></param>
        /// <param name="emergencyStressMin"></param>
       
        //public async void THIRule( int startStress =68 , int midStressMin=79 , int midStressMax = 88, int severeStressMin = 89, int severeStressMax = 98,
        //    int emergencyStressMin = 98)
        //{
        //    try
        //    {
        //        List<UserNotification> notifications = new List<UserNotification>();
        //        foreach (var farm in db.Farms)
        //        {
        //            var relatedNotice = db.Notifications.Where(n => n.Rule == RuleTypes.THI && n.FarmId == farm.ID);
        //            var users = db.Users.Where(u => u.FarmID == farm.ID);
        //            CattleTHI beginStress = db.CattleTHIs.Include("FreeStall").FirstOrDefault(t => t.THIValue >= startStress && t.THIValue <= midStressMin && t.date >= noticePeriod && t.FarmID == farm.ID);
        //            CattleTHI midStress = db.CattleTHIs.Include("FreeStall").FirstOrDefault(t => t.THIValue >= midStressMin && t.THIValue <= midStressMax && t.date >= noticePeriod && t.FarmID == farm.ID);
        //            CattleTHI severeStress = db.CattleTHIs.Include("FreeStall").FirstOrDefault(t => t.THIValue >= severeStressMin && t.THIValue < severeStressMax && t.date >= noticePeriod && t.FarmID == farm.ID);
        //            CattleTHI emergencyStress = db.CattleTHIs.Include("FreeStall").FirstOrDefault(t => t.THIValue >= emergencyStressMin && t.date >= noticePeriod && t.FarmID == farm.ID);
        //            foreach (var user in users)
        //            {
        //                string roleid = user.Roles.FirstOrDefault()!=null? user.Roles.FirstOrDefault().RoleId:"";
        //                RoleNotification relatedRole = db.RoleNotifications.FirstOrDefault(r => r.RoleId == roleid);                          
        //                foreach (var notice in relatedNotice)
        //                {
                            
        //                    if (emergencyStress != null)
        //                    {
        //                        UserNotification obj = new UserNotification();
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = emergencyStress.date;
        //                        //obj.Notification.Content = emergencyStress.FreeStall != null ? emergencyStress.FreeStall.name : "unknow place" +
        //                        //    " " + emergencyStress.THIValue.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        notifications.Add(obj);
        //                    }
        //                    if (severeStress != null)
        //                    {
        //                        UserNotification obj = new UserNotification();
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = severeStress.date;
        //                        //obj.Notification.Content = severeStress.FreeStall != null ? severeStress.FreeStall.name : "unknow place" +
        //                        //    " " + severeStress.THIValue.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        notifications.Add(obj);
        //                    }
        //                    if (midStress != null)
        //                    {
        //                        UserNotification obj = new UserNotification();
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = midStress.date;
        //                        //obj.Notification.Content = midStress.FreeStall != null ? midStress.FreeStall.name : "unknow place" +
        //                        //    " " + midStress.THIValue.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        notifications.Add(obj);
        //                    }
        //                    if (beginStress != null)
        //                    {
        //                        UserNotification obj = new UserNotification();
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = midStress.date;
        //                        //obj.Notification.Content = beginStress.FreeStall != null ? beginStress.FreeStall.name : "unknow place" +
        //                        //    " " + beginStress.THIValue.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        notifications.Add(obj);
        //                    }
        //                }
        //            }
        //        }

        //        db.UserNotifications.AddRange(notifications);
        //        await db.SaveChangesAsync();
        //    }
        //    catch(Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}
        #endregion
        #region Temperature Rule
        /// <summary>
        /// the rule for THI notifications
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        //public async void TempratureRule(double min=36 , double max=37)
        //{          
        //    try
        //    {
        //        foreach (var farm in db.Farms)
        //        {
        //            var relatedNotice = db.Notifications.Where(n => n.Rule == RuleTypes.Temperature && n.FarmId == farm.ID).ToList();
        //            var users = db.Users.Where(u => u.FarmID == farm.ID).ToList();
        //            var Temps = db.Tempretures.Include("Cattle").Where(t => t.value >= max || t.value <= min && t.date >= noticePeriod && t.FarmID == farm.ID).Take(100).ToList();
        //            foreach (var user in users)
        //            {
        //                string roleid = user.Roles.FirstOrDefault()!=null? user.Roles.FirstOrDefault().RoleId:"";
        //                RoleNotification relatedRole = db.RoleNotifications.FirstOrDefault(r => r.RoleId == roleid);

        //                foreach (var notice in relatedNotice)
        //                {
        //                    foreach (var temp in Temps)
        //                    {
        //                        UserNotification obj = new UserNotification();
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification = notice;
        //                        obj.AdditionalInfo = (temp.Cattle!= null ? "Cow : " + temp.Cattle.animalNumber.ToString() : "unknow Cow") +
        //                            " Body Temprature : " + temp.value.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                }
        //            }
        //        }
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}
        #endregion
        #region Activity Rule
        /// <summary>
        /// the rule for Activity notifications
        /// </summary>
        /// <param name="MinEating"></param>
        /// <param name="MaxEating"></param>
        /// <param name="MinSitting"></param>
        /// <param name="MaxSitting"></param>
        /// <param name="MinWalking"></param>
        /// <param name="MaxWalking"></param>
        /// <param name="MinRuminating"></param>
        /// <param name="MaxRuminating"></param>
        /// <param name="Drinking"></param>
        /// <param name="MinMilking"></param>
        /// <param name="MaxMilking"></param>
        //public async void ActivityRule(decimal MinEating =3 , decimal MaxEating =5 , decimal MinSitting =12 , decimal MaxSitting =14,
        //    decimal MinWalking =2 , decimal MaxWalking =3 , decimal MinRuminating =7 , decimal MaxRuminating = 10 , decimal Drinking =1,
        //    decimal MinMilking =3 , decimal MaxMilking =4)
        //{
        //    try
        //    {
        //        foreach (var farm in db.Farms)
        //        {
        //            var relatedNotice = db.Notifications.Where(n => n.Rule == RuleTypes.Activity && n.FarmId == farm.ID).ToList();
        //            var users = db.Users.Where(u => u.FarmID == farm.ID).ToList();
        //            var LowEat = db.ActivityStates.Include("Cattle").Where(t => t.Eating<MinEating && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var AlotEat = db.ActivityStates.Include("Cattle").Where(t => t.Eating > MaxEating && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var LowSit = db.ActivityStates.Include("Cattle").Where(t => t.Sitting < MinSitting && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var AlotSit = db.ActivityStates.Include("Cattle").Where(t => t.Sitting >MaxSitting && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var LowWalk = db.ActivityStates.Include("Cattle").Where(t => t.Walking < MinWalking && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var AlotWalk = db.ActivityStates.Include("Cattle").Where(t => t.Walking > MaxWalking && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var LowRuminate = db.ActivityStates.Include("Cattle").Where(t => t.Rumination < MinRuminating && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var AlotRuminate = db.ActivityStates.Include("Cattle").Where(t => t.Rumination > MaxRuminating && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var LowDrink = db.ActivityStates.Include("Cattle").Where(t => t.Drinking < Drinking && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            var AlotDrink = db.ActivityStates.Include("Cattle").Where(t => t.Sitting > Drinking && t.date >= noticePeriod && t.FarmID == farm.ID).ToList();
        //            foreach (var user in users)
        //            {
        //                string roleid = user.Roles.FirstOrDefault() != null ? user.Roles.FirstOrDefault().RoleId : "";
        //                RoleNotification relatedRole = db.RoleNotifications.FirstOrDefault(r => r.RoleId == roleid);

        //                foreach (var notice in relatedNotice)
        //                {
        //                    UserNotification obj = new UserNotification();
        //                    #region generate notification
        //                    foreach (var temp in LowEat)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Eating less than : " + MinEating+" hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in AlotEat)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Eating more than : " + MaxEating + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in LowSit)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Sitting less than : " + MinSitting + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in AlotSit)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Sitting more than : " + MaxSitting + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in LowWalk)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Walking less than : " + MinWalking + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in AlotWalk)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Walking more than : " + MaxWalking + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in LowRuminate)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Ruminating less than : " + MinRuminating + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in AlotRuminate)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Ruminating more than : " + MaxRuminating + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in LowDrink)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Drinking less than : " + Drinking + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    foreach (var temp in AlotDrink)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = temp.date;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Drinking more than : " + Drinking + " hours";
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}
        #endregion
        #region Heat Rule
        /// <summary>
        /// the rule for Heat notifications
        /// </summary>
        //public async void HeatRule()
        //{
        //    try
        //    {
        //        foreach (var farm in db.Farms)
        //        {
        //            var relatedNotice = db.Notifications.Where(n => n.Rule == RuleTypes.Heat && n.FarmId == farm.ID);
        //            var users = db.Users.Where(u => u.FarmID == farm.ID);
        //            var cattles = db.Cattles.Include("ActivityState").Where(t => t.FarmID == farm.ID && t.ActivityState.Average(p => p.).Average(;
        //            foreach (var user in users)
        //            {
        //                RoleNotification relatedRole = db.RoleNotifications.FirstOrDefault(r => r.RoleId == user.Roles.FirstOrDefault().RoleId);
        //                foreach (var notice in relatedNotice)
        //                {
        //                    UserNotification obj = new UserNotification();
        //                    foreach (var temp in cattles)
        //                    {
        //                        obj.UserId = user.Id;
        //                        obj.NotificationId = notice.ID;
        //                        obj.Date = DateTime.Now;
        //                        obj.Notification.Content = temp.Cattle != null ? temp.Cattle.animalNumber.ToString() : "unknow Cattle" +
        //                            " has Temprature : " + temp.value.ToString();
        //                        obj.Seen = false;
        //                        obj.Received = false;
        //                        obj.priority = relatedRole != null ? relatedRole.priority : 0;
        //                        db.UserNotifications.Add(obj);
        //                    }
        //                }
        //            }
        //        }
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion
        public void Dispose()
        {
            //db.Dispose();
            //db.Dispose();
        }
    }
}
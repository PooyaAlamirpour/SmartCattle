using Elmah;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Web.Areas.BackOffice.CustomFilter;
using SmartCattle.Web.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartCattle.Web.Areas.BackOffice.Models;
using SmartCattle.Service;

namespace SmartCattle.Web.Areas.BackOffice.Controllers
{
    [SuperAdmin]
    public class MainController : Controller
    {
        // GET: BackOffice/Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewFarm()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewFarm(FormCollection form)
        {
            //string farmName = form["farmName"];
            //string email = form["email"];
            //string lat = form["lat"];
            //string lng = form["lng"];
            //string sensors = form["sensors"];

            //using (SmartCattleContext ctx = new SmartCattleContext())
            //{
            //    DomainClass.Farm farm = new Farm() { email = email, name = farmName, Latitude = lat, Longitude = lng };
            //    ctx.Farms.Add(farm);
            //    if (ctx.saveChanges() > 0)
            //    {
            //        bool res = await simpleRegister(email, farm.ID);
            //        if (res)
            //        {
            //            string[] sensorlist = sensors.Split(',');
            //            foreach (var item in sensorlist)
            //            {
            //                Sensor sensor = new Sensor();
            //                sensor.FarmID = farm.ID;
            //                sensor.MacAddress = item;
            //                ctx.Sensors.Add(sensor);
            //            }
            //            if (ctx.saveChanges() > 0)
            //            {
            //                ViewBag.msg = "farm set successfully";
            //            }
            //            else
            //            {
            //                ViewBag.msg = "error in set sensors";
            //            }
            //        }
            //        else
            //        {
            //            ctx.Farms.Remove(farm);
            //            ctx.saveChanges();
            //            ViewBag.msg = "Error in save user";
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.msg = "Error in save Farm";
            //    }
            //}
            return View();
        }

        //[HttpGet]
        //public ActionResult FarmList()
        //{
        //    SmartCattleContext db = new SmartCattleContext();
        //    return View(db.Farms.ToList());
        //}

        //[HttpGet]
        //public ActionResult FarmList()
        //{
        //    //SmartCattleContext db = new SmartCattleContext();
        //    //FarmListModel model = new FarmListModel();
        //    //FarmListModel model = new FarmListModel()
        //    //{

        //    //    Farms = db.Farms.ToList(),
        //    //    FarmSensors = db.Sensors.GroupBy(r => r.FarmID)
        //    //};

        //    //var reports = db.Sensors.GroupBy(r => r.FarmID).Select(r => new
        //    //     {
        //    //         FarmID = r.Key,
        //    //         FarmSensorCount = r.Count()
        //    //     });
          


        //    return View(model);
        //}

        [HttpGet]
        public ActionResult CreateRolePermission()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRolePermission(CreateRolePermissionModel model)
        {
            //SmartCattleContext db = new SmartCattleContext();
            //if (ModelState.IsValid)
            //{
            //    RolePermission RP = new RolePermission()
            //    {
            //        Action = model.Action,
            //        Controller = model.Controller,
            //        Description = model.Description,
            //        Title = model.Title,
            //        Read = model.Read,
            //        Write = model.Write
            //    };
            //    db.RolePermissions.Add(RP);
            //    if (db.saveChanges() <= 0)
            //    {
            //        ViewBag.msg = "add Role Permission Failed";
            //    }
            //}
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string RemovePermission(int id)
        //{
        //    //try
        //    //{
        //    //    using (SmartCattleContext db = new SmartCattleContext())
        //    //    {
        //    //        db.RolePermissions.Remove(db.RolePermissions.FirstOrDefault(r => r.ID == id));
        //    //        if (db.saveChanges() > 0)
        //    //        {
        //    //            return "success";
        //    //        }
        //    //        return "fialed";
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return "fialed";
        //    //}
        //}

        /// <summary>
        /// download Farm Map for specific farm
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public JsonResult GetFarmMap(GetFarmMapParam param)
        //{
        //    string targetpath = Server.MapPath("~/FarmsMaps/");
        //    string url = Setting.GetZoneMap + "?tokenId=" + param.tokenId + "&subProjectId=" + param.subProjectId;
        //    ActionMessage msg = new ActionMessage();
        //    string filename = targetpath + Guid.NewGuid() + ".json";
        //    HttpWebRequest httpRequest = (HttpWebRequest)
        //    WebRequest.Create(url);
        //    httpRequest.Method = WebRequestMethods.Http.Get;
        //    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        //    Stream httpResponseStream = httpResponse.GetResponseStream();

        //    // create and open a FileStream, using calls dispose when done
        //    using (var fs = System.IO.File.Create(filename))
        //    {
        //        // Copy all bytes from the responsestream to the filestream
        //        httpResponseStream.CopyTo(fs);
        //    }
        //    using (SmartCattleContext db = new SmartCattleContext())
        //    {
        //        var farm = db.Farms.Where(f => f.ID == param.subProjectId).FirstOrDefault();
        //        if (farm != null)
        //        {
        //            farm.MapFilePath = filename;
        //            if (db.saveChanges() > 0)
        //            {
        //                msg.title = "map download successfully";
        //                msg.type = messageType.success;
        //            }
        //            else
        //            {
        //                msg.title = "map download failed";
        //                msg.type = messageType.error;
        //            }
        //        }
        //        else
        //        {
        //            msg.title = "unspecified Farm !";
        //            msg.type = messageType.error;
        //        }
        //    }
        //    return Json(msg);
        //}

        //public PartialViewResult PermisionList()
        //{
        //    SmartCattleContext db = new SmartCattleContext();
        //    return PartialView(db.RolePermissions.ToList());
        //}

        //register Admin user for new Farm
        public async Task<bool> simpleRegister(string Email, int FarmID)
        {
            try
            {
                SmartCattleContext db = new SmartCattleContext();
                var UserManager = HttpContext.GetOwinContext().GetUserManager<SmartCattleUserManager>();
                string Password = System.Web.Security.Membership.GeneratePassword(12, 1);
                var user = new SmartCattleUser { UserName = Email, Email = Email, FarmID = FarmID };
                var result = await UserManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    #region insert role manually - by pass duplicate role name constraint
                    object[] parametes = new object[5];
                    Guid RoleId = Guid.NewGuid();
                    string query = @"INSERT INTO [SmartCattle].[AspNetRoles]
                           (Id
                           ,Name
                           ,Description
                           ,FarmID
                           ,Discriminator)
                     VALUES (@id,@name,@desc,@farmId,@Discriminator)";
                    parametes[0] = new SqlParameter("@id", RoleId);
                    parametes[1] = new SqlParameter("@name", "Admin");
                    parametes[2] = new SqlParameter("@desc", "");
                    parametes[3] = new SqlParameter("@farmId", FarmID);
                    parametes[4] = new SqlParameter("@Discriminator", "UserRole");
                    db.Database.ExecuteSqlCommand(query, parametes);
                    #endregion
                    #region insert User-Role relation manually
                    string relationQuery = @"INSERT INTO [SmartCattle].[AspNetUserRoles]
                           (UserId
                           ,RoleId)
                     VALUES (@UserId,@RoleId)";
                    parametes = new object[2];
                    parametes[0] = new SqlParameter("@UserId", user.Id);
                    parametes[1] = new SqlParameter("@RoleId", RoleId);

                    db.Database.ExecuteSqlCommand(relationQuery, parametes);
                    #endregion


                    await UserManager.SendEmailAsync(user.Id, "Account info", "username :" + Email + "  password : " +
                            Password + "  Rest passowrd after first login ");

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
    public class CreateRolePermissionModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Controller { get; set; }

        [Required]
        public string Action { get; set; }

        public string Description { get; set; }
        public bool Write { get; set; }
        public bool Read { get; set; }
    }
}

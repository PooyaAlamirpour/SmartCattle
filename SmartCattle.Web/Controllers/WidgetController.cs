using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.Web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Net;
using NHibernate;
using SmartCattle.Web.Domain;

namespace SmartCattle.Web.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class WidgetController : Controller
    {
        BaseServices<Cattle> cattleService;
        BaseServices<CattleTempreture> TemperatureService;
        BaseServices<CattleActivityState> ActivityService;
        BaseServices<CattlePosition> PositionService;

        public WidgetController(BaseServices<Cattle> cattleService, BaseServices<CattleTempreture> TemperatureService
            , BaseServices<CattleActivityState> ActivityService , BaseServices<CattlePosition> PositionService)
        {
            this.cattleService = cattleService;
            this.TemperatureService = TemperatureService;
            this.ActivityService = ActivityService;
            this.PositionService = PositionService;
        }
        
        public async Task<ActionResult> TempretureWidget(int CattleId, int FreeStallId, string from , string to)
        {
            //SmartCattleContext db = new SmartCattleContext();
            ISession mContext = Context.Open();
            int FarmId = Helper.Helper.getCurrentFarmId();
            List<SensorTbl> cattles = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == CattleId).Where(x => x.FarmID == FarmId).List().ToList();
            Context.Close(mContext);

            //List<Sensor> cattles = db.Sensors.Where(x => x.cattleId == CattleId).ToList();
            if (cattles.Count != 0)
            {
                ViewBag.MAC = cattles[0].MacAddress;
            }
            else
            {
                ViewBag.MAC = 0;
            }

            return PartialView();
        }

        public async Task<ActionResult> TimebudgetWidget(int CattleId, int FreeStallId, string from, string to)
        {
            //SmartCattleContext db = new SmartCattleContext();
            ISession mContext = Context.Open();
            List<SensorTbl> cattles = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == CattleId).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            //List<Sensor> cattles = db.Sensors.Where(x => x.cattleId == CattleId).ToList();
            if (cattles.Count != 0)
            {
                ViewBag.MAC = cattles[0].MacAddress;
            }
            else
            {
                ViewBag.MAC = 0;
            }

            return PartialView();
        }

        public async Task<ActionResult> CattleActivityWidget(int CattleId, String IsEmpty)
        {
            //////////////////////////////
            //var Position = (await PositionService.List(p => p.cattleId == CattleId)).OrderByDescending(p => p.ID);
            //SmartCattleContext db = new SmartCattleContext();
            ISession mContext = Context.Open();
            List<SensorTbl> cattles = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == CattleId).Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            if (cattles.Count != 0)
            {
                ViewBag.MAC = cattles[0].MacAddress;
                ViewBag.IsEmpty = IsEmpty;
            }
            else
            {
                ViewBag.MAC = 0;
                ViewBag.IsEmpty = IsEmpty;
            }
            /////////////////////////////

            return PartialView();
        }

        public async Task<ActionResult> CattlePositionWidget(int cattleId)
        {
            //SmartCattleContext db = new SmartCattleContext();
            ISession mContext = Context.Open();
            List<SensorTbl> cattles = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == cattleId).List().ToList();
            if(cattles.Count != 0)
            {
                ViewBag.MAC = cattles[0].MacAddress;
            }
            else
            {
                ViewBag.MAC = 0;
            }
            Context.Close(mContext);

            return PartialView();         
        }

    }

    public class TemperatureViweModel
    {
      public  IEnumerable<CattleTempreture> Cattletemps { get; set; }
      public IEnumerable<CattleTempreture> FreeStallTemps { get; set; }
      public int cattleId { set; get; }
      public int freestallId { set; get; }
      public string from { get; set; }
      public string to { get; set; }
    }

    public class CattleActivityviewModel
    {
        public IEnumerable<CattleActivityState> activities;
        public int cattleId { set; get; } 
        public string from { get; set; }
        public string to { get; set; }
    }

    public class DataValue
    {
        public double Value { set; get; }
    }


}
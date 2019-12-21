using SmartCattle.Core;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class TimeBudgetController : BaseController
    {
        BaseServices<GroupTimeBudget> TimeBudgetService;
        BaseServices<GroupTimeBudgetItem> TimeBudgetItemService;
        BaseServices<CattleActivityState> ActivityService;
        //BaseServices<CattleGroup> CattleGroupService;
        public TimeBudgetController(BaseServices<GroupTimeBudget> TimeBudgetService, BaseServices<GroupTimeBudgetItem> TimeBudgetItemService,
              BaseServices<CattleActivityState> ActivityService, BaseServices<CattleGroup> CattleGroupService)
        {
            this.TimeBudgetService = TimeBudgetService;
            this.TimeBudgetItemService = TimeBudgetItemService;
            this.ActivityService = ActivityService;
            //this.CattleGroupService = CattleGroupService;
        }
        public async Task<ActionResult> Index()
        {              
            var model = await TimeBudgetService.List(t => t.FarmID == farmID); 
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> CreateGroupTimeBudget()
        {
            IEnumerable<GroupTimeBudget> groupTimeBudgets = await TimeBudgetService.List(t => t.FarmID == farmID);
            //IEnumerable<CattleGroup> groups = await CattleGroupService.List(g => g.FarmID == farmID);
            TimeBudgetViweModel VM = new TimeBudgetViweModel();
            //VM.Groups = groups;
            VM.timeBudgets = groupTimeBudgets;
            return View(VM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroupTimeBudget(int CattleGroupId,string Title ,seasons season)
        {
            GroupTimeBudget TimeBudget = new GroupTimeBudget();
            TimeBudget.CattleGroupId = CattleGroupId;
            TimeBudget.season = season;
            TimeBudget.Title = Title;
            TimeBudget.FarmID = farmID;
            TimeBudget.UserId = userID;
            TimeBudget.date = DateTime.Now;
            ActionMessage msg = await TimeBudgetService.Insert(TimeBudget);
            IEnumerable<GroupTimeBudget> groupTimeBudgets = await TimeBudgetService.List(t => t.FarmID == farmID);
            //IEnumerable<CattleGroup> groups = await CattleGroupService.List(g => g.FarmID == farmID);
            TimeBudgetViweModel VM = new Controllers.TimeBudgetViweModel();
            //VM.Groups = groups;
            VM.timeBudgets = groupTimeBudgets;
            ViewBag.msg = msg;
            return View(VM);
        }
     [HttpPost]
     [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(int TimebudgetId , string Description, double Percent , TimeBudgetItem TimeBudgetItem)
        {
            var entity = new GroupTimeBudgetItem() { TimeBudgetId = TimebudgetId, UserId = userID, Item = TimeBudgetItem, description = Description ,FarmID=farmID, valuePercent=Percent };
            ActionMessage msg = await TimeBudgetItemService.Insert(entity);
            ViewBag.msg = msg;
            var model = TimeBudgetService.GetById(TimebudgetId);
            return View(model);
        }
        [HttpGet]
        public  ActionResult Detail(int Id)
        {
            var model = TimeBudgetService.GetById(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int ID)
        {
            GroupTimeBudget entity = TimeBudgetService.GetById(ID);
            ActionMessage msg = await TimeBudgetService.Delete(entity);
            return Json(msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteTimeBudgetItem(int ID)
        {
            GroupTimeBudgetItem entity = TimeBudgetItemService.GetById(ID);
            ActionMessage msg = await TimeBudgetItemService.Delete(entity);
            return Json(msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(int ID , string title , seasons season)
        {
            GroupTimeBudget entity = TimeBudgetService.GetById(ID);
            entity.Title = title;
            entity.season = season;
            ActionMessage msg = await TimeBudgetService.Update(entity);
            return Json(msg);
        }

        public GroupTimeBudget CalculateTimeBudget(int cattleGroupId , int farmId , DateTime? date)
        {
            GroupTimeBudget _timeBudget = new GroupTimeBudget();
            if (date == null)
                date = DateTime.Now;
            return _timeBudget;
        }
    }

    public class GroupTimeBudgetViweModel
    {
        public IEnumerable<GroupTimeBudget> StandardTimeBudgets { get; set; }
        public IEnumerable<GroupTimeBudgetItem> items { get; set;}
    }
    public class CreateGroupTimeBudgetViweModel
    {
        public string Title { get; set; }
        public seasons Seasson { get; set; }
        public int CattleGroupId { get; set; }
        //public IEnumerable<CattleGroup> Groups { get; set; }
    }
    public class TimeBudgetViweModel
    {
        //public IEnumerable<CattleGroup> Groups { get; set; }
        public IEnumerable<GroupTimeBudget> timeBudgets { set; get; }
    }



}
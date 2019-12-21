using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartCattle.DomainClass;
using SmartCattle.DataAccess;
using SmartCattle.Web.Models.ViewModels;
using SmartCattle.Service;
using SmartCattle.Core;
using System.Threading.Tasks;
using NHibernate;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Domain;

namespace SmartCattle.Web.Controllers
{
    [Authorize]
    public class CattleGroupController : BaseController
    {
        public CattleGroupController(BaseServices<CattleGroup> services)
        {

        }
        // GET: CattleHerd
        public ActionResult Index()
        {

            return View();
        }
        public async Task<ActionResult> List()
        {
            List<CattleGroupTbl> CattleGroupList = new List<CattleGroupTbl>();
            ISession mContext = Context.Open();
            CattleGroupList = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            List<CattleGroupTbl> DefaultGroup = mContext.QueryOver<CattleGroupTbl>().Where(x => x.FarmID == -1).List().ToList();
            CattleGroupList.AddRange(DefaultGroup);
            Context.Close(mContext);

            for (int i = 0; i < CattleGroupList.Count; i++)
            {
                CattleGroupList[i].name = Localization.getString(CattleGroupList[i].name);
            }

            return View(CattleGroupList);
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public String DeleteGroup(int GroupId)
        {
            String retvalue = "NaN";
            ISession mContext = Context.Open();
            List<CattleTbl> CattleList = mContext.QueryOver<CattleTbl>().Where(x => x.CattleGroupId == GroupId).List().ToList();
            if(CattleList.Count == 0)
            {
                String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleGroupTbl", GroupId);
                mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();
                retvalue = "OK";
            }
            else
            {
                retvalue = "SIMILAR";
            }
            
            Context.Close(mContext);

            return retvalue;
        }

        [HttpPost]
        public async Task<JsonResult> CreateGroup(CattleGroupViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.GroupId == null)
                {
                    ISession mContext = Context.Open();
                    CattleGroupTbl _CattleGroupTbl = new CattleGroupTbl()
                    {
                        name = model.name,
                        Description = model.description,
                        FarmID = Helper.Helper.getCurrentFarmId(),
                        UserName = Helper.Helper.getCurrentUserNameFamily(),
                        UserIdentity = Helper.Helper.getCurrentUserId(),
                        date = DateTime.Now
                    };
                    mContext.Save(_CattleGroupTbl);
                    Context.Close(mContext);
                    return Json(new ActionMessage() { type = messageType.success });
                }
                else
                {
                    ISession mContext = Context.Open();
                    CattleGroupTbl _CattleGroupTbl = mContext.Get<CattleGroupTbl>(model.GroupId);
                    _CattleGroupTbl.Description = model.description;
                    _CattleGroupTbl.name = model.name;
                    _CattleGroupTbl.UserName = Helper.Helper.getCurrentUserNameFamily();
                    _CattleGroupTbl.UserIdentity = Helper.Helper.getCurrentUserId();
                    _CattleGroupTbl.date = DateTime.Now;
                    mContext.Update(_CattleGroupTbl);
                    mContext.Flush();

                    Context.Close(mContext);

                    return Json(new ActionMessage() { type = messageType.success });
                }
            }
            return Json(new ActionMessage() { type = messageType.error });
        }
    }
}
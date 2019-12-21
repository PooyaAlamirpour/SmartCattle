using NHibernate;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class CattleHerdController : BaseController
    {
        public CattleHerdController(BaseServices<CattleHerd> services)
        {

        }

        // GET: CattleHerd
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> List()
        {
            ISession mContext = Context.Open();
            List<CattleHerds> CattleHerdsList = mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            List<CattleHerds> DefaultCattleHerdsList = mContext.QueryOver<CattleHerds>().Where(x => x.FarmID == -1).List().ToList();
            CattleHerdsList.AddRange(DefaultCattleHerdsList);
            Context.Close(mContext);

            return View(CattleHerdsList);
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public String DeleteHerd(int HerdId)
        {
            String retvalue = "NaN";
            ISession mContext = Context.Open();
            List<CattleTbl> CattleList = mContext.QueryOver<CattleTbl>().Where(x => x.CattleHerd_ID == HerdId).List().ToList();
            if (CattleList.Count == 0)
            {
                String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.CattleHerds", HerdId);
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
        public async Task<JsonResult> CreateHerd(CattleHerdViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.HerdId == null)
                {
                    ISession mContext = Context.Open();
                    try
                    {
                        CattleHerds newCattleHerds = new CattleHerds()
                        {
                            name = model.name,
                            Description = model.description,
                            FarmID = Helper.Helper.getCurrentFarmId(),
                            UserName = Helper.Helper.getCurrentUserNameFamily(),
                            UserIdentity = Helper.Helper.getCurrentUserId(),
                            date = DateTime.Now
                        };
                        mContext.Save(newCattleHerds);
                    }
                    catch (Exception ex)
                    {
                        String ACK = ex.Message;
                    }
                    Context.Close(mContext);

                    return Json(new ActionMessage() { type = 0 });
                }
                else
                {
                    ISession mContext = Context.Open();
                    CattleHerds _CattleHerds = mContext.Get<CattleHerds>(model.HerdId);
                    _CattleHerds.name = model.name;
                    _CattleHerds.Description = model.description;
                    _CattleHerds.UserName = Helper.Helper.getCurrentUserNameFamily();
                    _CattleHerds.UserIdentity = Helper.Helper.getCurrentUserId();
                    _CattleHerds.date = DateTime.Now;
                    mContext.Update(_CattleHerds);
                    mContext.Flush();
                    Context.Close(mContext);

                    return Json(new ActionMessage() { type = 0 });
                }
            }
            return Json( new ActionMessage() { type = messageType.error });
        }
    }
}

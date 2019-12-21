using Newtonsoft.Json;
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartCattle.Web.CustomFilters;

namespace SmartCattle.Web.Controllers
{

    public class SensorController : BaseController
    {
        const int ItemPerPage = 15;
        BaseServices<Sensor> services;
        BaseServices<Cattle> CattleService;
        public SensorController(BaseServices<Sensor> services, BaseServices<Cattle> CattleService)
        {
            this.services = services;
            this.CattleService = CattleService;
        }
		
        [AuthenticateFilter]
        public async Task<ActionResult> List()
        {
            IEnumerable<Sensor> Sensors = await services.List();
            return View(Sensors);
        }
		
        [AuthenticateFilter]
        public async Task<ActionResult> Empty()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteSensor(int SensorID)
        {
            return Json(services.DeleteById(SensorID));
        }

        [HttpPost]
        public async Task<JsonResult> CreateSensor(SensorViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.sensorId == null)
                {
                    Sensor sensor = new Sensor() { softwareVersion = model.softwareVersion, MacAddress = model.macAddress.Trim().Replace(" ", ""), FarmID = farmID, UserId = userID, cattleId = 0 };
                    return Json(await services.Insert(sensor));
                }
                else
                {
                    Sensor sensor = services.GetById(model.sensorId);
                    sensor.MacAddress = model.macAddress;
                    sensor.softwareVersion = model.softwareVersion;
                    return Json(await services.Update(sensor));
                }
            }
            return Json(new ActionMessage() { type = messageType.error });
        }

        [HttpGet]
        public async Task<ActionResult> AssignToCattle(int page = 1)
        {
            AssignSensorModel model = new AssignSensorModel();
            model = LoadSensorsList(page);
            return View(model);
        }

        public AssignSensorModel LoadSensorsList(int page = 1)
        {
            ISession mContext = Context.Open();
            List<CattleTbl> Cattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            int CurrentSubId = Helper.Helper.getCurrentSubId();
            int CurrentFarmId = Helper.Helper.getCurrentFarmId();
            if (CurrentFarmId != -1)
            {
                List<EquipmentTbl> EquipmentList = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == CurrentSubId).List().ToList();
                foreach (var item in EquipmentList)
                {
                    if (item.DeviceType.Equals("SensorTag"))
                    {
                        List<SensorTbl> isSensorExist = mContext.QueryOver<SensorTbl>().Where(x => x.MacAddress == item.Mac).List().ToList();
                        if (isSensorExist.Count == 0)
                        {
                            //String tmp = Helper.Helper.getDateJustGeorgian(DateTime.Now);
                            //DateTime tmpdate = DateTime.ParseExact(tmp, "HH:mm:ss yyyy/MM/dd", new CultureInfo("en-US"));
                            mContext.Clear();
                            SensorTbl newSensor = new SensorTbl()
                            {
                                MacAddress = item.Mac,
                                FarmID = CurrentFarmId
                            };
                            mContext.Save(newSensor);
                        }
                    }
                }
            }

            List<SensorTbl> Sensors = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            List<SensorView> _SensorView = new List<SensorView>();
            int SubId = Helper.Helper.getCurrentSubId();
            foreach (var sensor in Sensors)
            {
                SensorView item = new SensorView();

                item.ID = sensor.ID;
                item.MacAddress = sensor.MacAddress;
                item.cattleId = sensor.cattleId;
                item.FarmID = sensor.FarmID;
                int Equipmentid = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == SubId).Where(x => x.Mac == sensor.MacAddress).Select(x => x.Equipmentid).SingleOrDefault<int>();
                item.Equipmentid = Equipmentid;

                _SensorView.Add(item);
            }
            int farmId = Helper.Helper.getCurrentFarmId();
            List<SensorTbl> PagedSensors = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == farmId).Skip((page - 1) * ItemPerPage).Take(ItemPerPage).List().ToList();
            List<SensorView> _PagedSensorsView = new List<SensorView>();
            foreach (var sensor in PagedSensors)
            {
                SensorView item = new SensorView();

                item.ID = sensor.ID;
                item.MacAddress = sensor.MacAddress;
                item.cattleId = sensor.cattleId;
                item.FarmID = sensor.FarmID;
                int Equipmentid = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == SubId).Where(x => x.Mac == sensor.MacAddress).Select(x => x.Equipmentid).SingleOrDefault<int>();
                item.Equipmentid = Equipmentid;

                _PagedSensorsView.Add(item);
            }
            Context.Close(mContext);

            AssignSensorModel model = new AssignSensorModel();
            model.Cattles = Cattles;
            model.Sensors = _SensorView;
            model.PagedSensors = _PagedSensorsView;
            model.current = page;

            model.pages = (int)((double)Sensors.Count / (double)ItemPerPage);
            if (page % ItemPerPage != 0)
                model.pages++;

            //////////////////////////////////////////////////////////////////
            //List<SensorTbl> TotalSensorList = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == farmId).List().ToList();
            //List<CattleTbl> TotalCattleList = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == farmId).List().ToList();

            //foreach (var item in TotalSensorList)
            //{
            //    if (item.cattleId == 0)
            //    {
            //        SensorView _SensorViewTmp = new SensorView();

            //        _SensorViewTmp.cattleId = item.cattleId;
            //        _SensorViewTmp.ID = item.ID;
            //        _SensorViewTmp.MacAddress = item.MacAddress;

            //        model.Sensors.Add(_SensorViewTmp);
            //    }
            //    else
            //    {
            //        CattleTbl tmpCattleTbl = TotalCattleList.Where(x => x.ID == item.cattleId).SingleOrDefault();
            //        TotalCattleList.Remove(tmpCattleTbl);
            //    }
            //}

            //Context.Close(mContext);

            //model.Cattles = TotalCattleList;
            //////////////////////////////////////////////////////////////////

            return model;
        }

        [HttpPost]
        public async Task<PartialViewResult> paging(String[] AllFields, int page = 1)
        {
            AssignSensorModel model = AdvanceSensorSearch(AllFields, page);

            model.current = page;
            model.pages = (int)((double)model.Sensors.Count / (double)ItemPerPage);
            if (page % ItemPerPage != 0)
                model.pages++;

            return PartialView("paging", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnassignSensor(int ID, int page = 1)
        {
            ISession mContext = Context.Open();
            mContext.Clear();
            SensorTbl CurrentSensor = mContext.QueryOver<SensorTbl>().Where(x => x.ID == ID).SingleOrDefault();
            if(CurrentSensor != null)
            {
                CurrentSensor.cattleId = 0;
                mContext.Update(CurrentSensor);
                mContext.Flush();
            }
            Context.Close(mContext);

            AssignSensorModel model = LoadSensorsList(page);
            return PartialView("AdvanceSearchOnSensor", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> AssignToCattle(int SensorId, int CattleId, int page = 1)
        {
            ISession mContext = Context.Open();
            List<SensorTbl> CurrentSensor = mContext.QueryOver<SensorTbl>().Where(x => x.ID == SensorId).List().ToList();
            if(CurrentSensor.Count != 0)
            {
                CurrentSensor[0].cattleId = CattleId;
                mContext.Update(CurrentSensor[0]);
                mContext.Flush();
            }
            Context.Close(mContext);

            AssignSensorModel model = LoadSensorsList(page);
            return PartialView("AdvanceSearchOnSensor", model);
        }

        [HttpPost]
        public JsonResult SyncMenuAgain(int page = 1)
        {
            AssignSensorModel model = new AssignSensorModel();
            model.Sensors = new List<SensorView>();
            model.Cattles = new List<CattleTbl>();

            ISession mContext = Context.Open();
            int farmId = Helper.Helper.getCurrentFarmId();
            List<SensorTbl> TotalSensorList = mContext.QueryOver<SensorTbl>().Where(x => x.FarmID == farmId).List().ToList();
            List<CattleTbl> TotalCattleList = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == farmId).List().ToList();

            foreach (var item in TotalSensorList)
            {
                if (item.cattleId == 0)
                {
                    SensorView _SensorView = new SensorView();

                    _SensorView.cattleId = item.cattleId;
                    _SensorView.ID = item.ID;
                    _SensorView.MacAddress = item.MacAddress;

                    model.Sensors.Add(_SensorView);
                }
                else
                {
                    CattleTbl tmpCattleTbl = TotalCattleList.Where(x => x.ID == item.cattleId).SingleOrDefault();
                    TotalCattleList.Remove(tmpCattleTbl);
                }
            }

            Context.Close(mContext);

            model.Cattles = TotalCattleList;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public class EquipmentView
        {
            public int ID { get; set; }
            public string DeviceCategory { get; set; }
            public string projectName { get; set; }
            public int subId { get; set; }
            public string DeviceType { get; set; }
            public string DeviceSubtype { get; set; }
            public string PacketType { get; set; }
            public string Version { get; set; }
            public string PowerType { get; set; }
            public int Equipmentid { get; set; }
            public string Mac { get; set; }
            public string Projectid { get; set; }
            public string Zoneid { get; set; }
            public string Locationx { get; set; }
            public string Locationy { get; set; }
            public string Locationz { get; set; }
            public string Date1 { get; set; }
            public string Date2 { get; set; }
            public String BatteryLevel { get; set; }
            public String Reserved1 { get; set; }
            public String CattleNumber { get; set; }
        }

        public class AssignSensorModel
        {
            public List<CattleTbl> Cattles { get; set; }
            public List<SensorView> Sensors { get; set; }
            public List<SensorView> PagedSensors { get; set; }
            public int pages { get; set; }
            public int current { get; set; }
        }

        [AuthenticateFilter]
        public async Task<ActionResult> Report()
        {
            List<EquipmentTbl> _EquipmentTbl = new List<EquipmentTbl>();
            List<EquipmentView> retValue = new List<EquipmentView>();

            ISession mContext = Context.Open();
            int CurrentSubId = Helper.Helper.getCurrentSubId();
            if (CurrentSubId != -1 && CurrentSubId != -6)
            {
                _EquipmentTbl = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == CurrentSubId).OrderBy(x => x.Equipmentid).Asc.List().ToList();
            }
            foreach (var Equipment in _EquipmentTbl)
            {
                EquipmentView item = new EquipmentView();

                item.ID = Equipment.ID;
                item.DeviceCategory = Equipment.DeviceCategory;
                item.projectName = Equipment.projectName;
                item.subId = Equipment.subId;
                item.DeviceType = Equipment.DeviceType;
                item.DeviceSubtype = Equipment.DeviceSubtype;
                item.PacketType = Equipment.PacketType;
                item.Version = Equipment.Version;
                item.PowerType = Equipment.PowerType;
                item.Equipmentid = Equipment.Equipmentid;
                item.Mac = Equipment.Mac;
                item.Projectid = Equipment.Projectid;
                item.Zoneid = Equipment.Zoneid;
                item.Locationx = Equipment.Locationx;
                item.Locationy = Equipment.Locationy;
                item.Locationz = Equipment.Locationz;
                item.Date1 = Equipment.Date1;
                item.Date2 = Equipment.Date2;
                BatteryLevelTbl BatteryLevel = mContext.QueryOver<BatteryLevelTbl>().Where(x => x.MacAddress == Equipment.Mac)
                    .OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault<BatteryLevelTbl>();
                item.BatteryLevel = BatteryLevel == null ? "" : BatteryLevel.BatteryLevel.ToString();
                item.Reserved1 = Equipment.Reserved1;
                List<SensorTbl> CattleId = mContext.QueryOver<SensorTbl>().Where(x => x.MacAddress == Equipment.Mac).List().ToList();
                if(CattleId.Count != 0)
                {
                    if(CattleId[0].cattleId != 0)
                    {
                        CattleTbl Cattle = mContext.QueryOver<CattleTbl>().Where(x => x.ID == CattleId[0].cattleId).SingleOrDefault();
                        if (Cattle != null)
                        {
                            item.CattleNumber = Cattle.animalNumber.ToString();
                        }
                        else
                        {
                            item.CattleNumber = "Cattle was killed!";
                        }
                    }
                    else
                    {
                        item.CattleNumber = "Unassign";
                    }
                }

                retValue.Add(item);
            }
            Context.Close(mContext);

            return View(retValue);
        }

        public async Task<ActionResult> PagingReport(int page = 1)
        {
            ISession mContext = Context.Open();
            //List<EquipmentTbl> Model = mContext.QueryOver<EquipmentTbl>().Where(x => x.FarmId == Helper.Helper.getCurrentFarmId()).List().ToList();
            Context.Close(mContext);

            ReportPaging Model = new ReportPaging();
            Model.Current = 1;
            Model.Total = 1;
            Model.Pages = 1;
            return PartialView(Model);
        }

        private enum AllEquipmentFieldsname
        {
            ID_Input,
            DeviceType_Input,
            DeviceSubtype_Input,
            Version_Input,
            EquipmentId_Input,
            MAC_Input,
            LastUpdate_Input,
            Status_Input,
            CattleId_Input,
            InstallationDate_Input
        }

        private enum AllSensorFieldsname
        {
            page,
            SerialNumber_Input,
            MacAddress_Input,
            CattleId_Input
        }

        public async Task<PartialViewResult> Advancefilter(String[] AllFields)
        {
            ListModel model = new ListModel();
            int CurrentSubId = Helper.Helper.getCurrentSubId();
            IList<EquipmentTbl> AllEquipment = new List<EquipmentTbl>();
            List<EquipmentView> retValue = new List<EquipmentView>();
            bool hasExp = false;
            ISession mContext = null;
            try
            {
                mContext = Context.Open();
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
            }
            IQueryOver<EquipmentTbl, EquipmentTbl> query = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == CurrentSubId);

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.ID_Input]))
            {
                int ID_Input = Convert.ToInt32(AllFields[(int)AllEquipmentFieldsname.ID_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.ID == ID_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.DeviceType_Input]))
            {
                String DeviceType_Input = Convert.ToString(AllFields[(int)AllEquipmentFieldsname.DeviceType_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.DeviceType == DeviceType_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.DeviceSubtype_Input]))
            {
                String DeviceSubtype_Input = Convert.ToString(AllFields[(int)AllEquipmentFieldsname.DeviceSubtype_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.DeviceSubtype == DeviceSubtype_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.Version_Input]))
            {
                String Version_Input = Convert.ToString(AllFields[(int)AllEquipmentFieldsname.Version_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.Version == Version_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.EquipmentId_Input]))
            {
                int EquipmentId_Input = Convert.ToInt32(AllFields[(int)AllEquipmentFieldsname.EquipmentId_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.subId == CurrentSubId).Where(x => x.Equipmentid == EquipmentId_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.MAC_Input]))
            {
                String MAC_Input = Convert.ToString(AllFields[(int)AllEquipmentFieldsname.MAC_Input]);
                query = query.Where(x => x.subId == CurrentSubId).Where(x => x.Mac == MAC_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.LastUpdate_Input]))
            {
                //String LastUpdate_Input = Convert.ToString(AllFields[(int)AllFieldsname.LastUpdate_Input]);
                //query = query.Where(x => x.Date1 == LastUpdate_Input);
                //hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.Status_Input]))
            {
                //int Status_Input = Convert.ToInt32(AllFields[(int)AllFieldsname.Status_Input]);
                //query = query.Where(x => x.St == Status_Input);
                //hasExp = true;
            }

            bool hasCattle = false;
            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.CattleId_Input]))
            {
                int CattleId_Input = Convert.ToInt32(AllFields[(int)AllEquipmentFieldsname.CattleId_Input]);
                //query = query.Where(x => x.CattleID == CattleId_Input);
                int FarmId = Helper.Helper.getCurrentFarmId();
                List<CattleTbl> _Cattle = mContext.QueryOver<CattleTbl>().Where(x => x.animalNumber == CattleId_Input).Where(x => x.FarmID == FarmId).List().ToList();
                if(_Cattle.Count != 0)
                {
                    List<SensorTbl> _Sensor = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == _Cattle[0].ID).List().ToList();
                    if(_Sensor.Count != 0)
                    {
                        AllEquipment = mContext.QueryOver<EquipmentTbl>().Where(x => x.Mac == _Sensor[0].MacAddress).Where(x => x.subId == CurrentSubId).OrderBy(x => x.Equipmentid).Asc.List().ToList();
                    }
                }
                hasExp = false;
                hasCattle = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllEquipmentFieldsname.InstallationDate_Input]))
            {
                //int animalNumber = Convert.ToInt32(AllFields[(int)AllFieldsname.InstallationDate_Input]);
                //query = query.Where(x => x.animalNumber == animalNumber);
                //hasExp = true;
            }

            model.pages = model.total / ItemPerPage;
            if (model.total % ItemPerPage != 0)
            {
                model.pages++;
            }
            if (hasExp)
            {
                try
                {
                    AllEquipment = query.OrderBy(x => x.Equipmentid).Asc.List<EquipmentTbl>();
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
            }
            else
            {
                if(hasCattle)
                {

                }
                else
                {
                    
                    if (CurrentSubId != -1 && CurrentSubId != -6)
                    {
                        AllEquipment = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == CurrentSubId).OrderBy(x => x.Equipmentid).Asc.List<EquipmentTbl>();
                    }
                    else
                    {
                        AllEquipment = new List<EquipmentTbl>();
                    }
                }
            }

            model.Cattles = new List<CattleTbl>();
            if (AllEquipment != null)
            {
                for (int i = 0; i < AllEquipment.Count; i++)
                {

                    EquipmentView item = new EquipmentView();

                    item.ID = AllEquipment[i].ID;
                    item.DeviceCategory = AllEquipment[i].DeviceCategory;
                    item.projectName = AllEquipment[i].projectName;
                    item.subId = AllEquipment[i].subId;
                    item.DeviceType = AllEquipment[i].DeviceType;
                    item.DeviceSubtype = AllEquipment[i].DeviceSubtype;
                    item.PacketType = AllEquipment[i].PacketType;
                    item.Version = AllEquipment[i].Version;
                    item.PowerType = AllEquipment[i].PowerType;
                    item.Equipmentid = AllEquipment[i].Equipmentid;
                    item.Mac = AllEquipment[i].Mac;
                    item.Projectid = AllEquipment[i].Projectid;
                    item.Zoneid = AllEquipment[i].Zoneid;
                    item.Locationx = AllEquipment[i].Locationx;
                    item.Locationy = AllEquipment[i].Locationy;
                    item.Locationz = AllEquipment[i].Locationz;
                    item.Date1 = AllEquipment[i].Date1;
                    item.Date2 = AllEquipment[i].Date2;
                    item.Reserved1 = AllEquipment[i].Reserved1;
                    List<SensorTbl> CattleId = mContext.QueryOver<SensorTbl>().Where(x => x.MacAddress == AllEquipment[i].Mac).List().ToList();
                    if (CattleId.Count != 0)
                    {
                        if (CattleId[0].cattleId != 0)
                        {
                            CattleTbl Cattle = mContext.QueryOver<CattleTbl>().Where(x => x.ID == CattleId[0].cattleId).SingleOrDefault();
                            if (Cattle != null)
                            {
                                item.CattleNumber = Cattle.animalNumber.ToString();
                            }
                            else
                            {
                                item.CattleNumber = "Cattle was killed!";
                            }
                        }
                        else
                        {
                            item.CattleNumber = "Unassign";
                        }
                    }

                    retValue.Add(item);
                }
            }
            for (int i = 0; i < retValue.Count; i++)
            {
                String MacAddress = retValue[i].Mac;
                BatteryLevelTbl BatteryLevel = mContext.QueryOver<BatteryLevelTbl>().Where(x => x.MacAddress == MacAddress).OrderBy(x => x.Date).Desc.Take(1).SingleOrDefault<BatteryLevelTbl>();
                if(BatteryLevel != null)
                {
                    String _BatteryLevel = BatteryLevel.BatteryLevel.ToString();
                    retValue[i].BatteryLevel = _BatteryLevel == null ? "" : BatteryLevel.BatteryLevel.ToString();
                }
            }

            Context.Close(mContext);
            ////////////////////////////////////////////////////////////////////////////////////////////////
            return PartialView("filter", retValue);
        }
        
        public async Task<PartialViewResult> AdvanceSensorfilter(String[] AllFields, int page = 1)
        {
            AssignSensorModel model = AdvanceSensorSearch(AllFields, page);
            return PartialView("AdvanceSearchOnSensor", model);
        }

        public AssignSensorModel AdvanceSensorSearch(String[] AllFields, int page = 1)
        {
            IList<EquipmentTbl> AllSensors = new List<EquipmentTbl>();
            List<CattleTbl> AllCattles = new List<CattleTbl>();
            List<SensorView> _SensorView = new List<SensorView>();
            int CurrentSubId = Helper.Helper.getCurrentSubId();
            int CurrentFarmId = Helper.Helper.getCurrentFarmId();
            AssignSensorModel model = new AssignSensorModel();

            bool hasExp = false;
            ISession mContext = null;
            try
            {
                mContext = Context.Open();
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
            }
            IQueryOver<EquipmentTbl, EquipmentTbl> query_Sensor = mContext.QueryOver<EquipmentTbl>().Where(x => x.subId == CurrentSubId);
            IQueryOver<SensorTbl, SensorTbl> query_Cattle = mContext.QueryOver<SensorTbl>();

            if (!string.IsNullOrEmpty(AllFields[(int)AllSensorFieldsname.SerialNumber_Input]))
            {
                int SerialNumber_Input = Convert.ToInt32(AllFields[(int)AllSensorFieldsname.SerialNumber_Input]);
                query_Sensor = query_Sensor.Where(x => x.subId == CurrentSubId).Where(x => x.Equipmentid == SerialNumber_Input);
                hasExp = true;
            }

            if (!string.IsNullOrEmpty(AllFields[(int)AllSensorFieldsname.MacAddress_Input]))
            {
                String MacAddress_Input = Convert.ToString(AllFields[(int)AllSensorFieldsname.MacAddress_Input]);
                query_Sensor = query_Sensor.Where(x => x.subId == CurrentSubId).Where(x => x.Mac == MacAddress_Input);
                hasExp = true;
            }

            bool hasCattle = false;
            int CattleId_Input = 0;
            if (!string.IsNullOrEmpty(AllFields[(int)AllSensorFieldsname.CattleId_Input]))
            {
                CattleId_Input = Convert.ToInt32(AllFields[(int)AllSensorFieldsname.CattleId_Input]);
                hasCattle = true;
            }

            List<CattleTbl> Cattles = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).List().ToList();
            if (hasExp)
            {
                try
                {
                    if (hasCattle)
                    {
                        CattleTbl _Cattle = mContext.QueryOver<CattleTbl>()
                            .Where(x => x.animalNumber == CattleId_Input)
                            .Where(x => x.FarmID == CurrentFarmId).SingleOrDefault<CattleTbl>();
                        if (_Cattle != null)
                        {
                            AllSensors = query_Sensor.List<EquipmentTbl>();
                            foreach (var sensor in AllSensors)
                            {
                                int CattleId = mContext.QueryOver<SensorTbl>()
                                .Where(x => x.FarmID == CurrentFarmId)
                                .Where(x => x.MacAddress == sensor.Mac).Select(x => x.cattleId).SingleOrDefault<int>();
                                if (_Cattle.ID == CattleId)
                                {
                                    AllCattles.Add(_Cattle);
                                    SensorView item = new SensorView();
                                    item.ID = sensor.ID;
                                    item.MacAddress = sensor.Mac;
                                    item.cattleId = CattleId;
                                    item.FarmID = CurrentFarmId;
                                    item.Equipmentid = sensor.Equipmentid;
                                    _SensorView.Add(item);
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        AllSensors = query_Sensor.List<EquipmentTbl>();
                        foreach (var sensor in AllSensors)
                        {
                            int CattleId = mContext.QueryOver<SensorTbl>()
                                .Where(x => x.FarmID == CurrentFarmId)
                                .Where(x => x.MacAddress == sensor.Mac).Select(x => x.cattleId).SingleOrDefault<int>();
                            if (CattleId != 0)
                            {
                                CattleTbl _Cattle = mContext.QueryOver<CattleTbl>().Where(x => x.ID == CattleId).SingleOrDefault<CattleTbl>();
                                AllCattles.Add(_Cattle);

                                SensorView item = new SensorView();
                                item.ID = sensor.ID;
                                item.MacAddress = sensor.Mac;
                                item.cattleId = CattleId;
                                item.FarmID = CurrentFarmId;
                                item.Equipmentid = sensor.Equipmentid;
                                _SensorView.Add(item);
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
                int pages = (int)((double)_SensorView.Count / (double)ItemPerPage);
                if (pages == 0)
                {
                    pages++;
                }
                model.Cattles = AllCattles;
                model.Sensors = _SensorView;
                model.PagedSensors = _SensorView;
                model.current = pages;
            }
            else
            {
                if (hasCattle)
                {
                    List<CattleTbl> _CattleList = mContext.QueryOver<CattleTbl>()
                            .Where(x => x.animalNumber == CattleId_Input)
                            .Where(x => x.FarmID == CurrentFarmId).List().ToList();
                    foreach (var cattle in _CattleList)
                    {
                        SensorTbl CurrentCattle = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == cattle.ID).Where(x => x.FarmID == CurrentFarmId).SingleOrDefault();
                        if (CurrentCattle != null)
                        {
                            AllSensors = mContext.QueryOver<EquipmentTbl>().Where(x => x.Mac == CurrentCattle.MacAddress).Where(x => x.subId == CurrentSubId).List().ToList();
                            foreach (var sensor in AllSensors)
                            {
                                int CattleId = cattle.ID;
                                if (CattleId != 0)
                                {
                                    AllCattles.Add(cattle);
                                    SensorView item = new SensorView();
                                    item.ID = sensor.ID;
                                    item.MacAddress = sensor.Mac;
                                    item.cattleId = CattleId;
                                    item.FarmID = CurrentFarmId;
                                    item.Equipmentid = sensor.Equipmentid;
                                    _SensorView.Add(item);
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    int pages = (int)((double)_SensorView.Count / (double)ItemPerPage);
                    if (pages == 0)
                    {
                        pages++;
                    }
                    model.Cattles = AllCattles;
                    model.Sensors = _SensorView;
                    model.PagedSensors = _SensorView;
                    model.current = pages;
                }
                else
                {
                    model = LoadSensorsList(page);
                }
            }

            Context.Close(mContext);

            return model;
        }
    }

    public class SensorView
    {
        public int ID { get; set; }
        public String MacAddress { get; set; }
        public int cattleId { get; set; }
        public int FarmID { get; set; }
        public int Equipmentid { get; set; }
    }

    public class SensorData
    {
        public string MAC { get; set; }
        public SensorStatus status { get; set; }
    }

    public class ReportPaging
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
    }
}

using NHibernate;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using SmartCattle.Web.Models;
using System.Web.Configuration;
using System.Net;
using System.Text;

namespace SmartCattle.Web.Controllers
{
    public class FarmController : Controller
    {
        // GET: Farm
        //https://en.wikipedia.org/wiki/Lists_of_cities_by_country
        public ActionResult Index()
        {
            ISession mContext = Context.Open();
            EssentialDataForCreationFarm _retItem = new EssentialDataForCreationFarm();
            List<String> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.FarmId != -1).SelectList(list => list.SelectGroup(x => x.Email)).List<String>().ToList();
            List<CountriesTbl> _CountryList = mContext.QueryOver<CountriesTbl>().List<CountriesTbl>().ToList<CountriesTbl>();
            List<CitiesTbl> _CitiesTbl = new List<CitiesTbl>();
            if(_CountryList.Count != 0)
            {
                _CitiesTbl = mContext.QueryOver<CitiesTbl>().Where(x => x.country_id == _CountryList[0].ID).List<CitiesTbl>().ToList<CitiesTbl>();
            }
            List<FarmTbl> FarmList = new List<FarmTbl>();
            List<RolesList_FarmTbl> SystemRole_FarmList = mContext.QueryOver<RolesList_FarmTbl>().List().ToList();
            _retItem.CityList = _CitiesTbl;
            _retItem.UserInfoList = _UserInfo;
            _retItem.CountryList = _CountryList;

            //////////////////////////////////////////////////
            UserInfo _clssUserInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == Helper.Helper.getCurrentUserId()).SingleOrDefault();
            if (_clssUserInfo != null)
            {
                if (_clssUserInfo.FarmId == -1)
                {
                    String _FarmList = _clssUserInfo.FarmIdList;
                    if (_FarmList != null)
                    {
                        if (_FarmList != "")
                        {
                            String[] _SplitedFarmList = _FarmList.Split(',');
                            for (int i = 0; i < _SplitedFarmList.Length; i++)
                            {
                                FarmTbl farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == Convert.ToInt16(_SplitedFarmList[i])).SingleOrDefault();
                                if (farm != null)
                                {
                                    farm.FarmTypeUId = _clssUserInfo.RoleId;
                                    FarmList.Add(farm);
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

                    }
                }
                else
                {
                    String CurrentUserEmail = _clssUserInfo.Email;
                    List<UserInfo> _UserInfos = mContext.QueryOver<UserInfo>().Where(x => x.Email == CurrentUserEmail).List<UserInfo>().ToList();
                    if (_UserInfos != null)
                    {
                        for (int i = 0; i < _UserInfos.Count; i++)
                        {
                            FarmTbl farm = new FarmTbl();
                            farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == Convert.ToInt16(_UserInfos[i].FarmId)).SingleOrDefault();
                            if (farm != null)
                            {
                                FarmTbl newFarm = new FarmTbl();

                                newFarm.Street1 = farm.Street1;
                                newFarm.Street2 = farm.Street2;
                                newFarm.No = farm.No;
                                newFarm.Phone1 = farm.Phone1;
                                newFarm.Phone2 = farm.Phone2;
                                newFarm.ID = farm.ID;
                                newFarm.FarmName = farm.FarmName;
                                newFarm.Email = farm.Email;
                                newFarm.Country = farm.Country;
                                newFarm.City = farm.City;
                                newFarm.PostalCode = farm.PostalCode;
                                newFarm.FarmTypeUId = _UserInfos[i].RoleId;

                                FarmList.Add(newFarm);
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
            }
            _retItem.FarmList = FarmList;
            /////////////////////////////////////////////////
            List<RolesList_FarmTbl> SystemRole_FarmList_tmp = mContext.QueryOver<RolesList_FarmTbl>().List().ToList();
            Boolean f_find = true;
            String CurrentUserRole = Helper.Helper.getCurrentRoleuId();
            String CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
            String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');

            f_find = true;
            List<RolesList_FarmTbl> AccessRoleList = new List<RolesList_FarmTbl>();
            for (int i = 0; i < SystemRole_FarmList_tmp.Count; i++)
            {
                String tmpPermission = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == SystemRole_FarmList_tmp[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
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
                            AccessRoleList.Add(SystemRole_FarmList_tmp[i]);
                        }
                    }
                }
                else
                {

                }
            }
            ////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////

            mContext.Clear();
            _retItem.FarmType = AccessRoleList;

            Context.Close(mContext);
            return View(_retItem);
        }

        private struct HttpPostParam
        {
            private string _key;
            private string _value;

            public string Key { get { return HttpUtility.UrlEncode(this._key); } set { this._key = value; } }
            public string Value { get { return HttpUtility.UrlEncode(this._value); } set { this._value = value; } }

            public HttpPostParam(string key, string value)
            {
                this._key = key;
                this._value = value;
            }
        };

        private static string PostTrivialData(Uri page, HttpPostParam[] parameters)
        {
            string pageResponse = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(page); //create the initial request.
                request.Method = WebRequestMethods.Http.Post; //set the method
                request.AllowAutoRedirect = true; //couple of settings I personally prefer.
                request.KeepAlive = true;
                request.ContentType = "application/form-data";
                request.UseDefaultCredentials = true;

                //create the post data.
                byte[] bData = Encoding.UTF8.GetBytes(string.Join("&", Array.ConvertAll(parameters, kvp => string.Format("{0}={1}", kvp.Key, kvp.Value))));
                using (var reqStream = request.GetRequestStream())
                    reqStream.Write(bData, 0, bData.Length); //write the data to the request.

                using (var response = (HttpWebResponse)request.GetResponse()) //attempt to get the response.
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotModified) //check for a valid status (should only return 200 if successful)
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                            pageResponse = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                String Ack = e.Message;
            }
            return pageResponse;
        }

        [HttpPost]
        public String SetFarm(String[] FarmData)
        {
            String _retValue = "NaN";
            int NewFarmId = -3;
            String txtFarmName = FarmData[(int)AllFarmFields.txtFarmName];
            String FarmUserId = FarmData[(int)AllFarmFields.FarmUserId];
            String txtFarmRoleType = FarmData[(int)AllFarmFields.txtFarmRoleType];
            String Country = FarmData[(int)AllFarmFields.Country];
            String txtProvince = FarmData[(int)AllFarmFields.txtProvince];
            String CityName = FarmData[(int)AllFarmFields.CityName];
            String txtStreet_Name1 = FarmData[(int)AllFarmFields.txtStreet_Name1];
            String txtStreet_Name2 = FarmData[(int)AllFarmFields.txtStreet_Name2];
            String txtFarmNo = FarmData[(int)AllFarmFields.txtFarmNo];
            String txtPostalCode = FarmData[(int)AllFarmFields.txtPostalCode];
            String txtPhone_1 = FarmData[(int)AllFarmFields.txtPhone_1];
            String txtPhone_2 = FarmData[(int)AllFarmFields.txtPhone_2];
            String txtLatitiude = FarmData[(int)AllFarmFields.txtLatitiude];
            String txtLongitude = FarmData[(int)AllFarmFields.txtLongitude];
            String eExcelType = FarmData[(int)AllFarmFields.eExcelType];
            String txtFileName = FarmData[(int)AllFarmFields.txtFileName];

            ISession mContext = Context.Open();

            List<UserInfo> _UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.Email == FarmUserId).List().ToList();
            if (_UserInfoList.Count == 0)
            {
                _retValue = "NULL";
            }
            else
            {
                Encryption _Encrypt = new Encryption();
                String iv = WebConfigurationManager.AppSettings["ENCRYPTION_IV"];
                String key = WebConfigurationManager.AppSettings["ENCRYPTION_KEY"];

                requestNewSubProject _requestNewSubProject = new requestNewSubProject()
                {
                    apiKey = "a43f6670-9d37-11e7-ad9d-819a9b28ee42",
                    subProjectName = txtFarmName// _Encrypt.encrypt(NewFarmId.ToString() + "_" + DateTime.Now.ToString() + "_" + (new Random().NextDouble().ToString().Replace("0.", "")), key, iv)
                };

                ServicePointManager.Expect100Continue = false;
                WebService.HttpService HttpServiceClientSensorsList = new WebService.HttpService();
                HttpServiceClientSensorsList.ContentType = "application/form-data";
                HttpServiceClientSensorsList.EndPoint = "http://79.175.133.194:2222/requestNewSubProject";
                HttpServiceClientSensorsList.Method = WebService.HttpVerb.POST;
                HttpServiceClientSensorsList.Body = _requestNewSubProject;
                String jsonSubProject = HttpServiceClientSensorsList.MakeRequest();
                X_requestNewSubProject SubProjectModel = JsonConvert.DeserializeObject<X_requestNewSubProject>(jsonSubProject);
                //X_requestNewSubProject SubProjectModel = new X_requestNewSubProject();
                //SubProjectModel.statusCode = 200;
                //SubProjectModel.message.spId = 10;

                if (SubProjectModel != null)
                {
                    if (SubProjectModel.statusCode == 200)
                    {
                        FarmTbl _Farm = new FarmTbl()
                        {
                            SubprojectID = SubProjectModel.message.spId,
                            FarmName = txtFarmName,
                            Email = FarmUserId,
                            Latitude = txtLatitiude,
                            Longitude = txtLongitude,
                            Country = Country,
                            City = CityName,
                            Province = txtProvince,
                            No = txtFarmNo,
                            Street1 = txtStreet_Name1,
                            Street2 = txtStreet_Name2,
                            PostalCode = txtPostalCode,
                            Phone1 = txtPhone_1,
                            Phone2 = txtPhone_2,
                            CreateDate = DateTime.Now
                        };
                        var item = mContext.Save(_Farm);
                        NewFarmId = (int)item;

                        //Change jName with uId
                        String Permissions = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == txtFarmRoleType).Select(x => x.Permissions).SingleOrDefault<String>();
                        List<UserInfo> GetUserData = mContext.QueryOver<UserInfo>().Where(x => x.Email == FarmUserId).Where(x => x.RoleId == "NaN").Where(x => x.FarmId == -3).List().ToList();
                        mContext.Clear();
                        if (GetUserData.Count == 0)
                        {
                            UserInfo NewUserInfo = new UserInfo()
                            {
                                Name = _UserInfoList[0].Name,
                                Family = _UserInfoList[0].Family,
                                Email = _UserInfoList[0].Email,
                                Password = _UserInfoList[0].Password,
                                RoleName = "FarmManager_" + item.ToString(),
                                RoleId = Encryption.GenerateAlphabicUId(),
                                FarmId = (int)item,
                                CreateDate = DateTime.Now,
                            };
                            var newFarm = mContext.Save(NewUserInfo);
                        }
                        else
                        {
                            GetUserData[0].FarmId = (int)item;
                            GetUserData[0].RoleName = "FarmManager_" + item.ToString();
                            GetUserData[0].RoleId = Encryption.GenerateAlphabicUId();

                            mContext.Update(GetUserData[0]);
                            mContext.Flush();
                        }

                        int getCurrentUserId = Helper.Helper.getCurrentUserId();
                        UserInfo CurrentStaff = mContext.QueryOver<UserInfo>().Where(x => x.ID == getCurrentUserId).SingleOrDefault();
                        if (CurrentStaff.FarmIdList == null)
                        {
                            CurrentStaff.FarmIdList = item.ToString();
                        }
                        else
                        {
                            CurrentStaff.FarmIdList += "," + item.ToString();
                        }
                        mContext.Update(CurrentStaff);
                        mContext.Flush();

                        mContext.Clear();
                        RolesList_UserTbl _NewRole = new RolesList_UserTbl()
                        {
                            jName = "FarmManager_" + item.ToString(),
                            uId = Encryption.GenerateAlphabicUId(),
                            Permissions = Permissions,
                            Comment = "یک نقش سیستمی است که امکان حذف و ویرایش آن وجود ندارد. این نقش به مدیر فارم تخصیص داده می شود",
                            FarmId = (int)item
                        };
                        mContext.Save(_NewRole);
                    }
                    else
                    {
                        _retValue = "SPID_N200";
                    }
                }
                else
                {
                    _retValue = "SPID_NULL";
                }
            }

            Context.Close(mContext);
            if(NewFarmId != -3)
            {
                String path = Path.Combine(Server.MapPath("~/App_Data/uploads"), txtFileName);
                List<List<String>> ParsedExcell = Helper.Helper.ParseCattleExcel(path, eExcelType);
                foreach (var item in ParsedExcell)
                {
                    SaveCattle(
                        Convert.ToInt16(item[(int)ExcelType.SmartCattle.animalNumber]),
                        item[(int)ExcelType.SmartCattle.Name],
                        item[(int)ExcelType.SmartCattle.Sex],
                        item[(int)ExcelType.SmartCattle.Genetics_type_num],
                        item[(int)ExcelType.SmartCattle.birthDate],
                        Convert.ToInt16(item[(int)ExcelType.SmartCattle.MotherID]),
                        item[(int)ExcelType.SmartCattle.lastCalvingDate],
                        Convert.ToInt16(item[(int)ExcelType.SmartCattle.lactationNumber]), NewFarmId);

                }
            }
            
            return _retValue;
        }

        [HttpPost]
        public String RemoveFarm(int FarmId)
        {
            String retvalue = "NaN";
            ISession mContext = Context.Open();
            List<CattleTbl> CattleList = mContext.QueryOver<CattleTbl>().Where(x => x.FarmID == FarmId).List().ToList();
            if (CattleList.Count == 0)
            {
                String deleteQuery = string.Format("DELETE FROM {0} where ID = {1}", "SmartCattle.FarmTbl", FarmId);
                mContext.CreateSQLQuery(deleteQuery).ExecuteUpdate();
                retvalue = "OK";
                mContext.Clear();
                List<UserInfo> AllUserInfoList = mContext.QueryOver<UserInfo>().List().ToList();
                foreach (var item in AllUserInfoList)
                {
                    if(item.FarmIdList == null)
                    {

                    }
                    else
                    {
                        if(item.FarmIdList.Contains("," + FarmId.ToString()))
                        {
                            item.FarmIdList = item.FarmIdList.Replace("," + FarmId.ToString(), "");
                            mContext.Update(item);
                            mContext.Flush();
                        }
                        else if (item.FarmIdList.Contains(FarmId.ToString() + ","))
                        {
                            item.FarmIdList = item.FarmIdList.Replace(FarmId.ToString() + ",", "");
                            mContext.Update(item);
                            mContext.Flush();
                        }
                    }

                    if(item.FarmId == FarmId)
                    {
                        item.FarmId = -3;
                        mContext.Update(item);
                        mContext.Flush();
                    }
                }
            }
            else
            {
                retvalue = "SIMILAR";
            }
            
            Context.Close(mContext);

            return retvalue;
        }

        public ActionResult Edit(int FarmId)
        {
            EditedFarm _EditedFarm = new EditedFarm();
            _EditedFarm._EssentialDataForEditFarm = new EssentialDataForCreationFarm();
            ISession mContext = Context.Open();

            if (FarmId != 0)
            {
                FarmTbl farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).SingleOrDefault<FarmTbl>();
                if (farm != null)
                {
                    EssentialDataForCreationFarm _EssentialDataForFarm = new EssentialDataForCreationFarm();
                    List<String> _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.FarmId != -1).SelectList(list => list.SelectGroup(x => x.Email)).List<String>().ToList();
                    List<FarmTbl> FarmList = mContext.QueryOver<FarmTbl>().List().ToList();
                    List<RolesList_FarmTbl> SystemRole_FarmList_tmp = mContext.QueryOver<RolesList_FarmTbl>().List().ToList();

                    List<CountriesTbl> _CountryList = mContext.QueryOver<CountriesTbl>().List<CountriesTbl>().ToList<CountriesTbl>();
                    List<RegionsTbl> _RegionsTbl = new List<RegionsTbl>();
                    if (_CountryList.Count != 0)
                    {
                        CountriesTbl CurrentCountry = _CountryList.Where(x => x.Name == farm.Country).Take(1).SingleOrDefault();
                        if (CurrentCountry != null)
                        {
                            _RegionsTbl = mContext.QueryOver<RegionsTbl>().Where(x => x.CountryId == CurrentCountry.ID).List<RegionsTbl>().ToList<RegionsTbl>();
                        }
                    }
                    /////////////////////////////////////////////////////////////////
                    Boolean f_find = true;
                    String CurrentUserRole = Helper.Helper.getCurrentRoleuId();
                    String CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    String[] SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');

                    f_find = true;
                    List<RolesList_FarmTbl> AccessRoleList = new List<RolesList_FarmTbl>();
                    for (int i = 0; i < SystemRole_FarmList_tmp.Count; i++)
                    {
                        String tmpPermission = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == SystemRole_FarmList_tmp[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
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
                                    AccessRoleList.Add(SystemRole_FarmList_tmp[i]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    ////////////////////////////////////////////////////////////////
                    List<CitiesTbl> _CitiesTbl = new List<CitiesTbl>();
                    foreach (var City in _RegionsTbl)
                    {
                        CitiesTbl ItemCity = new CitiesTbl();

                        ItemCity.country_id = City.CountryId;
                        ItemCity.name = City.Name;

                        _CitiesTbl.Add(ItemCity);
                    }

                    _EssentialDataForFarm.CityList = _CitiesTbl;
                    _EssentialDataForFarm.UserInfoList = _UserInfo;
                    _EssentialDataForFarm.CountryList = _CountryList;
                    _EssentialDataForFarm.FarmList = FarmList;
                    _EssentialDataForFarm.FarmType = AccessRoleList;
                    _EditedFarm._EssentialDataForEditFarm = _EssentialDataForFarm;
                    _EditedFarm.FarmId = FarmId;
                    _EditedFarm.Farm = farm;

                    mContext.Clear();
                    RolesList_FarmTbl tmpFarm = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == farm.FarmTypeUId).SingleOrDefault();
                    _EditedFarm._EssentialDataForEditFarm.Mine = new List<string>();
                    if (tmpFarm != null)
                    {
                        _EditedFarm._EssentialDataForEditFarm.Mine.Add(tmpFarm.jName);
                        _EditedFarm._EssentialDataForEditFarm.Mine.Add(tmpFarm.ID.ToString());

                        for (int i = 0; i < _EditedFarm._EssentialDataForEditFarm.FarmType.Count; i++)
                        {
                            if(_EditedFarm._EssentialDataForEditFarm.FarmType[i].uId.Equals(tmpFarm.uId))
                            {
                                _EditedFarm._EssentialDataForEditFarm.FarmType.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        _EditedFarm._EssentialDataForEditFarm.Mine.Add("Nothing");
                        _EditedFarm._EssentialDataForEditFarm.Mine.Add("NaN");
                    }
                }
                else
                {

                }
            }
            else
            {

            }
            Context.Close(mContext);

            return View(_EditedFarm);
        }

        public String Edit_Farm_Identity(int FarmId, String FarmName, String FarmUserId, String FarmRole)
        {
            String retValue = "NaN";
            if (FarmId != 0)
            {
                if (FarmName != "")
                {
                    if (FarmUserId != "")
                    {
                        if (FarmRole != "")
                        {
                            ISession mContext = Context.Open();
                            RolesList_FarmTbl tmpRoleList = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.ID == Convert.ToInt16(FarmRole)).SingleOrDefault();
                            if(tmpRoleList != null)
                            {
                                FarmTbl CurrentFarm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).SingleOrDefault();
                                CurrentFarm.FarmName = FarmName;
                                CurrentFarm.Email = FarmUserId;
                                CurrentFarm.FarmTypeUId = tmpRoleList.uId;
                                CurrentFarm.FarmTypeId = Convert.ToInt16(FarmRole);
                                mContext.Update(CurrentFarm);
                                mContext.Flush();
                                Context.Close(mContext);

                                retValue = "OK";
                            }
                            else
                            {
                                retValue = "NaN";
                            }
                        }
                        else
                        {
                            retValue = "FILL";
                        }
                    }
                    else
                    {
                        retValue = "FILL";
                    }
                }
                else
                {
                    retValue = "FILL";
                }
            }
            else
            {
                retValue = "FILL";
            }
            return retValue;
        }

        public String Edit_Farm_Address(int FarmId, String Country, String City, String Province, String Street_Name1, String Street_Name2, String FarmNo, String PostalCode, String Phone_1, String Phone_2)
        {
            String retValue = "NaN";
            if (FarmId != 0)
            {
                if (PostalCode != "")
                {
                    ISession mContext = Context.Open();
                    FarmTbl CurrentFarm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).SingleOrDefault();
                    CurrentFarm.Country = Country;
                    CurrentFarm.Province = Province;
                    CurrentFarm.City = City;
                    CurrentFarm.Street1 = Street_Name1;
                    CurrentFarm.Street2 = Street_Name2;
                    CurrentFarm.No = FarmNo;
                    CurrentFarm.PostalCode = PostalCode;
                    CurrentFarm.Phone1 = Phone_1;
                    CurrentFarm.Phone2 = Phone_2;
                    mContext.Update(CurrentFarm);
                    mContext.Flush();
                    Context.Close(mContext);
                    retValue = "OK";
                }
                else
                {
                    retValue = "FILL";
                }
            }
            else
            {
                retValue = "FILL";
            }
            return retValue;
        }

        public String Edit_Farm_Location(int FarmId, String Lat, String Lng)
        {
            String retValue = "NaN";
            if(Lat != "" && Lng != "")
            {
                if(FarmId != 0)
                {
                    ISession mContext = Context.Open();
                    FarmTbl _farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).Take(1).SingleOrDefault();
                    if(_farm != null)
                    {
                        _farm.Latitude = Lat;
                        _farm.Longitude = Lng;

                        mContext.Clear();
                        mContext.Update(_farm);
                        mContext.Flush();

                        retValue = "OK";
                    }
                    else
                    {
                        retValue = "NaN";
                    }
                    Context.Close(mContext);
                }
                else
                {
                    retValue = "NaN";
                }
            }
            else
            {
                retValue = "EMPTY";
            }
            return retValue;
        }

        [HttpPost]
        public String EditFarm(int FarmId)
        {
            return "OK";
        }

        public ActionResult List()
        {
            ISession mContext = Context.Open();
            List<FarmTbl> FarmList = new List<FarmTbl>();
            UserInfo _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == Helper.Helper.getCurrentUserId()).SingleOrDefault();
            if (_UserInfo != null)
            {
                if (_UserInfo.FarmId == -1)
                {
                    String _FarmList = _UserInfo.FarmIdList;
                    if (_FarmList != null)
                    {
                        if (_FarmList != "")
                        {
                            String[] _SplitedFarmList = _FarmList.Split(',');
                            for (int i = 0; i < _SplitedFarmList.Length; i++)
                            {
                                FarmTbl farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == Convert.ToInt16(_SplitedFarmList[i])).SingleOrDefault();
                                if (farm != null)
                                {
                                    farm.FarmTypeUId = _UserInfo.RoleId;
                                    FarmList.Add(farm);
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

                    }
                }
                else
                {
                    String CurrentUserEmail = _UserInfo.Email;
                    List<UserInfo> _UserInfos = mContext.QueryOver<UserInfo>().Where(x => x.Email == CurrentUserEmail).List<UserInfo>().ToList();
                    if (_UserInfos != null)
                    {
                        for (int i = 0; i < _UserInfos.Count; i++)
                        {
                            FarmTbl farm = new FarmTbl();
                            farm = mContext.QueryOver<FarmTbl>().Where(x => x.ID == Convert.ToInt16(_UserInfos[i].FarmId)).SingleOrDefault();
                            if (farm != null)
                            {
                                FarmTbl newFarm = new FarmTbl();

                                newFarm.Street1 = farm.Street1;
                                newFarm.Street2 = farm.Street2;
                                newFarm.No = farm.No;
                                newFarm.Phone1 = farm.Phone1;
                                newFarm.Phone2 = farm.Phone2;
                                newFarm.ID = farm.ID;
                                newFarm.FarmName = farm.FarmName;
                                newFarm.Email = farm.Email;
                                newFarm.Country = farm.Country;
                                newFarm.City = farm.City;
                                newFarm.PostalCode = farm.PostalCode;
                                newFarm.FarmTypeUId = _UserInfos[i].RoleName;

                                FarmList.Add(newFarm);
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
            }

            Context.Close(mContext);
            return View(FarmList);
        }

        [HttpPost]
        public String SetCurrentFarm(int FarmId)
        {
            Helper.Helper.setCurrentFarmId(FarmId);
            return "OK";
        }

        [HttpPost]
        public String SignOutFarm(int FarmId)
        {
            Helper.Helper.setCurrentFarmId(-1);
            return "OK";
        }

        private String SaveCattle(int animalNumber, String Name, String Sex, String Genetics_type_num, String birthDate, int MotherID, String lastCalvingDate, int lactationNumber, int NewFarmId)
        {
            String ret = "";
            int farmID = NewFarmId;
            if (animalNumber != 0 && Sex != "" && Sex != "Sex" && Genetics_type_num != "" && birthDate != "")
            {
                try
                {
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


                    DateTime CattleBirthday = DateHelper.ConvertToGeorginDate(birthDate);
                    DateTime CattleLastCalvingDate = DateHelper.ConvertToGeorginDate(lastCalvingDate);

                    CattleTbl cattle = new CattleTbl();
                    cattle.age = DateHelper.calculateAge(CattleBirthday);
                    cattle.animalNumber = animalNumber;
                    cattle.Name = Name;
                    cattle.Sex = Sex;
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

        public JsonResult LoadCitybyCountry(int CityId)
        {
            ISession mContext = Context.Open();
            List<RegionsTbl> _CitiesTbl = mContext.QueryOver<RegionsTbl>().Where(x => x.CountryId == CityId).List().ToList();
            Context.Close(mContext);
            return Json(_CitiesTbl, JsonRequestBehavior.AllowGet);
        }
    }

    //https://en.wikipedia.org/wiki/Lists_of_cities_by_country

    public class EditedFarm
    {
        public int FarmId { get; set; }
        public FarmTbl Farm { get; set; }
        public EssentialDataForCreationFarm _EssentialDataForEditFarm { get; set; }
    }

    public class EssentialDataForCreationFarm
    {
        public List<String> UserInfoList { get; set; }
        public List<CitiesTbl> CityList { get; set; }
        public List<CountriesTbl> CountryList { get; set; }
        public List<FarmTbl> FarmList { get; set; }
        public List<RolesList_FarmTbl> FarmType { get; set; }
        public List<String> Mine { get; set; }
    }

    public class AnswerResult
    {
        public int Code { get; set; }
        public String Status { get; set; }
        public String Message { get; set; }
    }

    public class AnswerStatus
    {
        public static int SuccessCode = 200;
        public static int FailerCode = -1;
        public static int ExceptionCode = 500;
    }

    public enum AllFarmFields
    {
        txtFarmName,
        FarmUserId,
        txtFarmRoleType,
        Country,
        txtProvince,
        CityName,
        txtStreet_Name1,
        txtStreet_Name2,
        txtFarmNo,
        txtPostalCode,
        txtPhone_1,
        txtPhone_2,
        txtLatitiude,
        txtLongitude,
        eExcelType,
        txtFileName
    }

    public class ExcelType
    {
        public enum SmartCattle
        {
            ID,
            animalNumber,
            Name,
            Sex,
            Genetics_type_num,
            MotherID,
            birthDate,
            lastCalvingDate,
            lactationNumber
        }
    }

}
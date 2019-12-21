using Microsoft.Office.Interop.Excel;
using NHibernate;
using SmartCattle.Web.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;

namespace SmartCattle.Web.Helper
{
    public class Helper
    {
        private static int CurrentUseID = 0;
        private static int CurrentFarmId = -2;

        public static string FA = "fa-IR";
        public static string EN = "en-US";

        public static String getGitUpdateTime()
        {
            String retValue = "";
            string path = HttpContext.Current.Server.MapPath("~/Web.config");

            FileInfo fi = new FileInfo(path);
            var created = fi.CreationTime;
            var lastmodified = fi.LastWriteTime;
            retValue = "Updated Time: " + lastmodified.ToString("yyyy/MM/dd HH:mm");
            return retValue;
        }

        // get HiddenPrivilegesList
        public static List<Tuple<string, string>> GetIgnorePrivilegeList()
        {
            var privilegeIgnoreList = new List<Tuple<string, string>>
            {
                Tuple.Create("Setting", "PermissionsList"),
                Tuple.Create("Home", "Alerts"),
                Tuple.Create("UserPermissions", "SavePermissions"),
                Tuple.Create("Cattle", "CattleNotification"),
                Tuple.Create("Setting", "ImportCattles"),
                Tuple.Create("Cattle", "RefreshDetail"),
                Tuple.Create("Cattle", "CattleLocation"),
                Tuple.Create("Home", "SitePath"),
                Tuple.Create("TimeBudget", "Index"),
                Tuple.Create("TimeBudget", "CreateGroupTimeBudget"),
                Tuple.Create("Farm", "SetCurrentFarm")
            };

            return privilegeIgnoreList;
        }

        public static bool isIgnored(string strController, string strAction)
        {
            bool checkIgnored = false;
            var privilegeIgnoreList = GetIgnorePrivilegeList();
            foreach (Tuple<string, string> tuple in privilegeIgnoreList)
            {
                if (tuple.Item1 == strController && tuple.Item2 == strAction)
                {
                    checkIgnored = true;
                    break;
                }

            }
            return checkIgnored;
        }

        // Remove IgnorePrivilegeList from PermissionList
        public static void RemoveIgnoreList(ref List<ActionControllerListTbl> permissionList)
        {
            var privilegeIgnoreList = GetIgnorePrivilegeList();

            for (int i = 0; i < permissionList.Count; i++)
            {
                foreach (Tuple<string, string> tuple in privilegeIgnoreList)
                {
                    if (tuple.Item1 == permissionList[i].Controller && tuple.Item2 == permissionList[i].Action)
                    {
                        permissionList.Remove(permissionList[i]);
                    }

                }
            }
        }

        public static void RemoveIgnoreList(ref string permissionList)
        {
            var privilegeIgnoreList = GetIgnorePrivilegeList();
            string strPermission = string.Empty;
            foreach (Tuple<string, string> tuple in privilegeIgnoreList)
            {
                strPermission = tuple.Item1.ToString() + "-" + tuple.Item2.ToString();
                if (permissionList.Contains(strPermission))
                {
                    permissionList.Replace("," + strPermission, "");
                }

            }
        }

        public static void AddIgnoreList(ref string permissionList)
        {
            var privilegeIgnoreList = GetIgnorePrivilegeList();

            foreach (Tuple<string, string> tuple in privilegeIgnoreList)
            {
                string strPermission = tuple.Item1.ToString() + "-" + tuple.Item2.ToString();
                if (!permissionList.Contains(strPermission))
                {
                    permissionList += "," + strPermission;
                }

            }
        }

        internal static void setCurrentCulture(string CurrentCulture)
        {
            HttpContext.Current.Session["CurrentCulture"] = CurrentCulture;
        }

        public static void setCurrentUser(int iD)
        {
            CurrentUseID = iD;
            HttpContext.Current.Session["CurrentCulture"] = iD;
        }

        public static void setCurrentFarmId(int FarmId)
        {
            CurrentFarmId = FarmId;
            HttpContext.Current.Session["CurrentFarmId"] = FarmId;
        }

        public static void setCurrentUser(String UserName)
        {
            if (UserName != "")
            {
                ISession mContext = Context.Open();
                CurrentUseID = mContext.QueryOver<UserInfo>().Where(x => x.Email == UserName).Select(x => x.ID).List<int>().ToList()[0];
                HttpContext.Current.Session["CurrentUseID"] = CurrentUseID;
                if (CurrentFarmId == -2)
                {
                    CurrentFarmId = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUseID).Select(x => x.FarmId).List<int>().ToList()[0];
                    HttpContext.Current.Session["CurrentFarmId"] = CurrentFarmId;
                }
                Context.Close(mContext);
                setCurrentUser(CurrentUseID);
            }
        }

        public static String getCattleSensorId(int CattleId)
        {
            ISession mContext = Context.Open();
            List<SensorTbl> MacAddress = mContext.QueryOver<SensorTbl>().Where(x => x.cattleId == CattleId).List().ToList();
            Context.Close(mContext);
            if (MacAddress.Count != 0)
            {
                return MacAddress[0].MacAddress;
            }
            else
            {
                return "";
            }
        }

        internal static String getPermissionList(String RoleId)
        {
            ISession mContext = Context.Open();
            String userPermissionsStr = "";
            String userPermissions = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
            String staffPermissions = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
            String farmPermissions = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == RoleId).Select(x => x.Permissions).SingleOrDefault<String>();
            
            if (userPermissions != null)
            {
                userPermissionsStr = userPermissions;
            }
            else if (staffPermissions != null)
            {
                userPermissionsStr = staffPermissions;
            }
            else if (farmPermissions != null)
            {
                userPermissionsStr = farmPermissions;
            }
            Context.Close(mContext);

            return userPermissionsStr;
        }

        public static String getCattleHerdName(int HerdId)
        {
            ISession mContext = Context.Open();
            String GroupName = mContext.QueryOver<CattleHerds>().Where(x => x.ID == HerdId).Select(x => x.name).SingleOrDefault<String>();
            Context.Close(mContext);
            return GroupName;
        }

        public static String getCattleGroupName(int GroupId)
        {
            ISession mContext = Context.Open();
            String GroupName = mContext.QueryOver<CattleGroupTbl>().Where(x => x.ID == GroupId).Select(x => x.name).SingleOrDefault<String>();
            Context.Close(mContext);
            return GroupName;
        }

        public static String getCattleFreeStallName(int FreeStallId)
        {
            ISession mContext = Context.Open();
            String FreeStallName = mContext.QueryOver<FreeStallTbl>().Where(x => x.ID == FreeStallId).Select(x => x.name).SingleOrDefault<String>();
            Context.Close(mContext);
            return FreeStallName;
        }

        public static String getCattleGroup(int CattleId)
        {
            return "تازه زا";
        }

        public static String DateDiff(DateTime _datetime)
        {
            return "17";
        }

        public static String getStringDiff_NotInSecond(String Str1, String Str2)
        {
            String retValue = "";
            List<String> diff = new List<String>();
            IEnumerable<string> set1 = Str1.Split(',').Distinct();
            IEnumerable<string> set2 = Str2.Split(',').Distinct();
            diff = set2.Except(set1).ToList();
            for (int i = 0; i < diff.Count - 1; i++)
            {
                retValue += diff[i] + ",";
            }
            if(diff.Count != 0)
            {
                retValue += diff[diff.Count - 1];
            }
            return retValue;
        }

        public static String getCurrentCulture()
        {
            String CurrentCulture = "";
            try
            {
                CurrentCulture = (String)HttpContext.Current.Session["CurrentCulture"];
                if (CurrentCulture == null)
                {
                    CurrentCulture = "fa-IR";
                }
                else
                {
                    CurrentCulture = "en-US";
                }
            }
            catch (Exception ex)
            {
                CurrentCulture = "fa-IR";
                String Ack = ex.Message;
            }
            
            return CurrentCulture;
        }

        public static String getUserLocation()
        {
            String retValue = "NaN";
            if (Thread.CurrentThread.CurrentCulture.Name == FA)
            {
                ISession mContext = Context.Open();
                int FarmId = getCurrentFarmId();
                if (FarmId != -1)
                {
                    String FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).Select(x => x.City).Take(1).SingleOrDefault<String>();
                    if (FarmName != null)
                    {
                        retValue = FarmName;
                    }
                }
                else
                {
                    retValue = "Tehran";
                }
                Context.Close(mContext);
            }
            else
            {
                retValue = "Los Angeles";
            }
            return retValue;
        }

        public static String getDateBaseOnCulture(DateTime _datetime)
        {
            String retDate = "";
            if (Thread.CurrentThread.CurrentCulture.Name == FA)
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = _datetime.ToString("HH:mm:ss yyyy/MM/dd");
                }
                else
                {
                    retDate = DateHelper.toPersian(_datetime);
                }
            }
            else
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = DateHelper.toGeorjian(_datetime.ToString("HH:mm:ss yyyy/MM/dd")).ToString("HH:mm:ss yyyy/MM/dd");
                }
                else
                {
                    retDate = _datetime.ToString("HH:mm:ss yyyy/MM/dd");
                }
            }
            return retDate;
        }

        public static String getCurrentDate()
        {
            String retDate = "";
            DateTime _datetime = DateTime.Now;
            if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = _datetime.ToString("yyyy/MM/dd");
                }
                else
                {
                    retDate = DateHelper.toPersian(_datetime);
                }
            }
            else
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = DateHelper.toGeorjian(_datetime.ToString("HH:mm:ss yyyy/MM/dd")).ToString("HH:mm:ss yyyy/MM/dd");
                }
                else
                {
                    retDate = _datetime.ToString("yyyy/MM/dd");
                }
            }
            return retDate;
        }

        public static String EliminateHoutFromDate(DateTime date)
        {
            return date.Year.ToString() + "/" + date.Month.ToString() + "/" + date.Day.ToString();
        }

        public static String EliminateHoutFromDate(String date)
        {
            String retValue = "";
            String[] SplitedDate = date.Split(' ');
            if(SplitedDate.Length == 2)
            {
                if(SplitedDate[0].Contains(":"))
                {
                    retValue = SplitedDate[1];
                }
                else
                {
                    retValue = SplitedDate[0];
                }
            }
            return retValue;
        }

        public static String getDateJustGeorgian(DateTime _datetime)
        {
            String retDate = "";
            if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = DateHelper.toGeorjian(_datetime.ToString("HH:mm:ss yyyy/MM/dd")).ToString("HH:mm:ss yyyy/MM/dd");
                }
                else
                {
                    retDate = _datetime.ToString("HH:mm:ss yyyy/MM/dd");
                }
            }
            else
            {
                if (_datetime.ToString("HH:mm:ss yyyy/MM/dd").Contains("1397"))// ;-)(-;
                {
                    retDate = DateHelper.toGeorjian(_datetime.ToString("HH:mm:ss yyyy/MM/dd")).ToString("HH:mm:ss yyyy/MM/dd");
                }
                else
                {
                    retDate = _datetime.ToString("HH:mm:ss yyyy/MM/dd");
                }
            }
            return retDate;
        }

        public static int getCurrentUserId()
        {
            //return CurrentUseID;
            int CurrentUseIDs = -5;
            if (HttpContext.Current.Session["CurrentUseID"] != null)
            {
                CurrentUseIDs = (int)HttpContext.Current.Session["CurrentUseID"];
            }
            return CurrentUseIDs;
        }

        public static String getCurrentUserNameFamily()
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.ID == getCurrentUserId()).List().ToList();
            Context.Close(mContext);
            if (UserInfoList.Count != 0)
            {
                retValue = UserInfoList[0].Name + " " + UserInfoList[0].Family;
            }
            return retValue;
        }

        public static String getCurrentUserEmail()
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.ID == getCurrentUserId()).List().ToList();
            Context.Close(mContext);
            if (UserInfoList.Count != 0)
            {
                retValue = UserInfoList[0].Email;
            }
            return retValue;
        }

        public static int getCurrentFarmId()
        {
            //return CurrentFarmId;
            int CurrentFarmIDs = -5;
            if (HttpContext.Current.Session["CurrentFarmId"] != null)
            {
                CurrentFarmIDs = (int)HttpContext.Current.Session["CurrentFarmId"];
            }
            return CurrentFarmIDs;
        }

        public static int getCurrentSubId()
        {
            int retValue = 0;
            ISession mContext = Context.Open();
            int CurrentfarmId = getCurrentFarmId();
            List<FarmTbl> SubprojectID = mContext.QueryOver<FarmTbl>().Where(x => x.ID == CurrentfarmId).List().ToList();
            Context.Close(mContext);
            if (SubprojectID.Count != 0)
            {
                retValue = SubprojectID[0].SubprojectID;
            }
            else
            {
                retValue = -6;
            }
            return retValue;
        }

        public static String getCurrentRolejName()
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            String CurrentEmail = getCurrentUserEmail();
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.Email == CurrentEmail).List().ToList();
            switch (UserInfoList.Count)
            {
                case 0:
                    break;

                case 1:
                    if (UserInfoList[0].FarmId == -1)
                    {
                        retValue = UserInfoList[0].RoleName;
                    }
                    else
                    {
                        retValue = UserInfoList[0].RoleName;
                    }
                    break;

                default:
                    retValue = UserInfoList.Where(x => x.FarmId == getCurrentFarmId()).Select(x => x.RoleName).SingleOrDefault<String>();
                    break;
            }
            Context.Close(mContext);
            return retValue;
        }

        public static String getCurrentRoleuId()
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            String CurrentEmail = getCurrentUserEmail();
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.Email == CurrentEmail).List().ToList();
            switch (UserInfoList.Count)
            {
                case 0:
                    break;

                case 1:
                    if (UserInfoList[0].FarmId == -1)
                    {
                        retValue = UserInfoList[0].RoleId;
                    }
                    else
                    {
                        retValue = UserInfoList[0].RoleId;
                    }
                    break;

                default:
                    retValue = UserInfoList.Where(x => x.FarmId == getCurrentFarmId()).Select(x => x.RoleId).SingleOrDefault<String>();
                    break;
            }
            Context.Close(mContext);
            return retValue;
        }

        public static int getCurrentRoleId()
        {
            ISession mContext = Context.Open();
            String CurrentEmail = getCurrentUserEmail();
            int RoleId = mContext.QueryOver<UserInfo>().Where(x => x.Email == CurrentEmail).Where(x => x.FarmId == getCurrentFarmId()).Select(x => x.ID).SingleOrDefault<int>();
            Context.Close(mContext);
            return RoleId;
        }

        public static String getCurrentFarmName()
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            int FarmId = getCurrentFarmId();
            if (FarmId != -1)
            {
                String FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                if (FarmName != null)
                {
                    retValue = FarmName;
                }
            }
            Context.Close(mContext);

            return retValue;
        }

        public static String getFarmName(int FarmId)
        {
            String retValue = "NaN";
            ISession mContext = Context.Open();
            if (FarmId != -1)
            {
                String FarmName = mContext.QueryOver<FarmTbl>().Where(x => x.ID == FarmId).Select(x => x.FarmName).SingleOrDefault<String>();
                if (FarmName != null)
                {
                    retValue = FarmName;
                }
            }
            Context.Close(mContext);

            return retValue;
        }

        public static void SwitchLanguageToEn()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("en-US", "en", "US"));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("en-US", "en", "US"));
        }

        public static void SwitchLanguageToFn()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("fa-IR", "fa", "IR"));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("fa-IR", "fa", "IR"));
        }

        public static bool Find(string world, String[] array)
        {
            String[] result = Array.FindAll(array, x => x.Equals(world));
            if (result.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool isStaff()
        {
            bool retvalue = false;
            int CurrentUserId = getCurrentUserId();
            ISession mContext = Context.Open();
            int FarmId = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).Select(x => x.FarmId).SingleOrDefault<int>();
            if (FarmId == 0)
            {
                retvalue = false;
            }
            else
            {
                if (FarmId == -1)
                {
                    retvalue = true;
                }
                else
                {
                    retvalue = false;
                }
            }
            Context.Close(mContext);
            return retvalue;
        }

        public static void SaveParentChild(int ParentId, int ParentType, int ChildId, int ChildType)
        {
            int FarmId = getCurrentFarmId();
            int SubId = getCurrentSubId();

            ISession mContext = Context.Open();
            Role_ParentChildTbl _ParentChild = new Role_ParentChildTbl()
            {
                ParentId = ParentId,
                ParentType = ParentType,
                ChildId = ChildId,
                ChildType = ChildType,
                FarmId = FarmId, 
                SubId = SubId
            };
            mContext.Save(_ParentChild);
            Context.Close(mContext);
        }

        public static List<Role_ParentChildTbl> getAllChildWithParentOf(int ParentId)
        {
            List<Role_ParentChildTbl> retValue = new List<Role_ParentChildTbl>();

            ISession mContext = Context.Open();
            retValue = mContext.QueryOver<Role_ParentChildTbl>().Where(x => x.ParentId == ParentId).List().ToList();
            Context.Close(mContext);

            return retValue;
        }

        public static List<Role_ParentChildTbl> getAllChildWithParentOf(List<Role_ParentChildTbl> ParentIds)
        {
            List<Role_ParentChildTbl> retValue = new List<Role_ParentChildTbl>();

            ISession mContext = Context.Open();
            foreach (var item in ParentIds)
            {
                List<Role_ParentChildTbl> tmp = getAllChildWithParentOf(item.ParentId);
                if (tmp.Count != 0)
                {
                    retValue.AddRange(tmp);
                }
            }
            Context.Close(mContext);

            return retValue;
        }

        internal static void RemovePrivilege(string Privileges, int RoleId, RoleType _RoleType)
        {
            ISession mContext = Context.Open();
            mContext.Clear();
            switch (_RoleType)
            {
                case RoleType.RolesList_Farm:
                    RolesList_FarmTbl _RolesList_FarmTbl = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.ID == RoleId).SingleOrDefault();
                    if (_RolesList_FarmTbl != null)
                    {
                        String[] PrivilegesList = Privileges.Split(',');
                        for (int i = 0; i < PrivilegesList.Length; i++)
                        {
                            _RolesList_FarmTbl.Permissions = _RolesList_FarmTbl.Permissions.Replace(PrivilegesList[i] + ",", "").Replace("," + PrivilegesList[i], "");
                        }
                    }
                    mContext.Update(_RolesList_FarmTbl);
                    mContext.Flush();
                    break;

                case RoleType.RolesList_Staff:
                    RolesList_StaffTbl _RolesList_StaffTbl = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.ID == RoleId).SingleOrDefault();
                    if (_RolesList_StaffTbl != null)
                    {
                        String[] PrivilegesList = Privileges.Split(',');
                        for (int i = 0; i < PrivilegesList.Length; i++)
                        {
                            _RolesList_StaffTbl.Permissions = _RolesList_StaffTbl.Permissions.Replace(PrivilegesList[i] + ",", "").Replace("," + PrivilegesList[i], "");
                        }
                    }
                    mContext.Update(_RolesList_StaffTbl);
                    mContext.Flush();
                    break;

                case RoleType.RolesList_User:
                    RolesList_UserTbl _RolesList_UserTbl = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.ID == RoleId).SingleOrDefault();
                    if (_RolesList_UserTbl != null)
                    {
                        String[] PrivilegesList = Privileges.Split(',');
                        for (int i = 0; i < PrivilegesList.Length; i++)
                        {
                            _RolesList_UserTbl.Permissions = _RolesList_UserTbl.Permissions.Replace(PrivilegesList[i] + ",", "").Replace("," + PrivilegesList[i], "");
                        }
                    }
                    mContext.Update(_RolesList_UserTbl);
                    mContext.Flush();
                    break;

                default:
                    break;
            }
            Context.Close(mContext);
        }

        public static List<List<String>> ParseCattleExcel(String Path, String inputExcelType)
        {
            List<List<String>> AllCells = new List<List<String>>();
            try
            {
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(Path);
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                switch (inputExcelType)
                {
                    case "SmartCattle":
                        for (int i = 2; i <= rowCount; i++)
                        {
                            List<String> item = new List<string>();
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    item.Add(xlRange.Cells[i, j].Value2.ToString());
                                }
                            }
                            AllCells.Add(item);
                        }
                        break;

                    case "Modiran":
                        for (int i = 1; i <= rowCount; i++)
                        {
                            List<String> item = new List<string>();
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    item.Add(xlRange.Cells[i, j].Value2.ToString());
                                }
                            }
                        }
                        break;

                    case "BaniAsady":
                        break;

                    case "ExcelType4":
                        for (int i = 1; i <= rowCount; i++)
                        {
                            List<String> item = new List<string>();
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    item.Add(xlRange.Cells[i, j].Value2.ToString());
                                }
                            }
                        }
                        break;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {

            }

            return AllCells;
        }

        public static String getCurrentMap()
        {
            String rawJsonGetMap = "";
            if (getCurrentSubId() == 0)
            {
                rawJsonGetMap = "[\n{\n\"type\":\"FeatureCollection\",\n\"features\":[\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.871226,\n35.720333,\n0\n],\n[\n50.8710203,\n35.7202697,\n0\n],\n[\n50.8711812,\n35.7199343,\n0\n],\n[\n50.871397,\n35.720004,\n0\n],\n[\n50.871226,\n35.720333,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"4\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8713126,\n35.7204548,\n0\n],\n[\n50.8714266,\n35.7204929,\n0\n],\n[\n50.8712965,\n35.7207596,\n0\n],\n[\n50.8711839,\n35.7207183,\n0\n],\n[\n50.8713126,\n35.7204548,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"1\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8711839,\n35.7207183,\n0\n],\n[\n50.8710551,\n35.7206747,\n0\n],\n[\n50.8711866,\n35.7204199,\n0\n],\n[\n50.8713126,\n35.7204548,\n0\n],\n[\n50.8711839,\n35.7207183,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"0\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8714192,\n35.7207596,\n0\n],\n[\n50.8715191,\n35.7205637,\n0\n],\n[\n50.8715728,\n35.7205419,\n0\n],\n[\n50.8717793,\n35.7206137,\n0\n],\n[\n50.8716533,\n35.720838,\n0\n],\n[\n50.8714192,\n35.7207596,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"2\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8716533,\n35.720838,\n0\n],\n[\n50.8717793,\n35.7206137,\n0\n],\n[\n50.8719617,\n35.7206682,\n0\n],\n[\n50.8719912,\n35.7207204,\n0\n],\n[\n50.871888,\n35.7209175,\n0\n],\n[\n50.8716533,\n35.720838,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"3\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.871723,\n35.7199822,\n0\n],\n[\n50.8714642,\n35.7198951,\n0\n],\n[\n50.8716372,\n35.7195467,\n0\n],\n[\n50.8718625,\n35.7196229,\n0\n],\n[\n50.8718839,\n35.7196643,\n0\n],\n[\n50.871723,\n35.7199822,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"9\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8714642,\n35.7198951,\n0\n],\n[\n50.8712026,\n35.7198124,\n0\n],\n[\n50.8713797,\n35.719464,\n0\n],\n[\n50.8716372,\n35.7195467,\n0\n],\n[\n50.8714642,\n35.7198951,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"8\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8716908,\n35.7194237,\n0\n],\n[\n50.871833,\n35.7191776,\n0\n],\n[\n50.8720475,\n35.7192636,\n0\n],\n[\n50.8720502,\n35.7193115,\n0\n],\n[\n50.8719429,\n35.719514,\n0\n],\n[\n50.8716908,\n35.7194237,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"11\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8717136,\n35.719428,\n0\n],\n[\n50.8714548,\n35.7193355,\n0\n],\n[\n50.8715594,\n35.7191112,\n0\n],\n[\n50.871605,\n35.7190916,\n0\n],\n[\n50.8718397,\n35.7191798,\n0\n],\n[\n50.8717136,\n35.719428,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"10\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8716318,\n35.7203546,\n0\n],\n[\n50.8717257,\n35.7201608,\n0\n],\n[\n50.871951,\n35.7202261,\n0\n],\n[\n50.8718491,\n35.7204276,\n0\n],\n[\n50.8716318,\n35.7203546,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"6\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.8718491,\n35.7204276,\n0\n],\n[\n50.871951,\n35.7202261,\n0\n],\n[\n50.8721709,\n35.7202958,\n0\n],\n[\n50.872187,\n35.7203459,\n0\n],\n[\n50.87212,\n35.720494,\n0\n],\n[\n50.8720797,\n35.7205027,\n0\n],\n[\n50.8718491,\n35.7204276,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"7\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n50.871226,\n35.720333,\n0\n],\n[\n50.871397,\n35.720004,\n0\n],\n[\n50.871654,\n35.720092,\n0\n],\n[\n50.871483,\n35.720421,\n0\n],\n[\n50.871226,\n35.720333,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"5\",\n\"styleUrl\":\"#poly-0F9D58-1200-76-nodesc\",\n\"styleHash\":\"-3d2b8830\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0F9D58-1200-76-nodesc-normal\",\n\"highlight\":\"#poly-0F9D58-1200-76-nodesc-highlight\"\n},\n\"stroke\":\"#0f9d58\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#0f9d58\",\n\"fill-opacity\":0.2980392156862745\n}\n}\n]\n}\n]";
            }
            else if (getCurrentSubId() == 7)
            {
                rawJsonGetMap = "[\n{\n\"type\":\"FeatureCollection\",\n\"features\":[\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389047,\n35.732411,\n0\n],\n[\n51.389005,\n35.732607,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_1\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389005,\n35.732607,\n0\n],\n[\n51.389171,\n35.732631,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_2\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389183,\n35.732577,\n0\n],\n[\n51.389168,\n35.732643,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_3\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389168,\n35.732643,\n0\n],\n[\n51.38988,\n35.732745,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_4\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38988,\n35.732745,\n0\n],\n[\n51.389926,\n35.732535,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_5\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389926,\n35.732535,\n0\n],\n[\n51.38928,\n35.732442,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_6\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389235,\n35.732652,\n0\n],\n[\n51.389249,\n35.732586,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_7\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389252,\n35.732573,\n0\n],\n[\n51.389309,\n35.732311,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_8\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389309,\n35.732311,\n0\n],\n[\n51.389242,\n35.732301,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_9\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389213,\n35.732435,\n0\n],\n[\n51.389047,\n35.732411,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_10\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389431,\n35.732573,\n0\n],\n[\n51.389436,\n35.732552,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_11\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389436,\n35.732552,\n0\n],\n[\n51.389859,\n35.732613,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_12\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389859,\n35.732613,\n0\n],\n[\n51.389855,\n35.732634,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_13\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389858,\n35.732621,\n0\n],\n[\n51.389905,\n35.732628,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_14\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389093,\n35.732418,\n0\n],\n[\n51.389051,\n35.732614,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_15\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389085,\n35.732619,\n0\n],\n[\n51.389096,\n35.732568,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_16\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389096,\n35.732568,\n0\n],\n[\n51.389182,\n35.732581,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_17\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389183,\n35.732577,\n0\n],\n[\n51.389312,\n35.732595,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_18\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389297,\n35.732661,\n0\n],\n[\n51.389315,\n35.732582,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_19\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389212,\n35.732567,\n0\n],\n[\n51.389224,\n35.732511,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_20\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389264,\n35.732517,\n0\n],\n[\n51.389214,\n35.73251,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_21\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389214,\n35.73251,\n0\n],\n[\n51.389243,\n35.732376,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_22\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389293,\n35.732384,\n0\n],\n[\n51.389226,\n35.732374,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_23\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389106,\n35.732528,\n0\n],\n[\n51.3891,\n35.732553,\n0\n],\n[\n51.389184,\n35.732565,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_24\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389106,\n35.732528,\n0\n],\n[\n51.389107,\n35.732528,\n0\n],\n[\n51.38913,\n35.732423,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_25\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389198,\n35.732503,\n0\n],\n[\n51.389115,\n35.732491,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_26\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389082,\n35.732471,\n0\n],\n[\n51.389118,\n35.732476,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_27\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389242,\n35.732301,\n0\n],\n[\n51.389184,\n35.732565,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_28\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389066,\n35.732544,\n0\n],\n[\n51.389101,\n35.732549,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_29\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389816,\n35.732736,\n0\n],\n[\n51.389838,\n35.732632,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_30\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389843,\n35.732611,\n0\n],\n[\n51.389861,\n35.732525,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_31\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38975,\n35.732726,\n0\n],\n[\n51.389773,\n35.732622,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_32\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389777,\n35.732601,\n0\n],\n[\n51.389796,\n35.732516,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_33\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389699,\n35.732667,\n0\n],\n[\n51.38971,\n35.732613,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_34\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389714,\n35.732592,\n0\n],\n[\n51.389732,\n35.732507,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_35\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389699,\n35.732667,\n0\n],\n[\n51.389633,\n35.732658,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_36\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389668,\n35.732498,\n0\n],\n[\n51.389649,\n35.732583,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_37\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389645,\n35.732604,\n0\n],\n[\n51.389641,\n35.732622,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_38\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389638,\n35.732637,\n0\n],\n[\n51.389622,\n35.732708,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_39\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389557,\n35.732699,\n0\n],\n[\n51.389572,\n35.732627,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_40\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389575,\n35.732612,\n0\n],\n[\n51.389579,\n35.732594,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_41\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389584,\n35.732573,\n0\n],\n[\n51.389602,\n35.732488,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_42\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389492,\n35.732689,\n0\n],\n[\n51.389515,\n35.732585,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_43\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389519,\n35.732564,\n0\n],\n[\n51.389538,\n35.732479,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_44\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389473,\n35.73247,\n0\n],\n[\n51.389454,\n35.732555,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_45\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38945,\n35.732576,\n0\n],\n[\n51.389446,\n35.732594,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_46\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38941,\n35.732461,\n0\n],\n[\n51.389387,\n35.732567,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_47\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389383,\n35.732585,\n0\n],\n[\n51.389364,\n35.732671,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_48\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389382,\n35.732592,\n0\n],\n[\n51.389212,\n35.732567,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_49\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38913,\n35.732625,\n0\n],\n[\n51.389141,\n35.732575,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_50\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389063,\n35.732555,\n0\n],\n[\n51.389018,\n35.732548,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_51\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389071,\n35.73252,\n0\n],\n[\n51.389025,\n35.732514,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_52\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389078,\n35.732488,\n0\n],\n[\n51.389032,\n35.732482,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_53\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38922,\n35.732483,\n0\n],\n[\n51.38927,\n35.732491,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_54\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389285,\n35.732419,\n0\n],\n[\n51.389235,\n35.732412,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_55\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389239,\n35.732313,\n0\n],\n[\n51.389306,\n35.732322,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_56\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389383,\n35.732585,\n0\n],\n[\n51.389707,\n35.732631,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_57\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389572,\n35.732627,\n0\n],\n[\n51.389703,\n35.732646,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_58\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389059,\n35.732573,\n0\n],\n[\n51.389094,\n35.732578,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_59\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389609,\n35.732577,\n0\n],\n[\n51.389614,\n35.732552,\n0\n],\n[\n51.389655,\n35.732558,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_60\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389222,\n35.732521,\n0\n],\n[\n51.389262,\n35.732527,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_61\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389598,\n35.732631,\n0\n],\n[\n51.389602,\n35.732616,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_62\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389632,\n35.732709,\n0\n],\n[\n51.38963,\n35.73272,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_63\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38963,\n35.73272,\n0\n],\n[\n51.389673,\n35.732726,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_64\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389673,\n35.732726,\n0\n],\n[\n51.389675,\n35.732715,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_65\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.389903,\n35.732641,\n0\n],\n[\n51.389217,\n35.732543,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_66\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"LineString\",\n\"coordinates\":[\n[\n51.38929,\n35.732553,\n0\n],\n[\n51.389284,\n35.732578,\n0\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_67\",\n\"styleUrl\":\"#line-BDBDBD-1000-nodesc\",\n\"styleHash\":\"d23e901\",\n\"styleMapHash\":{\n\"normal\":\"#line-BDBDBD-1000-nodesc-normal\",\n\"highlight\":\"#line-BDBDBD-1000-nodesc-highlight\"\n},\n\"stroke\":\"#bdbdbd\",\n\"stroke-opacity\":1,\n\"stroke-width\":1\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389859,\n35.732613,\n0\n],\n[\n51.389855,\n35.732634,\n0\n],\n[\n51.389579,\n35.732594,\n0\n],\n[\n51.389584,\n35.732573,\n0\n],\n[\n51.389859,\n35.732613,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"1\",\n\"styleUrl\":\"#poly-FFFFFF-1000-227-nodesc\",\n\"styleHash\":\"-2e90c6a3\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FFFFFF-1000-227-nodesc-normal\",\n\"highlight\":\"#poly-FFFFFF-1000-227-nodesc-highlight\"\n},\n\"stroke\":\"#ffffff\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ffffff\",\n\"fill-opacity\":0.8901960784313725\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389094,\n35.732578,\n0\n],\n[\n51.389059,\n35.732573,\n0\n],\n[\n51.389066,\n35.732544,\n0\n],\n[\n51.389101,\n35.732549,\n0\n],\n[\n51.3891,\n35.732553,\n0\n],\n[\n51.389184,\n35.732565,\n0\n],\n[\n51.389226,\n35.732374,\n0\n],\n[\n51.389243,\n35.732376,\n0\n],\n[\n51.389214,\n35.73251,\n0\n],\n[\n51.389224,\n35.732511,\n0\n],\n[\n51.389212,\n35.732567,\n0\n],\n[\n51.389315,\n35.732582,\n0\n],\n[\n51.389312,\n35.732595,\n0\n],\n[\n51.389183,\n35.732577,\n0\n],\n[\n51.389182,\n35.732581,\n0\n],\n[\n51.389096,\n35.732568,\n0\n],\n[\n51.389094,\n35.732578,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_44\",\n\"styleUrl\":\"#poly-FFFFFF-1000-224-nodesc\",\n\"styleHash\":\"1726dabd\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FFFFFF-1000-224-nodesc-normal\",\n\"highlight\":\"#poly-FFFFFF-1000-224-nodesc-highlight\"\n},\n\"stroke\":\"#ffffff\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ffffff\",\n\"fill-opacity\":0.8784313725490196\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389598,\n35.732631,\n0\n],\n[\n51.389602,\n35.732616,\n0\n],\n[\n51.389707,\n35.732631,\n0\n],\n[\n51.389703,\n35.732646,\n0\n],\n[\n51.389598,\n35.732631,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_73\",\n\"styleUrl\":\"#poly-FFFFFF-1000-224-nodesc\",\n\"styleHash\":\"1726dabd\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FFFFFF-1000-224-nodesc-normal\",\n\"highlight\":\"#poly-FFFFFF-1000-224-nodesc-highlight\"\n},\n\"stroke\":\"#ffffff\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ffffff\",\n\"fill-opacity\":0.8784313725490196\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389431,\n35.732573,\n0\n],\n[\n51.389436,\n35.732552,\n0\n],\n[\n51.389584,\n35.732573,\n0\n],\n[\n51.389579,\n35.732594,\n0\n],\n[\n51.389431,\n35.732573,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"0\",\n\"styleUrl\":\"#poly-000000-1200-77-nodesc\",\n\"styleHash\":\"4fc78414\",\n\"styleMapHash\":{\n\"normal\":\"#poly-000000-1200-77-nodesc-normal\",\n\"highlight\":\"#poly-000000-1200-77-nodesc-highlight\"\n},\n\"stroke\":\"#000000\",\n\"stroke-opacity\":1,\n\"stroke-width\":1.2,\n\"fill\":\"#000000\",\n\"fill-opacity\":0.30196078431372547\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38988,\n35.732745,\n0\n],\n[\n51.389816,\n35.732736,\n0\n],\n[\n51.389838,\n35.732632,\n0\n],\n[\n51.389903,\n35.732641,\n0\n],\n[\n51.38988,\n35.732745,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"230\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389816,\n35.732736,\n0\n],\n[\n51.38975,\n35.732726,\n0\n],\n[\n51.389773,\n35.732622,\n0\n],\n[\n51.389838,\n35.732632,\n0\n],\n[\n51.389816,\n35.732736,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"242\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389557,\n35.732699,\n0\n],\n[\n51.389572,\n35.732627,\n0\n],\n[\n51.389638,\n35.732637,\n0\n],\n[\n51.389622,\n35.732708,\n0\n],\n[\n51.389557,\n35.732699,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"240\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389364,\n35.732671,\n0\n],\n[\n51.389383,\n35.732585,\n0\n],\n[\n51.389511,\n35.732603,\n0\n],\n[\n51.389492,\n35.732689,\n0\n],\n[\n51.389364,\n35.732671,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389364,\n35.732671,\n0\n],\n[\n51.389297,\n35.732661,\n0\n],\n[\n51.389315,\n35.732582,\n0\n],\n[\n51.389382,\n35.732592,\n0\n],\n[\n51.389364,\n35.732671,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389297,\n35.732661,\n0\n],\n[\n51.389235,\n35.732652,\n0\n],\n[\n51.389249,\n35.732586,\n0\n],\n[\n51.389312,\n35.732595,\n0\n],\n[\n51.389297,\n35.732661,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389235,\n35.732652,\n0\n],\n[\n51.389168,\n35.732643,\n0\n],\n[\n51.389183,\n35.732577,\n0\n],\n[\n51.389249,\n35.732586,\n0\n],\n[\n51.389235,\n35.732652,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389171,\n35.732631,\n0\n],\n[\n51.38913,\n35.732625,\n0\n],\n[\n51.389141,\n35.732575,\n0\n],\n[\n51.389182,\n35.732581,\n0\n],\n[\n51.389171,\n35.732631,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38913,\n35.732625,\n0\n],\n[\n51.389085,\n35.732619,\n0\n],\n[\n51.389096,\n35.732568,\n0\n],\n[\n51.389141,\n35.732575,\n0\n],\n[\n51.38913,\n35.732625,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389051,\n35.732614,\n0\n],\n[\n51.389005,\n35.732607,\n0\n],\n[\n51.389018,\n35.732548,\n0\n],\n[\n51.389063,\n35.732555,\n0\n],\n[\n51.389051,\n35.732614,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389018,\n35.732548,\n0\n],\n[\n51.389025,\n35.732514,\n0\n],\n[\n51.389071,\n35.73252,\n0\n],\n[\n51.389063,\n35.732555,\n0\n],\n[\n51.389018,\n35.732548,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389032,\n35.732482,\n0\n],\n[\n51.389078,\n35.732488,\n0\n],\n[\n51.389071,\n35.73252,\n0\n],\n[\n51.389025,\n35.732514,\n0\n],\n[\n51.389032,\n35.732482,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389078,\n35.732488,\n0\n],\n[\n51.389032,\n35.732482,\n0\n],\n[\n51.389047,\n35.732411,\n0\n],\n[\n51.389093,\n35.732418,\n0\n],\n[\n51.389078,\n35.732488,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389093,\n35.732418,\n0\n],\n[\n51.38913,\n35.732423,\n0\n],\n[\n51.389118,\n35.732476,\n0\n],\n[\n51.389082,\n35.732471,\n0\n],\n[\n51.389093,\n35.732418,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38913,\n35.732423,\n0\n],\n[\n51.389213,\n35.732435,\n0\n],\n[\n51.389198,\n35.732503,\n0\n],\n[\n51.389115,\n35.732491,\n0\n],\n[\n51.38913,\n35.732423,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389242,\n35.732301,\n0\n],\n[\n51.389309,\n35.732311,\n0\n],\n[\n51.389306,\n35.732322,\n0\n],\n[\n51.389239,\n35.732313,\n0\n],\n[\n51.389242,\n35.732301,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389306,\n35.732322,\n0\n],\n[\n51.389293,\n35.732384,\n0\n],\n[\n51.389226,\n35.732374,\n0\n],\n[\n51.389239,\n35.732313,\n0\n],\n[\n51.389306,\n35.732322,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389285,\n35.732419,\n0\n],\n[\n51.389235,\n35.732412,\n0\n],\n[\n51.389243,\n35.732376,\n0\n],\n[\n51.389293,\n35.732384,\n0\n],\n[\n51.389285,\n35.732419,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389285,\n35.732419,\n0\n],\n[\n51.38927,\n35.732491,\n0\n],\n[\n51.38922,\n35.732483,\n0\n],\n[\n51.389235,\n35.732412,\n0\n],\n[\n51.389285,\n35.732419,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38927,\n35.732491,\n0\n],\n[\n51.389264,\n35.732517,\n0\n],\n[\n51.389214,\n35.73251,\n0\n],\n[\n51.38922,\n35.732483,\n0\n],\n[\n51.38927,\n35.732491,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389861,\n35.732525,\n0\n],\n[\n51.389843,\n35.732611,\n0\n],\n[\n51.389777,\n35.732601,\n0\n],\n[\n51.389796,\n35.732516,\n0\n],\n[\n51.389861,\n35.732525,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"7\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389796,\n35.732516,\n0\n],\n[\n51.389777,\n35.732601,\n0\n],\n[\n51.389714,\n35.732592,\n0\n],\n[\n51.389732,\n35.732507,\n0\n],\n[\n51.389796,\n35.732516,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"6\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389732,\n35.732507,\n0\n],\n[\n51.389714,\n35.732592,\n0\n],\n[\n51.389649,\n35.732583,\n0\n],\n[\n51.389668,\n35.732498,\n0\n],\n[\n51.389732,\n35.732507,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"5\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389584,\n35.732573,\n0\n],\n[\n51.389519,\n35.732564,\n0\n],\n[\n51.389538,\n35.732479,\n0\n],\n[\n51.389602,\n35.732488,\n0\n],\n[\n51.389584,\n35.732573,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"3\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389519,\n35.732564,\n0\n],\n[\n51.389454,\n35.732555,\n0\n],\n[\n51.389473,\n35.73247,\n0\n],\n[\n51.389538,\n35.732479,\n0\n],\n[\n51.389519,\n35.732564,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"2\",\n\"styleUrl\":\"#poly-F9A825-1000-46\",\n\"styleHash\":\"45021f93\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-highlight\"\n},\n\"description\":\"SarveenTech\",\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38975,\n35.732726,\n0\n],\n[\n51.389622,\n35.732708,\n0\n],\n[\n51.389633,\n35.732658,\n0\n],\n[\n51.389699,\n35.732667,\n0\n],\n[\n51.38971,\n35.732613,\n0\n],\n[\n51.389773,\n35.732622,\n0\n],\n[\n51.38975,\n35.732726,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"241\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389082,\n35.732471,\n0\n],\n[\n51.389118,\n35.732476,\n0\n],\n[\n51.389107,\n35.732528,\n0\n],\n[\n51.389106,\n35.732528,\n0\n],\n[\n51.389101,\n35.732549,\n0\n],\n[\n51.389066,\n35.732544,\n0\n],\n[\n51.389082,\n35.732471,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"203\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389858,\n35.732621,\n0\n],\n[\n51.389859,\n35.732613,\n0\n],\n[\n51.389843,\n35.732611,\n0\n],\n[\n51.389861,\n35.732525,\n0\n],\n[\n51.389926,\n35.732535,\n0\n],\n[\n51.389905,\n35.732628,\n0\n],\n[\n51.389858,\n35.732621,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"8\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389609,\n35.732577,\n0\n],\n[\n51.389584,\n35.732573,\n0\n],\n[\n51.389602,\n35.732488,\n0\n],\n[\n51.389668,\n35.732498,\n0\n],\n[\n51.389655,\n35.732558,\n0\n],\n[\n51.389614,\n35.732552,\n0\n],\n[\n51.389609,\n35.732577,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"4\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389431,\n35.732573,\n0\n],\n[\n51.389387,\n35.732567,\n0\n],\n[\n51.38941,\n35.732461,\n0\n],\n[\n51.389473,\n35.73247,\n0\n],\n[\n51.389454,\n35.732555,\n0\n],\n[\n51.389436,\n35.732552,\n0\n],\n[\n51.389431,\n35.732573,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"5\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389602,\n35.732616,\n0\n],\n[\n51.389598,\n35.732631,\n0\n],\n[\n51.389572,\n35.732627,\n0\n],\n[\n51.389557,\n35.732699,\n0\n],\n[\n51.389492,\n35.732689,\n0\n],\n[\n51.389511,\n35.732603,\n0\n],\n[\n51.389602,\n35.732616,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"6\",\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.3891,\n35.732553,\n0\n],\n[\n51.389106,\n35.732528,\n0\n],\n[\n51.389107,\n35.732528,\n0\n],\n[\n51.389115,\n35.732491,\n0\n],\n[\n51.389198,\n35.732503,\n0\n],\n[\n51.389184,\n35.732565,\n0\n],\n[\n51.3891,\n35.732553,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-F9A825-1000-46-nodesc\",\n\"styleHash\":\"-1681267c\",\n\"styleMapHash\":{\n\"normal\":\"#poly-F9A825-1000-46-nodesc-normal\",\n\"highlight\":\"#poly-F9A825-1000-46-nodesc-highlight\"\n},\n\"stroke\":\"#f9a825\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#f9a825\",\n\"fill-opacity\":0.1803921568627451\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389085,\n35.732619,\n0\n],\n[\n51.389051,\n35.732614,\n0\n],\n[\n51.389059,\n35.732573,\n0\n],\n[\n51.389094,\n35.732578,\n0\n],\n[\n51.389085,\n35.732619,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_84\",\n\"styleUrl\":\"#poly-757575-1000-76-nodesc\",\n\"styleHash\":\"4f8f6258\",\n\"styleMapHash\":{\n\"normal\":\"#poly-757575-1000-76-nodesc-normal\",\n\"highlight\":\"#poly-757575-1000-76-nodesc-highlight\"\n},\n\"stroke\":\"#757575\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#757575\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389675,\n35.732715,\n0\n],\n[\n51.389673,\n35.732726,\n0\n],\n[\n51.38963,\n35.73272,\n0\n],\n[\n51.389632,\n35.732709,\n0\n],\n[\n51.389675,\n35.732715,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_72\",\n\"styleUrl\":\"#poly-757575-1000-76-nodesc\",\n\"styleHash\":\"4f8f6258\",\n\"styleMapHash\":{\n\"normal\":\"#poly-757575-1000-76-nodesc-normal\",\n\"highlight\":\"#poly-757575-1000-76-nodesc-highlight\"\n},\n\"stroke\":\"#757575\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#757575\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389638,\n35.732637,\n0\n],\n[\n51.389703,\n35.732646,\n0\n],\n[\n51.389699,\n35.732667,\n0\n],\n[\n51.389633,\n35.732658,\n0\n],\n[\n51.389638,\n35.732637,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_71\",\n\"styleUrl\":\"#poly-757575-1000-76-nodesc\",\n\"styleHash\":\"4f8f6258\",\n\"styleMapHash\":{\n\"normal\":\"#poly-757575-1000-76-nodesc-normal\",\n\"highlight\":\"#poly-757575-1000-76-nodesc-highlight\"\n},\n\"stroke\":\"#757575\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#757575\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389649,\n35.732583,\n0\n],\n[\n51.389609,\n35.732577,\n0\n],\n[\n51.389614,\n35.732552,\n0\n],\n[\n51.389655,\n35.732558,\n0\n],\n[\n51.389649,\n35.732583,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_106\",\n\"styleUrl\":\"#poly-757575-1000-76-nodesc\",\n\"styleHash\":\"4f8f6258\",\n\"styleMapHash\":{\n\"normal\":\"#poly-757575-1000-76-nodesc-normal\",\n\"highlight\":\"#poly-757575-1000-76-nodesc-highlight\"\n},\n\"stroke\":\"#757575\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#757575\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389252,\n35.732573,\n0\n],\n[\n51.389257,\n35.732548,\n0\n],\n[\n51.38929,\n35.732553,\n0\n],\n[\n51.389284,\n35.732578,\n0\n],\n[\n51.389252,\n35.732573,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml-107\",\n\"styleUrl\":\"#poly-757575-1000-76-nodesc\",\n\"styleHash\":\"4f8f6258\",\n\"styleMapHash\":{\n\"normal\":\"#poly-757575-1000-76-nodesc-normal\",\n\"highlight\":\"#poly-757575-1000-76-nodesc-highlight\"\n},\n\"stroke\":\"#757575\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#757575\",\n\"fill-opacity\":0.2980392156862745\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389257,\n35.732548,\n0\n],\n[\n51.389252,\n35.732573,\n0\n],\n[\n51.389212,\n35.732567,\n0\n],\n[\n51.389217,\n35.732543,\n0\n],\n[\n51.389257,\n35.732548,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_100\",\n\"styleUrl\":\"#poly-FF5252-1000-135-nodesc\",\n\"styleHash\":\"5b9997da\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FF5252-1000-135-nodesc-normal\",\n\"highlight\":\"#poly-FF5252-1000-135-nodesc-highlight\"\n},\n\"stroke\":\"#ff5252\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ff5252\",\n\"fill-opacity\":0.5294117647058824\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389262,\n35.732527,\n0\n],\n[\n51.389257,\n35.732548,\n0\n],\n[\n51.389217,\n35.732543,\n0\n],\n[\n51.389222,\n35.732521,\n0\n],\n[\n51.389262,\n35.732527,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_99\",\n\"styleUrl\":\"#poly-FF5252-1000-135-nodesc\",\n\"styleHash\":\"5b9997da\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FF5252-1000-135-nodesc-normal\",\n\"highlight\":\"#poly-FF5252-1000-135-nodesc-highlight\"\n},\n\"stroke\":\"#ff5252\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ff5252\",\n\"fill-opacity\":0.5294117647058824\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389264,\n35.732517,\n0\n],\n[\n51.389262,\n35.732527,\n0\n],\n[\n51.389222,\n35.732521,\n0\n],\n[\n51.389224,\n35.732511,\n0\n],\n[\n51.389264,\n35.732517,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"kml_98\",\n\"styleUrl\":\"#poly-FF5252-1000-135-nodesc\",\n\"styleHash\":\"5b9997da\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FF5252-1000-135-nodesc-normal\",\n\"highlight\":\"#poly-FF5252-1000-135-nodesc-highlight\"\n},\n\"stroke\":\"#ff5252\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ff5252\",\n\"fill-opacity\":0.5294117647058824\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389855,\n35.732634,\n0\n],\n[\n51.389858,\n35.732621,\n0\n],\n[\n51.389905,\n35.732628,\n0\n],\n[\n51.389903,\n35.732641,\n0\n],\n[\n51.389855,\n35.732634,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"234\",\n\"styleUrl\":\"#poly-FF5252-1000-135-nodesc\",\n\"styleHash\":\"5b9997da\",\n\"styleMapHash\":{\n\"normal\":\"#poly-FF5252-1000-135-nodesc-normal\",\n\"highlight\":\"#poly-FF5252-1000-135-nodesc-highlight\"\n},\n\"stroke\":\"#ff5252\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#ff5252\",\n\"fill-opacity\":0.5294117647058824\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38971,\n35.732613,\n0\n],\n[\n51.389707,\n35.732631,\n0\n],\n[\n51.389641,\n35.732622,\n0\n],\n[\n51.389645,\n35.732604,\n0\n],\n[\n51.38971,\n35.732613,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389645,\n35.732604,\n0\n],\n[\n51.389641,\n35.732622,\n0\n],\n[\n51.389575,\n35.732612,\n0\n],\n[\n51.389579,\n35.732594,\n0\n],\n[\n51.389645,\n35.732604,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"236\",\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389575,\n35.732612,\n0\n],\n[\n51.389511,\n35.732603,\n0\n],\n[\n51.389515,\n35.732585,\n0\n],\n[\n51.389579,\n35.732594,\n0\n],\n[\n51.389575,\n35.732612,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"237\",\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389515,\n35.732585,\n0\n],\n[\n51.389511,\n35.732603,\n0\n],\n[\n51.389446,\n35.732594,\n0\n],\n[\n51.38945,\n35.732576,\n0\n],\n[\n51.389515,\n35.732585,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"238\",\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.38929,\n35.732553,\n0\n],\n[\n51.38945,\n35.732576,\n0\n],\n[\n51.389446,\n35.732594,\n0\n],\n[\n51.389383,\n35.732585,\n0\n],\n[\n51.389382,\n35.732592,\n0\n],\n[\n51.389284,\n35.732578,\n0\n],\n[\n51.38929,\n35.732553,\n0\n]\n]\n]\n},\n\"properties\":{\n\"name\":\"220\",\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n},\n{\n\"type\":\"Feature\",\n\"geometry\":{\n\"type\":\"Polygon\",\n\"coordinates\":[\n[\n[\n51.389257,\n35.732548,\n0\n],\n[\n51.38928,\n35.732442,\n0\n],\n[\n51.38941,\n35.732461,\n0\n],\n[\n51.389387,\n35.732567,\n0\n],\n[\n51.389257,\n35.732548,\n0\n]\n]\n]\n},\n\"properties\":{\n\"styleUrl\":\"#poly-0097A7-1000-140-nodesc\",\n\"styleHash\":\"-2d650826\",\n\"styleMapHash\":{\n\"normal\":\"#poly-0097A7-1000-140-nodesc-normal\",\n\"highlight\":\"#poly-0097A7-1000-140-nodesc-highlight\"\n},\n\"stroke\":\"#0097a7\",\n\"stroke-opacity\":1,\n\"stroke-width\":1,\n\"fill\":\"#0097a7\",\n\"fill-opacity\":0.5490196078431373\n}\n}\n]\n}\n]";
            }
            return rawJsonGetMap;
        }

        public enum RoleType
        {
            RolesList_Farm,
            RolesList_Staff,
            RolesList_User
        }

        #region Notification
        public static String Today_Notification()
        {
            List<SmartCattle.Web.Controllers.NotificationController.NotificationsList> retNotificationList = new List<SmartCattle.Web.Controllers.NotificationController.NotificationsList>();
            ISession mContext = Context.Open();
            List<NotificationsTable> AllNotification = new List<NotificationsTable>();// mContext.QueryOver<NotificationsTable>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.uId).Desc.List<NotificationsTable>();
            List<NotificationsTable> _tmpAllNotification = new List<NotificationsTable>();

            List<int> AllCattles = mContext.QueryOver<NotificationsTable>().SelectList(item => item.SelectGroup(m => m.Cattle_Freestall_Id)).List<int>().ToList<int>();
            foreach (var cattle in AllCattles)
            {
                int CurrentFarmId = getCurrentFarmId();
                List<NotificationsTable> CattleNotifications = mContext.QueryOver<NotificationsTable>()
                    .Where(x => x.FarmID == CurrentFarmId)
                    .Where(x => x.Cattle_Freestall_Id == cattle)
                    .Where(x => x.CreatedDate >= DateTime.Now.AddHours(-12))
                    .OrderBy(x => x.CreatedDate).Desc.List().ToList();

                for (int i = 0; i < CattleNotifications.Count; i++)
                {
                    if (CattleNotifications[i].Act.Equals(SmartCattle.Web.Controllers.NotificationController.NotificationStatus.Act.Acted))
                    {
                        AllNotification.Add(CattleNotifications[i]);
                    }
                }
            }
            Context.Close(mContext);

            return AllNotification.Count.ToString();
        }

        public static String Total_Notification()
        {
            List<SmartCattle.Web.Controllers.NotificationController.NotificationsList> retNotificationList = new List<SmartCattle.Web.Controllers.NotificationController.NotificationsList>();
            ISession mContext = Context.Open();
            List<NotificationsTable> AllNotification = new List<NotificationsTable>();
            List<NotificationsTable> _tmpAllNotification = new List<NotificationsTable>();

            List<int> AllCattles = mContext.QueryOver<NotificationsTable>().SelectList(item => item.SelectGroup(m => m.Cattle_Freestall_Id)).List<int>().ToList<int>();
            foreach (var cattle in AllCattles)
            {
                int CurrentFarmId = getCurrentFarmId();
                List<NotificationsTable> CattleNotifications = mContext.QueryOver<NotificationsTable>()
                    .Where(x => x.FarmID == CurrentFarmId)
                    .Where(x => x.Cattle_Freestall_Id == cattle)
                    .OrderBy(x => x.CreatedDate).Desc.List().ToList();

                if (CattleNotifications.Count != 0)
                {
                    _tmpAllNotification.Add(CattleNotifications[0]);
                    AllNotification.Add(CattleNotifications[0]);
                    for (int i = 1; i < CattleNotifications.Count; i++)
                    {
                        if (CattleNotifications[i].Act.Equals(SmartCattle.Web.Controllers.NotificationController.NotificationStatus.Act.Acted))
                        {
                            AllNotification.Add(CattleNotifications[i]);
                        }
                    }
                }
            }
            Context.Close(mContext);

            return AllNotification.Count.ToString();
        }

        public static String Ackted_Notification()
        {
            List<SmartCattle.Web.Controllers.NotificationController.NotificationsList> retNotificationList = new List<SmartCattle.Web.Controllers.NotificationController.NotificationsList>();
            ISession mContext = Context.Open();
            List<NotificationsTable> AllNotification = new List<NotificationsTable>();// mContext.QueryOver<NotificationsTable>().Where(x => x.FarmID == Helper.Helper.getCurrentFarmId()).OrderBy(x => x.uId).Desc.List<NotificationsTable>();
            List<NotificationsTable> _tmpAllNotification = new List<NotificationsTable>();

            List<int> AllCattles = mContext.QueryOver<NotificationsTable>().SelectList(item => item.SelectGroup(m => m.Cattle_Freestall_Id)).List<int>().ToList<int>();
            foreach (var cattle in AllCattles)
            {
                int CurrentFarmId = getCurrentFarmId();
                List<NotificationsTable> CattleNotifications = mContext.QueryOver<NotificationsTable>()
                    .Where(x => x.FarmID == CurrentFarmId)
                    .Where(x => x.Cattle_Freestall_Id == cattle)
                    .OrderBy(x => x.CreatedDate).Desc.List().ToList();

                for (int i = 0; i < CattleNotifications.Count; i++)
                {
                    if (CattleNotifications[i].Act.Equals(SmartCattle.Web.Controllers.NotificationController.NotificationStatus.Act.Acted))
                    {
                        AllNotification.Add(CattleNotifications[i]);
                    }
                }
            }
            Context.Close(mContext);
            return AllNotification.Count.ToString();
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.APIs.Models
{
    public class SqlModels
    {
        public class ActivityStateTbl
        {
            public static string TABLE_NAME = "SmartCattle.ActivityStateTbl";

            public static string ID = "ID";
            public static string jsonedActivities = "jsonedActivities";
            public static string Sitting = "Sitting";
            public static string Standing = "Standing";
            public static string Walking = "Walking";
            public static string Eating = "Eating";
            public static string Rumination = "Rumination";
            public static string Drinking = "Drinking";
            public static string cattleId = "cattleId";
            public static string date = "date";
            public static string FarmID = "FarmID";
            public static string LastRecievedId = "LastRecievedId";
            public static string UserId = "UserId";

            public static int ID_INDEX = 0;
            public static int jsonedActivities_INDEX = 1;
            public static int Sitting_INDEX = 2;
            public static int Standing_INDEX = 3;
            public static int Walking_INDEX = 4;
            public static int Eating_INDEX = 5;
            public static int Rumination_INDEX = 6;
            public static int Drinking_INDEX = 7;
            public static int cattleId_INDEX = 8;
            public static int date_INDEX = 9;
            public static int FarmID_INDEX = 10;
            public static int LastRecievedId_INDEX = 11;
            public static int UserId_INDEX = 12;

            public class X_Model
            {
                public int ID { get; set; }
                public String jsonedActivities { get; set; }
                public decimal Sitting { get; set; }
                public decimal Standing { get; set; }
                public decimal Walking { get; set; }
                public decimal Eating { get; set; }
                public decimal Rumination { get; set; }
                public decimal Drinking { get; set; }
                public int cattleId { get; set; }
                public DateTime date { get; set; }
                public int FarmID { get; set; }
                public long LastRecievedId { get; set; }
                public String UserId { get; set; }
            }

            public class X_Values_DateTime
            {
                public List<X_Model> Values { get; set; }
                public List<DateTime> DateList { get; set; }
            }

            public static X_Values_DateTime Execute(X_DBASE x_dbase)
            {
                X_Values_DateTime ret = new X_Values_DateTime();
                List<X_Model> Values = new List<X_Model>();
                List<DateTime> DateList = new List<DateTime>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.Sitting = (decimal)reader[Sitting];
                    obj.Standing = (decimal)reader[Standing];
                    obj.Walking = (decimal)reader[Walking];
                    obj.Eating = (decimal)reader[Eating];
                    obj.Rumination = (decimal)reader[Rumination];
                    obj.Drinking = (decimal)reader[Drinking];
                    obj.cattleId = (int)reader[cattleId];
                    obj.date = (DateTime)reader[date];
                    obj.FarmID = (int)reader[FarmID];
                    obj.LastRecievedId = (long)reader[LastRecievedId];

                    DateList.Add(obj.date);
                    Values.Add(obj);
                }
                CloseDb(x_dbase);
                ret.Values = Values;
                ret.DateList = DateList;

                return ret;
            }
        }

        public class SensorTbl
        {
            public static string TABLE_NAME = "SmartCattle.SensorTbl";

            public static string ID = "ID";
            public static string MacAddress = "MacAddress";
            public static string cattleId = "cattleId";
            public static string lastTransmitDate = "lastTransmitDate";
            public static string activationDate = "activationDate";
            public static string linkingDate = "linkingDate";
            public static string lastSyncDate = "lastSyncDate";
            public static string antennaId = "antennaId";
            public static string antennaName = "antennaName";
            public static string status = "status";
            public static string softwareVersion = "softwareVersion";
            public static string FarmID = "FarmID";
            public static string UserId = "UserId";

            public static int ID_INDEX = 0;
            public static int MacAddress_INDEX = 1;
            public static int cattleId_INDEX = 2;
            public static int lastTransmitDate_INDEX = 3;
            public static int activationDate_INDEX = 4;
            public static int linkingDate_INDEX = 5;
            public static int lastSyncDate_INDEX = 6;
            public static int antennaId_INDEX = 7;
            public static int antennaName_INDEX = 8;
            public static int status_INDEX = 9;
            public static int softwareVersion_INDEX = 10;
            public static int FarmID_INDEX = 11;
            public static int UserId_INDEX = 12;

            public class X_Model
            {
                public int ID { get; set; }
                public String MacAddress { get; set; }
                public int cattleId { get; set; }
                public DateTime lastTransmitDate { get; set; }
                public DateTime activationDate { get; set; }
                public DateTime linkingDate { get; set; }
                public DateTime lastSyncDate { get; set; }
                public int antennaId { get; set; }
                public String antennaName { get; set; }
                public int status { get; set; }
                public String softwareVersion { get; set; }
                public int FarmID { get; set; }
                public String UserId { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.MacAddress = (String)reader[MacAddress];
                    obj.cattleId = (int)reader[cattleId];
                    obj.lastTransmitDate = (DateTime)reader[lastTransmitDate];
                    obj.activationDate = (DateTime)reader[activationDate];
                    obj.linkingDate = (DateTime)reader[linkingDate];
                    obj.lastSyncDate = (DateTime)reader[lastSyncDate];
                    obj.antennaId = (int)reader[antennaId];
                    obj.status = (int)reader[status];
                    obj.FarmID = (int)reader[FarmID];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class TempretureTbl
        {
            public static string TABLE_NAME = "SmartCattle.TempretureTbl";

            public static string ID = "ID";
            public static string value = "value";
            public static string point = "point";
            public static string cattleId = "cattleId";
            public static string date = "date";
            public static string LastRecievedId = "LastRecievedId";
            public static string FarmID = "FarmID";
            public static string UserId = "UserId";

            public static int ID_INDEX = 0;
            public static int value_INDEX = 1;
            public static int point_INDEX = 2;
            public static int cattleId_INDEX = 3;
            public static int date_INDEX = 4;
            public static int LastRecievedId_INDEX = 5;
            public static int FarmID_INDEX = 6;
            public static int UserId_INDEX = 7;

            public class X_Model
            {
                public int ID { get; set; }
                public double value { get; set; }
                public String point { get; set; }
                public int cattleId { get; set; }
                public DateTime date { get; set; }
                public String dateStr { get; set; }
                public long LastRecievedId { get; set; }
                public int FarmID { get; set; }
                public String UserId { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.value = (double)reader[value];
                    obj.cattleId = (int)reader[cattleId];
                    obj.date = (DateTime)reader[date];
                    obj.LastRecievedId = (long)reader[LastRecievedId];
                    obj.FarmID = (int)reader[FarmID];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class CattlePositionTbl
        {
            public static string TABLE_NAME = "SmartCattle.CattlePositionTbl";

            public static string ID = "ID";
            public static string value = "value";
            public static string x = "x";
            public static string y = "y";
            public static string Latitude = "Latitude";
            public static string Longitude = "Longitude";
            public static string LatLong = "LatLong";
            public static string cattleId = "cattleId";
            public static string date = "date";
            public static string LastRecievedId = "LastRecievedId";
            public static string FarmID = "FarmID";
            public static string UserId = "UserId";

            public static int ID_INDEX = 0;
            public static int value_INDEX = 1;
            public static int x_INDEX = 2;
            public static int y_INDEX = 3;
            public static int Latitude_INDEX = 4;
            public static int Longitude_INDEX = 5;
            public static int LatLong_INDEX = 6;
            public static int cattleId_INDEX = 7;
            public static int date_INDEX = 8;
            public static int LastRecievedId_INDEX = 9;
            public static int FarmID_INDEX = 10;
            public static int UserId_INDEX = 11;
        }

        public class CattleTHITbl
        {
            public static string TABLE_NAME = "SmartCattle.CattleTHITbl";

            public static string ID = "ID";
            public static string date = "date";
            public static string TdbValue = "TdbValue";
            public static string RHValue = "RHValue";
            public static string THIValue = "THIValue";
            public static string Location = "Location";
            public static string FreeStallId = "FreeStallId";
            public static string FarmID = "FarmID";
            public static string UserId = "UserId";
            public static string Cattle_ID = "Cattle_ID";

            public static int ID_INDEX = 0;
            public static int dat_INDEX = 1;
            public static int TdbValue_INDEX = 2;
            public static int RHValue_INDEX = 3;
            public static int THIValue_INDEX = 4;
            public static int Location_INDEX = 5;
            public static int FreeStallId_INDEX = 6;
            public static int FarmID_INDEX = 7;
            public static int UserId_INDEX = 8;
            public static int Cattle_ID_INDEX = 9;
        }

        public class EnvSensors
        {
            public static string TABLE_NAME = "SmartCattle.EnvSensors";

            public static string id = "id";
            public static string FreeStallId = "FreeStallId";
            public static string FarmId = "FarmId";
            public static string Lat = "Lat";
            public static string Lng = "Lng";
            public static string MAC = "MAC";

            public static int id_INDEX = 0;
            public static int FreeStallId_INDEX = 1;
            public static int FarmId_INDEX = 2;
            public static int Lat_INDEX = 3;
            public static int Lng_INDEX = 4;
            public static int MAC_INDEX = 5;

            public class X_Model
            {
                public int id { get; set; }
                public int FreeStallId { get; set; }
                public int FarmId { get; set; }
                public double Lat { get; set; }
                public double Lng { get; set; }
                public String MAC { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.id = (int)reader[id];
                    obj.FreeStallId = (int)reader[FreeStallId];
                    obj.FarmId = (int)reader[FarmId];
                    obj.Lat = (double)reader[Lat];
                    obj.Lng = (double)reader[Lng];
                    obj.MAC = (String)reader[MAC];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class EnvTHITbl
        {
            public static string TABLE_NAME = "SmartCattle.EnvTHITbl";

            public static string ID = "ID";
            public static string FarmID = "FarmID";
            public static string FreeStallId = "FreeStallId";
            public static string TdbValue = "TdbValue";
            public static string RHValue = "RHValue";
            public static string THIValue = "THIValue";
            public static string SensorLat = "SensorLat";
            public static string SensorLng = "SensorLng";
            public static string LastId = "LastId";
            public static string MAC = "MAC";
            public static string date = "date";

            public static int ID_INDEX = 0;
            public static int FarmID_INDEX = 1;
            public static int FreeStallId_INDEX = 2;
            public static int TdbValue_INDEX = 3;
            public static int RHValue_INDEX = 4;
            public static int THIValue_INDEX = 5;
            public static int SensorLat_INDEX = 6;
            public static int SensorLng_INDEX = 7;
            public static int LastId_INDEX = 8;
            public static int MAC_INDEX = 9;
            public static int date_INDEX = 10;

            public class X_Model
            {
                public int ID { get; set; }
                public int FarmID { get; set; }
                public int FreeStallId { get; set; }
                public double TdbValue { get; set; }
                public double RHValue { get; set; }
                public double THIValue { get; set; }
                public double SensorLat { get; set; }
                public double SensorLng { get; set; }
                public int LastId { get; set; }
                public String MAC { get; set; }
                public DateTime date { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.FarmID = (int)reader[FarmID];
                    obj.FreeStallId = (int)reader[FreeStallId];
                    obj.TdbValue = (double)reader[TdbValue];
                    obj.RHValue = (double)reader[RHValue];
                    obj.THIValue = (double)reader[THIValue];
                    obj.SensorLat = (double)reader[SensorLat];
                    obj.SensorLng = (double)reader[SensorLng];
                    obj.LastId = (int)reader[LastId];
                    obj.MAC = ((String)reader[MAC]).Replace(" ", "");
                    obj.date = (DateTime)reader[date];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class UserPermissions
        {
            public static string TABLE_NAME = "SmartCattle.UserPermissions";

            public static string ID = "ID";
            public static string Controller = "Controller";
            public static string Action = "Action";
            public static string Comment = "Comment";
            public static string UniqueId = "UniqueId";

            public static int ID_INDEX = 0;
            public static int Controller_INDEX = 1;
            public static int Action_INDEX = 2;
            public static int Comment_INDEX = 3;
            public static int UniqueId_INDEX = 4;

            public class X_Model
            {
                public int ID { get; set; }
                public String Controller { get; set; }
                public String Action { get; set; }
                public String Comment { get; set; }
                public String UniqueId { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.Controller = (String)reader[Controller];
                    obj.Action = (String)reader[Action];
                    obj.Comment = (String)reader[Comment];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class UserRoles
        {
            public static string TABLE_NAME = "SmartCattle.UserRoles";

            public static string ID = "ID";
            public static string Name = "Name";
            public static string Permissions = "Permissions";
            public static string Comment = "Comment";

            public static int ID_INDEX = 0;
            public static int Name_INDEX = 1;
            public static int Permissions_INDEX = 2;
            public static int Comment_INDEX = 3;

            public class X_Model
            {
                public int ID { get; set; }
                public String Name { get; set; }
                public String Permissions { get; set; }
                public String Comment { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.Name = (String)reader[Name];
                    obj.Permissions = (String)reader[Permissions];
                    obj.Comment = (String)reader[Comment];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class UserInfo
        {
            public static string TABLE_NAME = "SmartCattle.UserInfo";

            public static string ID = "ID";
            public static string Name = "Name";
            public static string Family = "Family";
            public static string Email = "Email";
            public static string Password = "Password";
            public static string Role = "Role";
            public static string Permissions = "Permissions";
            public static string FarmId = "FarmId";

            public static int ID_INDEX = 0;
            public static int Name_INDEX = 1;
            public static int Family_INDEX = 2;
            public static int Email_INDEX = 3;
            public static int Password_INDEX = 4;
            public static int Role_INDEX = 5;
            public static int Permissions_INDEX = 6;
            public static int FarmId_INDEX = 7;

            public class X_Model
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public string Family { get; set; }
                public string Email { get; set; }
                public string Password { get; set; }
                public string Role { get; set; }
                public string Permissions { get; set; }
                public int FarmId { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.Name = (String)reader[Name];
                    obj.Family = (String)reader[Family];
                    obj.Email = (String)reader[Email];
                    obj.Password = (String)reader[Password];
                    obj.Role = (String)reader[Role];
                    obj.Permissions = (String)reader[Permissions];
                    obj.FarmId = (int)reader[FarmId];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class CattleTbl
        {
            public static string TABLE_NAME = "SmartCattle.CattleTbl";

            public static string ID = "ID";
            public static string age = "age";
            public static string preg = "preg";
            public static string milkAvg = "milkAvg";
            public static string healthStatus = "healthStatus";
            public static string animalNumber = "animalNumber";
            public static string heatStatus = "heatStatus";
            public static string birthDate = "birthDate";
            public static string Dim = "Dim";
            public static string fertilityStatus = "fertilityStatus";
            public static string lactationNumber = "lactationNumber";
            public static string InseminationCount = "InseminationCount";
            public static string lastInseminationDate = "lastInseminationDate";
            public static string lastCalvingDate = "lastCalvingDate";
            public static string calvingCount = "calvingCount";
            public static string CattleGroupId = "CattleGroupId";
            public static string FreeStallId = "FreeStallId";
            public static string FarmID = "FarmID";
            public static string UserId = "UserId";
            public static string CattleHerd_ID = "CattleHerd_ID";

            public static int ID_INDEX = 0;
            public static int age_INDEX = 1;
            public static int preg_INDEX = 2;
            public static int milkAvg_INDEX = 3;
            public static int healthStatus_INDEX = 4;
            public static int animalNumber_INDEX = 5;
            public static int heatStatus_INDEX = 6;
            public static int birthDate_INDEX = 7;
            public static int Dim_INDEX = 8;
            public static int fertilityStatus_INDEX = 9;
            public static int lactationNumber_INDEX = 10;
            public static int InseminationCount_INDEX = 11;
            public static int lastInseminationDate_INDEX = 12;
            public static int lastCalvingDate_INDEX = 13;
            public static int calvingCount_INDEX = 14;
            public static int CattleGroupId_INDEX = 15;
            public static int FreeStallId_INDEX = 16;
            public static int FarmID_INDEX = 17;
            public static int UserId_INDEX = 18;
            public static int CattleHerd_ID_INDEX = 19;

            public class X_Model
            {
                public int ID { get; set; }
                public int age { get; set; }
                public int preg { get; set; }
                public float milkAvg { get; set; }
                public int healthStatus { get; set; }
                public int animalNumber { get; set; }
                public int heatStatus { get; set; }
                public DateTime birthDate { get; set; }
                public int Dim { get; set; }
                public int fertilityStatus { get; set; }
                public int lactationNumber { get; set; }
                public int InseminationCount { get; set; }
                public DateTime lastInseminationDate { get; set; }
                public DateTime lastCalvingDate { get; set; }
                public int calvingCount { get; set; }
                public int CattleGroupId { get; set; }
                public int FreeStallId { get; set; }
                public int FarmID { get; set; }
                public String UserId { get; set; }
                public int CattleHerd_ID { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    try { obj.ID = (int)(reader[ID]); } catch (Exception ex) { obj.ID = -1; }
                    try { obj.age = (int)reader[age]; } catch (Exception ex) { obj.age = -1; }
                    try { obj.preg = (int)reader[preg]; } catch (Exception ex) { obj.preg = -1; }
                    try { obj.milkAvg = (int)reader[milkAvg]; } catch (Exception ex) { obj.milkAvg = -1; }
                    try { obj.healthStatus = (int)reader[healthStatus]; } catch (Exception ex) { obj.healthStatus = -1; }
                    try { obj.animalNumber = (int)reader[animalNumber]; } catch (Exception ex) { obj.animalNumber = -1; }
                    try { obj.heatStatus = (int)reader[heatStatus]; } catch (Exception ex) { obj.heatStatus = -1; }
                    try { obj.birthDate = (DateTime)reader[birthDate]; } catch (Exception ex) { obj.birthDate = new DateTime(); }
                    try { obj.Dim = (int)reader[Dim]; } catch (Exception ex) { obj.Dim = -1; }
                    try { obj.fertilityStatus = (int)reader[fertilityStatus]; } catch (Exception ex) { obj.fertilityStatus = -1; }
                    try { obj.lactationNumber = (int)reader[lactationNumber]; } catch (Exception ex) { obj.lactationNumber = -1; }
                    try { obj.InseminationCount = (int)reader[InseminationCount]; } catch (Exception ex) { obj.InseminationCount = -1; }
                    try { obj.lastInseminationDate = (DateTime)reader[lastInseminationDate]; } catch (Exception ex) { obj.lastInseminationDate = new DateTime(); }
                    try { obj.lastCalvingDate = (DateTime)reader[lastCalvingDate]; } catch (Exception ex) { obj.lastCalvingDate = new DateTime(); }
                    try { obj.calvingCount = (int)reader[calvingCount]; } catch (Exception ex) { obj.calvingCount = -1; }
                    try { obj.CattleGroupId = (int)reader[CattleGroupId]; } catch (Exception ex) { obj.CattleGroupId = -1; }
                    try { obj.FreeStallId = (int)reader[FreeStallId]; } catch (Exception ex) { obj.FreeStallId = -1; }
                    try { obj.FarmID = (int)reader[FarmID]; } catch (Exception ex) { obj.FarmID = -1; }
                    try { obj.UserId = (String)reader[UserId]; } catch (Exception ex) { obj.UserId = "NaN"; }
                    try { obj.CattleHerd_ID = (int)reader[CattleHerd_ID]; } catch (Exception ex) { obj.CattleHerd_ID = -1; }

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class Notifications
        {
            public static string TABLE_NAME = "SmartCattle.Notifications";

            public static string ID = "ID";
            public static string Topic = "Topic";
            public static string Comment = "Comment";
            public static string FarmID = "FarmID";
            public static string RoleName = "RoleName";
            public static string CreatedDate = "CreatedDate";
            public static string Status = "Status";
            public static string NotificationType = "NotificationType";

            public static int ID_INDEX = 0;
            public static int Topic_INDEX = 1;
            public static int Comment_INDEX = 2;
            public static int FarmID_INDEX = 3;
            public static int RoleName_INDEX = 4;
            public static int CreatedDate_INDEX = 5;
            public static int Status_INDEX = 6;
            public static int NotificationType_INDEX = 7;

            public class X_Model
            {
                public int ID { get; set; }
                public String Topic { get; set; }
                public String Comment { get; set; }
                public int FarmID { get; set; }
                public String RoleName { get; set; }
                public DateTime CreatedDate { get; set; }
                public String Status { get; set; }
                public String NotificationType { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.Topic = (String)reader[Topic];
                    obj.Comment = (String)reader[Comment];
                    obj.FarmID = (int)reader[FarmID];
                    obj.RoleName = (String)reader[RoleName];
                    obj.CreatedDate = (DateTime)reader[CreatedDate];
                    obj.Status = (String)reader[Status];
                    obj.NotificationType = (String)reader[NotificationType];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class NotificationsSetting
        {
            public static string TABLE_NAME = "SmartCattle.NotificationsSetting";

            public static string ID = "ID";
            public static string FarmId = "FarmId";
            public static string GroupName = "GroupName";
            public static string Topic = "Topic";
            public static string Roles = "Roles";
            public static string Comment = "Comment";
            public static string Snooz = "Snooz";
            public static string PeroidTime = "PeroidTime";
            public static string WindowTime = "WindowTime";
            public static string CattleTemp = "CattleTemp";
            public static string Sitting = "Sitting";
            public static string Walking = "Walking";
            public static string Rumination = "Rumination";
            public static string Drinking = "Drinking";
            public static string Eating = "Eating";
            public static string Standing = "Standing";
            public static string Name = "Name";

            public static int ID_INDEX = 0;
            public static int FarmId_INDEX = 1;
            public static int GroupName_INDEX = 2;
            public static int Topic_INDEX = 3;
            public static int Roles_INDEX = 4;
            public static int Comment_INDEX = 5;
            public static int Snooz_INDEX = 6;
            public static int PeroidTime_INDEX = 7;
            public static int WindowTime_INDEX = 8;
            public static int CattleTemp_INDEX = 9;
            public static int Sitting_INDEX = 10;
            public static int Walking_INDEX = 11;
            public static int Rumination_INDEX = 12;
            public static int Drinking_INDEX = 13;
            public static int Eating_INDEX = 14;
            public static int Standing_INDEX = 15;
            public static int Name_INDEX = 16;

            public class X_Model
            {
                public int ID { get; set; }
                public int FarmId { get; set; }
                public String GroupName { get; set; }
                public String Topic { get; set; }
                public String Roles { get; set; }
                public String Comment { get; set; }
                public int Snooz { get; set; }
                public int PeroidTime { get; set; }
                public float WindowTime { get; set; }
                public float CattleTemp { get; set; }
                public float Sitting { get; set; }
                public float Walking { get; set; }
                public float Rumination { get; set; }
                public float Drinking { get; set; }
                public float Eating { get; set; }
                public float Standing { get; set; }
                public String Name { get; set; }
            }

            public static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    obj.ID = (int)reader[ID];
                    obj.FarmId = (int)reader[FarmId];
                    obj.GroupName = (String)reader[GroupName];
                    obj.Topic = (String)reader[Topic];
                    obj.Roles = (String)reader[Roles];
                    obj.Comment = (String)reader[Comment];
                    obj.Snooz = (int)reader[Snooz];
                    obj.PeroidTime = (int)reader[PeroidTime];
                    obj.WindowTime = (float)reader[WindowTime];
                    obj.CattleTemp = (float)reader[CattleTemp];
                    obj.Sitting = (float)reader[Sitting];
                    obj.Walking = (float)reader[Walking];
                    obj.Rumination = (float)reader[Rumination];
                    obj.Drinking = (float)reader[Drinking];
                    obj.Eating = (float)reader[Eating];
                    obj.Standing = (float)reader[Standing];
                    obj.Name = (String)reader[Name];

                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        private static void OpenDb(X_DBASE x_dbase)
        {
            if (x_dbase.sqlConnection.State != ConnectionState.Open)
            {
                x_dbase.sqlConnection.Open();
            }
        }

        private static void CloseDb(X_DBASE x_dbase)
        {
            if (x_dbase.sqlConnection.State == ConnectionState.Open)
            {
                x_dbase.sqlConnection.Close();
            }
        }
    }
}
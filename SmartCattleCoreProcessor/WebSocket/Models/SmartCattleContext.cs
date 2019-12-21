using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TableDependency;
using TableDependency.SqlClient;
using WebSocket.Helper;
using static WebSocket.Helper.DbHelper;

namespace WebSocket.Models
{
    public class SmartCattleContext
    {

        public UserInfo Account { get; set; }
        public CurrentValueTbl CurrentValue { get; set; }
        public TempretureTbl Tempreture { get; set; }
        public ActivityStateTbl Activity { get; set; }
        public CattleNotificationsSetting CattleNotificationSetting { get; set; }
        public NotificationsTbl Notifications { get; set; }

        public SmartCattleContext()
        {
            Account = new UserInfo();
            CurrentValue = new CurrentValueTbl();
            Tempreture = new TempretureTbl();
            Activity = new ActivityStateTbl();
            CattleNotificationSetting = new CattleNotificationsSetting();
            Notifications = new NotificationsTbl();
        }

        public class UserInfo
        {
            public FarmTbl Farm;
            private static List<XModels.RelationTbl> Rels;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public UserInfo()
            {
                Farm = new FarmTbl();
                Called = true;
                Rels = Relations.get("UserInfo", "FarmTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.UserInfo";

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

            public UserInfo SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public UserInfo Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public UserInfo Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public UserInfo Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public UserInfo Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public UserInfo Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public UserInfo Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public UserInfo Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public UserInfo Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public UserInfo Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                expression = DevHelper.ExpressionBuilder(_whereClause);
                result = RunSQLExpression(expression);
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class FarmTbl
        {
            public FreeStallTbl FreeStall;
            private static List<XModels.RelationTbl> Rels;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public FarmTbl()
            {
                FreeStall = new FreeStallTbl();
                Called = true;
                Rels = Relations.get("UserInfo", "FarmTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.FarmTbl";

            public class X_Model
            {
                public int ID { get; set; }
                public string name { get; set; }
                public string LogoUrl { get; set; }
                public string email { get; set; }
                public string website { get; set; }
                public string Latitude { get; set; }
                public string Longitude { get; set; }
                public string UserId { get; set; }
                public string MapGeoJson { get; set; }
                public string MapFilePath { get; set; }
            }

            public FarmTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public FarmTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public FarmTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public FarmTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public FarmTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public FarmTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public FarmTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public FarmTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public FarmTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public FarmTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                List<UserInfo.X_Model> ParentList;
                if (UserInfo.Called)
                {
                    if (Rels.Count != 0)
                    {
                        ParentList = UserInfo._ToList();
                        if (ParentList.Count != 0)
                        {
                            for (int i = 0; i < ParentList.Count; i++)
                            {
                                String Where_with_Rel = DevHelper.Where(Rels, _whereClause, ParentList[i]);
                                if (!Where_with_Rel.Contains("NOT_REL"))
                                {
                                    _WhereList.Add(Where_with_Rel);
                                }
                            }
                            _WhereList = _WhereList.Distinct().ToList();
                            for (int i = 0; i < _WhereList.Count; i++)
                            {
                                _whereClause._where = _WhereList[i];
                                expression = DevHelper.ExpressionBuilder(_whereClause);
                                result.AddRange(RunSQLExpression(expression));
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    expression = DevHelper.ExpressionBuilder(_whereClause);
                    result = RunSQLExpression(expression);
                }
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class FreeStallTbl
        {
            public CattleTbl Cattle;
            private static List<XModels.RelationTbl> Rels;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public FreeStallTbl()
            {
                Cattle = new CattleTbl();
                Called = true;
                Rels = Relations.get("FarmTbl", "FreeStallTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.FreeStallTbl";

            public class X_Model
            {
                public int ID { get; set; }
                public string name { get; set; }
                public string Description { get; set; }
                public int code { get; set; }
                public float location { get; set; }
                public int FarmID { get; set; }
                public int? GroupID { get; set; }
                public string UserId { get; set; }
            }

            public FreeStallTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public FreeStallTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public FreeStallTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public FreeStallTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public FreeStallTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public FreeStallTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public FreeStallTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public FreeStallTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public FreeStallTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public FreeStallTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                List<FarmTbl.X_Model> ParentList;
                if (FarmTbl.Called)
                {
                    if (Rels.Count != 0)
                    {
                        ParentList = FarmTbl._ToList();
                        if (ParentList.Count != 0)
                        {
                            for (int i = 0; i < ParentList.Count; i++)
                            {
                                _WhereList.Add(DevHelper.Where(Rels, _whereClause, ParentList[i]));
                            }
                            _WhereList = _WhereList.Distinct().ToList();
                            for (int i = 0; i < _WhereList.Count; i++)
                            {
                                _whereClause._where = _WhereList[i];
                                expression = DevHelper.ExpressionBuilder(_whereClause);
                                result.AddRange(RunSQLExpression(expression));
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    expression = DevHelper.ExpressionBuilder(_whereClause);
                    result = RunSQLExpression(expression);
                }
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class CattleTbl
        {
            public ActivityStateTbl Acitivty;
            public TempretureTbl Tempreture;
            private static List<XModels.RelationTbl> Rels;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public CattleTbl()
            {
                Acitivty = new ActivityStateTbl();
                Tempreture = new TempretureTbl();
                Called = false;
                Rels = Relations.get("FreeStallTbl", "CattleTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.CattleTbl";

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
                public DateTime? lastInseminationDate { get; set; }
                public DateTime? lastCalvingDate { get; set; }
                public int calvingCount { get; set; }
                public int? CattleGroupId { get; set; }
                public int? FreeStallId { get; set; }
                public int FarmID { get; set; }
                public string UserId { get; set; }
                public int? CattleHerd_ID { get; set; }
            }

            public CattleTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public CattleTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public CattleTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public CattleTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public CattleTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public CattleTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public CattleTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public CattleTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public CattleTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public CattleTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public static List<X_Model> ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                List<FreeStallTbl.X_Model> ParentList;
                if (FreeStallTbl.Called)
                {
                    if (Rels.Count != 0)
                    {
                        ParentList = FreeStallTbl._ToList();
                        if (ParentList.Count != 0)
                        {
                            for (int i = 0; i < ParentList.Count; i++)
                            {
                                _WhereList.Add(DevHelper.Where(Rels, _whereClause, ParentList[i]));
                            }
                            _WhereList = _WhereList.Distinct().ToList();
                            for (int i = 0; i < _WhereList.Count; i++)
                            {
                                _whereClause._where = _WhereList[i];
                                expression = DevHelper.ExpressionBuilder(_whereClause);
                                result.AddRange(RunSQLExpression(expression));
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    expression = DevHelper.ExpressionBuilder(_whereClause);
                    result = RunSQLExpression(expression);
                }
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class ActivityStateTbl
        {
            private static List<XModels.RelationTbl> Rels;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public ActivityStateTbl()
            {
                Called = true;
                Rels = Relations.get("CattleTbl", "ActivityStateTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.ActivityStateTbl";

            public class X_Model
            {
                public int ID { get; set; }
                public string jsonedActivities { get; set; }
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
                public string UserId { get; set; }
            }

            public ActivityStateTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public ActivityStateTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public ActivityStateTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public ActivityStateTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public ActivityStateTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public ActivityStateTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public ActivityStateTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public ActivityStateTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public ActivityStateTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public ActivityStateTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public static List<X_Model> ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                List<CattleTbl.X_Model> ParentList;
                if (CattleTbl.Called)
                {
                    if (Rels.Count != 0)
                    {
                        ParentList = CattleTbl.ToList();
                        if (ParentList.Count != 0)
                        {
                            for (int i = 0; i < ParentList.Count; i++)
                            {
                                _WhereList.Add(DevHelper.Where(Rels, _whereClause, ParentList[i]));
                            }
                            _WhereList = _WhereList.Distinct().ToList();
                            for (int i = 0; i < _WhereList.Count; i++)
                            {
                                _whereClause._where = _WhereList[i];
                                expression = DevHelper.ExpressionBuilder(_whereClause);
                                result.AddRange(RunSQLExpression(expression));
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    expression = DevHelper.ExpressionBuilder(_whereClause);
                    result = RunSQLExpression(expression);
                }
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class TempretureTbl
        {
            //public xChild mChild;
            private List<XModels.RelationTbl> Rels;
            private DevHelper.WhereClause _whereClause;
            public bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public TempretureTbl()
            {
                Called = true;
                Rels = Relations.get("CattleTbl", "TempretureTbl");
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.TempretureTbl";

            public class X_Model
            {
                public int ID { get; set; }
                public double value { get; set; }
                public string point { get; set; }
                public int cattleId { get; set; }
                public DateTime date { get; set; }
                public long LastRecievedId { get; set; }
                public int FarmID { get; set; }
                public string UserId { get; set; }
                public int FreeStall { get; set; }
            }

            public TempretureTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public TempretureTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public TempretureTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public TempretureTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public TempretureTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public TempretureTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public TempretureTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public TempretureTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public TempretureTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public TempretureTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                List<CattleTbl.X_Model> ParentList;
                if (CattleTbl.Called)
                {
                    if (Rels.Count != 0)
                    {
                        ParentList = CattleTbl.ToList();
                        if (ParentList.Count != 0)
                        {
                            for (int i = 0; i < ParentList.Count; i++)
                            {
                                _WhereList.Add(DevHelper.Where(Rels, _whereClause, ParentList[i]));
                            }
                            _WhereList = _WhereList.Distinct().ToList();
                            for (int i = 0; i < _WhereList.Count; i++)
                            {
                                _whereClause._where = _WhereList[i];
                                expression = DevHelper.ExpressionBuilder(_whereClause);
                                result.AddRange(RunSQLExpression(expression));
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    expression = DevHelper.ExpressionBuilder(_whereClause);
                    result = RunSQLExpression(expression);
                }
                return result;
            }

            private List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception ex)
                        {
                            String exp = ex.Message;
                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class CurrentValueTbl
        {
            //public Child mChild;
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public CurrentValueTbl()
            {
                //mChild = new Child();
                Called = true;
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.CurrentValue";

            public class X_Model
            {
                public int ID { get; set; }
                public string ValueName { get; set; }
                public double Value { get; set; }
                public DateTime? DateTime { get; set; }
                public int FarmId { get; set; }
            }

            public CurrentValueTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public CurrentValueTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where = DevHelper.Where(expr);
                return this;
            }

            public CurrentValueTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public CurrentValueTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public CurrentValueTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public CurrentValueTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public CurrentValueTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public CurrentValueTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public CurrentValueTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public CurrentValueTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                expression = DevHelper.ExpressionBuilder(_whereClause);
                result = RunSQLExpression(expression);
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var value = reader[propertyInfo.Name];
                            propertyInfo.SetValue(obj, value);
                        }
                        catch (Exception ex)
                        {
                            String ack = ex.Message;
                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }

        }

        public class CattleNotificationsSetting
        {
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public CattleNotificationsSetting()
            {
                Called = true;
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.CattleNotificationsSetting";

            public class X_Model
            {
                public int ID { get; set; }
                public int? FarmId { get; set; }
                public string GroupName { get; set; }
                public string Topic { get; set; }
                public string Roles { get; set; }
                public string Comment { get; set; }
                public String SnoozeTime { get; set; }
                public int? PeroidTime { get; set; }
                public int? WindowTime { get; set; }
                public double? CattleTempMin { get; set; }
                public double? CattleTempMax { get; set; }
                public double? SittingMin { get; set; }
                public double? SittingMax { get; set; }
                public double? WalkingMin { get; set; }
                public double? WalkingMax { get; set; }
                public double? RuminationMin { get; set; }
                public double? RuminationMax { get; set; }
                public double? DrinkingMin { get; set; }
                public double? DrinkingMax { get; set; }
                public double? EatingMin { get; set; }
                public double? EatingMax { get; set; }
                public double? StandingMin { get; set; }
                public double? StandingMax { get; set; }
                public DateTime? CreateDate { get; set; }
                public String ActivationState { get; set; }
            }

            public CattleNotificationsSetting SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public CattleNotificationsSetting Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public CattleNotificationsSetting Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public CattleNotificationsSetting Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public CattleNotificationsSetting Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public CattleNotificationsSetting Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public CattleNotificationsSetting Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public CattleNotificationsSetting Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public CattleNotificationsSetting Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public CattleNotificationsSetting Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                expression = DevHelper.ExpressionBuilder(_whereClause);
                result = RunSQLExpression(expression);
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception ex)
                        {
                            String Ack = ex.Message;
                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }

        public class NotificationsTbl
        {
            private static DevHelper.WhereClause _whereClause;
            public static bool Called = false;
            private ModelToTableMapper<X_Model> mapper;

            public NotificationsTbl()
            {
                Called = true;
                _whereClause = new DevHelper.WhereClause();
                _whereClause.TABLE_NAME = TABLE_NAME;
                mapper = new ModelToTableMapper<X_Model>();
            }

            public static string TABLE_NAME = "SmartCattle.Notifications";

            public class X_Model
            {
                public int ID { get; set; }
                public string Topic { get; set; }
                public string Comment { get; set; }
                public int FarmID { get; set; }
                public string RoleName { get; set; }
                public string CreatedDate { get; set; }
                public string Status { get; set; }
                public string NotificationType { get; set; }
                public int Snooze { get; set; }
                public string TagName { get; set; }
                public String SnoozeMsg { get; set; }
            }

            public NotificationsTbl SetListener(Expression<Func<X_Model, object>> expr)
            {
                String Keyword = DevHelper.SetListener(expr);
                mapper.AddMapping(expr, Keyword);
                return this;
            }

            public SqlTableDependency<X_Model> SetOnChange()
            {
                String _con = Config.ConnectionString;
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(_con, TABLE_NAME, mapper);
                return dep;
            }

            public NotificationsTbl Where(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._where += DevHelper.Where(expr);
                return this;
            }

            public NotificationsTbl Update(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Update ";
                _whereClause._update = DevHelper.Update(expr);
                return this;
            }

            public NotificationsTbl Insert(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._Action = "Insert ";
                _whereClause._insert += DevHelper.Insert(expr);
                return this;
            }

            public NotificationsTbl Delete()
            {
                _whereClause._Action = "Delete ";
                return this;
            }

            public NotificationsTbl Include(Expression<Func<X_Model, bool>> expr)
            {
                _whereClause._include += DevHelper.Include(expr);
                return this;
            }

            public NotificationsTbl Offset(int _offset)
            {
                _whereClause._offset = " ORDER BY " + new X_Model().ID + " offset " + _offset.ToString() + " ROWS ";
                return this;
            }

            public NotificationsTbl Limit(int _limit)
            {
                _whereClause._limit = " FETCH NEXT " + _limit.ToString() + " ROWS ONLY ";
                return this;
            }

            public NotificationsTbl Descending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Descending(expr);
                return this;
            }

            public NotificationsTbl Ascending(Expression<Func<X_Model, object>> expr)
            {
                _whereClause._orderby = DevHelper.Ascending(expr);
                return this;
            }

            public List<X_Model> ToList()
            {
                return _ToList();
            }

            public static List<X_Model> _ToList()
            {
                String expression = "";
                List<String> _WhereList = new List<String>();
                List<X_Model> result = new List<X_Model>();
                expression = DevHelper.ExpressionBuilder(_whereClause);
                result = RunSQLExpression(expression);
                return result;
            }

            private static List<X_Model> RunSQLExpression(String expression)
            {
                List<X_Model> result = new List<X_Model>();
                DbHelper dbase = new DbHelper();
                X_DBASE x_dbase = dbase.Initial();
                x_dbase.sqlCommand.CommandText = expression;
                result = Execute(x_dbase);
                return result;
            }

            private static List<X_Model> Execute(X_DBASE x_dbase)
            {
                List<X_Model> ret = new List<X_Model>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    X_Model obj = new X_Model();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        try
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }
    }
}

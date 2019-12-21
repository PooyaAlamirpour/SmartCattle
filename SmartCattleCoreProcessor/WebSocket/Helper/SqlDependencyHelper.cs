using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TableDependency;
using TableDependency.SqlClient;

namespace WebSocket.Helper
{
    public class SqlDependencyHelper
    {
        public CurrentValue currentValue;

        public SqlDependencyHelper()
        {
            currentValue = new CurrentValue();
        }

        public class CurrentValue
        {
            private ModelToTableMapper<X_Model> mapper = new ModelToTableMapper<X_Model>();
            public static string TABLE_NAME = "SmartCattle.CurrentValue";

            public static string ID = "ID";
            public static string ValueName = "ValueName";
            public static string Value = "Value";
            public static string DateTime = "DateTime";
            public static string FarmId = "FarmId";

            public class X_Model
            {
                public int ID { get; set; }
                public string ValueName { get; set; }
                public float? Value { get; set; }
                public DateTime? DateTime { get; set; }
                public int? FarmId { get; set; }
            }

            public CurrentValue SetListenerOn(Expression<Func<X_Model, object>> expr)
            {
                var paramName = expr.Parameters[0].Name;
                String tmp = expr.Body.ToString().Replace("Convert", "").Replace("(", "").Replace(")", "").Replace(paramName + ".", "");
                PropertyInfo result = typeof(X_Model).GetProperty(tmp);
                mapper.AddMapping(result, tmp);
                return this;
            }

            public SqlTableDependency<X_Model> Set()
            {
                SqlTableDependency<X_Model> dep = new SqlTableDependency<X_Model>(Config.ConnectionString, TABLE_NAME, mapper);
                return dep;
            }

        }
    }
}

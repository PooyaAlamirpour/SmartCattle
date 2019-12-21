using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Helper
{
    public class DbHelper
    {
        public class X_DBASE
        {
            public SqlConnection sqlConnection { get; set; }
            public SqlCommand sqlCommand { get; set; }
        }

        public X_DBASE Initial()
        {
            X_DBASE x_dbase = new X_DBASE();
            SqlConnection sc = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            sc.ConnectionString = Config.ConnectionString;
            cmd.Connection = sc;
            cmd.CommandType = CommandType.Text;
            x_dbase.sqlCommand = cmd;
            x_dbase.sqlConnection = sc;

            return x_dbase;
        }

        public static void OpenDb(X_DBASE x_dbase)
        {
            if (x_dbase.sqlConnection.State != ConnectionState.Open)
            {
                x_dbase.sqlConnection.Open();
            }
        }

        public static void CloseDb(X_DBASE x_dbase)
        {
            if (x_dbase.sqlConnection.State == ConnectionState.Open)
            {
                x_dbase.sqlConnection.Close();
            }
        }
    }
}

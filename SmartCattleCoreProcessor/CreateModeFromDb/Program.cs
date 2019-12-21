using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateModeFromDb
{
    class Program
    {
        public static int ReturnValue { get; private set; }
        public static String TableName = "NaN";

        static void Main()
        {
            Console.Write("Enter name of table: ");
            TableName = Console.ReadLine();
            string connectionString = Config.ConnectionString;
            var fileContent = File.ReadAllText("assets\\sql.txt");
            fileContent = fileContent.Replace("[DBO_NAME]", "SmartCattle").Replace("[TABLE_NAME]", TableName);
            var sqlqueries = fileContent.Split(new[] { " GO " }, StringSplitOptions.RemoveEmptyEntries);

            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand("query", con);
            con.Open();
            foreach (var query in sqlqueries)
            {
                cmd.CommandText = query;
                con.InfoMessage += Con_InfoMessage;
                cmd.ExecuteScalar();
            }
            con.Close();
        }

        private static void Con_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            var outputFromStoredProcedure = e.Message;
            File.WriteAllText(TableName + ".txt", outputFromStoredProcedure);
            Console.Write(outputFromStoredProcedure);

            String BasePRM = File.ReadAllText("assets\\PRM.txt");
            String NewPRM = BasePRM.Replace("[TABLE_NAME]", TableName);
            File.WriteAllText("PRM_" + TableName + ".txt", NewPRM);

            Console.ReadKey();
            Process.Start("CreateModeFromDb.exe");
        }
    }
}

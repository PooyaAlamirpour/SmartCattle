using getSqlRelationList.Helper;
using getSqlRelationList.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getSqlRelationList
{
    class Program
    {
        public static int ReturnValue { get; private set; }
        public static String PARENTTABLE = "NaN";
        public static String CHILDTABLE = "NaN";

        static void Main()
        {
            Console.Write("Enter name of parent table: ");
            PARENTTABLE = Console.ReadLine();

            Console.Write("Enter name of child table: ");
            CHILDTABLE = Console.ReadLine();

            string connectionString = Config.ConnectionString;
            var fileContent = File.ReadAllText("assets\\sql.txt");
            fileContent = fileContent.Replace("[PARENTTABLE]", PARENTTABLE).Replace("[CHILDTABLE]", CHILDTABLE);

            DbHelper dbase = new DbHelper();
            DbHelper.X_DBASE x_dbase = dbase.Initial();
            x_dbase.sqlCommand.CommandText = fileContent;
            List<X_Model.RelationTbl> result = X_Model.RelationTbl.Execute(x_dbase);

            Console.ReadKey();
        }

    }
}

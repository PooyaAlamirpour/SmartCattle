using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartCattleCoreProcessor.Helper.DbHelper;

namespace SmartCattleCoreProcessor.Helper
{
    class Relations
    {
        public static List<XModels.RelationTbl> get(String PARENTTABLE, String CHILDTABLE)
        {
            string connectionString = Config.ConnectionString;
            var fileContent = File.ReadAllText("assets\\sql.txt");
            fileContent = fileContent.Replace("[PARENTTABLE]", PARENTTABLE).Replace("[CHILDTABLE]", CHILDTABLE);

            DbHelper dbase = new DbHelper();
            DbHelper.X_DBASE x_dbase = dbase.Initial();
            x_dbase.sqlCommand.CommandText = fileContent;
            List<XModels.RelationTbl> result = XModels.RelationTbl.Execute(x_dbase);
            return result;
        }
    }

    class XModels
    {
        public class RelationTbl
        {
            public String Parent_Table { get; set; }
            public String Child_Table { get; set; }
            public String FKey_Name { get; set; }
            public String FKey_Col { get; set; }
            public String Ref_KeyCol { get; set; }

            public static List<RelationTbl> Execute(X_DBASE x_dbase)
            {
                List<RelationTbl> ret = new List<RelationTbl>();
                OpenDb(x_dbase);
                SqlDataReader reader = x_dbase.sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    RelationTbl obj = new RelationTbl();
                    obj.Parent_Table = (string)reader[0];
                    obj.Child_Table = (string)reader[1];
                    obj.FKey_Name = (string)reader[2];
                    obj.FKey_Col = (string)reader[3];
                    obj.Ref_KeyCol = (string)reader[4];
                    ret.Add(obj);
                }
                CloseDb(x_dbase);
                return ret;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static getSqlRelationList.Helper.DbHelper;

namespace getSqlRelationList.Model
{
    class X_Model
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

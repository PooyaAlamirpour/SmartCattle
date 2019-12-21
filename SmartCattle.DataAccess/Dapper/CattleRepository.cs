using Dapper;
using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess.Dapper
{
    public class CattleRepository : RepositoryInterface<Cattle>
    {
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartCattle"].ConnectionString);

        public Cattle Add(Cattle entity)
        {
            throw new NotImplementedException();
        }

        public Cattle Find(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cattle> GetAll(int skip , int take)
        {
            string query = @"SELECT *
                            FROM [SmartCattle].[CattleTbl] as cattle left join[SmartCattle].[ActivityStateTbl] as activity on cattle.ID = activity.cattleId
                            left join[SmartCattle].[MilkingTbl] as milking on milking.cattleId = cattle.ID";          
            return this._db.Query<Cattle ,List<CattleActivityState>, Cattle>(query,(cattle, ActivityState) =>{ cattle.ActivityState = ActivityState; return cattle; });
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Cattle Update(Cattle entity)
        {
            throw new NotImplementedException();
        }
    }
}

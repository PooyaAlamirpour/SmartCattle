using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartCattleCoreProcessor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Domain;

namespace WebSocket.Helper
{
    class FluentNHibernateHelper
    {
        public static string connectionString = "data source=79.175.133.194;User ID=hossein;Password=myzYKEGuP70V_oWb30Yr; initial catalog=SmartCattle";
        private static MsSqlConfiguration _MsSqlConfiguration = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql();

        public static ISessionFactory Notifications_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CurrentValue>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }
    }
}

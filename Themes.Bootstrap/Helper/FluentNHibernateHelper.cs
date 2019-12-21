using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Themes.Bootstrap.Domain;

namespace Themes.Bootstrap.Helper
{
    class FluentNHibernateHelper
    {
        private static string connectionString = "data source=79.175.133.194;User ID=hossein;Password=myzYKEGuP70V_oWb30Yr; initial catalog=SmartCattle";
        private static MsSqlConfiguration _MsSqlConfiguration = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql();

        public static ISessionFactory Notifications_Session()
        {
            try
            {
                ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NotificationsTable>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
                return sessionFactory;
            }
            catch (Exception ex)
            {
                String ACK = ex.Message;
                return null;
            }
            
            
        }
    }
}

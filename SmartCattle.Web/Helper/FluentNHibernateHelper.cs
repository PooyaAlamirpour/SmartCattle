using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartCattle.Web.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Helper
{
    class FluentNHibernateHelper
    {

        private static string connectionString = Convert.ToString(Models.DBHelper.ConnectionStrings());
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
                String Ack = ex.Message;
                return null;
            }
            
        }

        
    }
}
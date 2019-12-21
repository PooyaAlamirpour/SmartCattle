using Client.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    class FluentNHibernateHelper
    {
        private static string connectionString = "data source=79.175.133.194;User ID=hossein;Password=myzYKEGuP70V_oWb30Yr; initial catalog=SmartCattle";
        private static MsSqlConfiguration _MsSqlConfiguration = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql();

        public static ISessionFactory ActivityStateTbl_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ActivityStateTbl>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }

        public static ISessionFactory TempretureTbl_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TempretureTbl>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }

        public static ISessionFactory SensorTbl_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SensorTbl>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }

        public static ISessionFactory CattlePositionTbl_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CattlePositionTbl>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }

        public static ISessionFactory EnvTHITbl_Session()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(_MsSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EnvTHITbl>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory;
        }
    }
}

using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Helper
{
    class Context
    {
        private static ISessionFactory session;
        public static ISession Open()
        {
            if (session == null)
            {
                session = FluentNHibernateHelper.Notifications_Session();
            }
            else
            {
                if (session.IsClosed)
                {
                    session = FluentNHibernateHelper.Notifications_Session();
                }
            }

            ISession _session = session.OpenSession();
            return _session;
        }

        public static void Close(ISession _session)
        {
            //_session.Close();
            //session.Close();
            //_session.Dispose();
            //session.Dispose();
        }

        public static void Close(ISessionFactory LocalSession, ISession _Localsession)
        {
            //_Localsession.Close();
            //LocalSession.Close();
            //_Localsession.Dispose();
            //LocalSession.Dispose();
        }

        internal static ISessionFactory CreateSessionFactory()
        {
            return FluentNHibernateHelper.Notifications_Session();
        }

        internal static ISession OpenSession(ISessionFactory session)
        {
            return session.OpenSession();
        }
    }
}

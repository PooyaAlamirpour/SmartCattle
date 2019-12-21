using NHibernate;
using SmartCattle.Web.Domain;
using SmartCattle.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class NotificationDetailController : Controller
    {
        //
        // GET: /NotificationDetail/

        private class NotificationDetail
        {
            public IList<NotificationsTable> Notifications { get; set; }
        }

        public ActionResult Index(String ID)
        {
            int tmpID = Convert.ToInt32(ID);
            NotificationDetail ret = new NotificationDetail();
            ISessionFactory session = FluentNHibernateHelper.Notifications_Session();
            ISession _session = session.OpenSession();
            ret.Notifications = _session.QueryOver<NotificationsTable>().Where(x => x.ID == tmpID).List<NotificationsTable>();
            _session.Close();
            session.Close();
            _session.Dispose();
            session.Dispose();

            return View(ret);
        }
	}
}
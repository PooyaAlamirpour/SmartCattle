using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using SmartCattle.DomainClass;
using System.Timers;
using SmartCattle.Service;
using SmartCattle.DataAccess;
using SmartCattle.Web.Helper;
using Elmah;

namespace SmartCattle.Web.Push
{ 
    public class PushHub : PersistentConnection
    { 
        SmartCattleContext db = new SmartCattleContext();
        IPersistentConnectionContext SignalrContext = GlobalHost.ConnectionManager.GetConnectionContext<PushHub>();
        public void SendNotification()
        {
            //try
            //{
            //    string[] excepts = new string[5];
            //    var notifications = db.UserNotifications.Where(n => n.Seen != true); // get not sent notifications
            //    if (notifications.Any())
            //    {
            //        foreach (var item in notifications.GroupBy(n => n.User.Roles.FirstOrDefault()))
            //        {
            //            foreach (var sub in item.GroupBy(n => n.NotificationId))
            //            {
            //                var notice = sub.FirstOrDefault();
            //                SignalrContext.Groups.Send(item.Key.RoleId,
            //                 new
            //                 {
            //                     title = notice.Notification.Title,
            //                     content = notice.Notification.Content,
            //                     id = notice.ID,
            //                     date = DateHelper.toPersian(notice.Date),
            //                     time = notice.Date.TimeOfDay,
            //                     priority = notice.priority
            //                 }, excepts);
            //            }
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    ErrorSignal.FromCurrentContext().Raise(ex);
            //}
        }
        protected override bool AuthorizeRequest(IRequest request)
        {
            return base.AuthorizeRequest(request);
        }

        //protected override  Task OnConnected(IRequest request, string connectionId)
        //{
        //    using (SmartCattleContext context = new SmartCattleContext())
        //    {
        //        try
        //        {
        //            ClaimsIdentity identity = (ClaimsIdentity)request.User.Identity;
        //            var role = identity.Claims.Where(c => c.Type == "Role").FirstOrDefault();
        //            if (role != null)
        //            { SignalrContext.Groups.Add(connectionId, role.Value); }
        //            //var userId = identity.Claims.FirstOrDefault(c => c.Type == "userID");
        //            //var farmId = identity.Claims.FirstOrDefault(c => c.Type == "FarmID");
        //            //int priority = 0;
        //            //if (userId != null && farmId != null && role != null)
        //            //{                       
        //            //   var notifications = context.UserNotifications.Where(n => n.Seen != true && n.UserId == userId.Value).Take(20); // get not sent notification for specific user
        //            //        foreach (var notify in notifications)
        //            //        {
        //            //            //check role compability
        //            //            var notifyRole = context.RoleNotifications.Where(r => r.RoleId == role.Value && r.NotificationId == notify.NotificationId).FirstOrDefault();
        //            //            if (notifyRole != null)
        //            //            {
        //            //                priority = notifyRole.priority;
        //            //            }
        //            //        SignalrContext.Connection.Send(connectionId, new
        //            //            {
        //            //                title = notify.Notification.Title,
        //            //                content = notify.Notification.Content+ " <br /> <br />" + notify.AdditionalInfo,
        //            //                id = notify.ID,
        //            //                date = DateHelper.toPersian(notify.Date),
        //            //                time = notify.Date.TimeOfDay,
        //            //                priority = priority
        //            //            });
        //            //        } 
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorSignal.FromCurrentContext().Raise(ex);
        //        }
        //    }
        //    return  base.OnConnected(request, connectionId);
        //}

        //protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        //{
        //    try
        //    {
        //        ClaimsIdentity identity = (ClaimsIdentity)request.User.Identity;
        //        var role = identity.Claims.Where(c => c.Type == "Role").FirstOrDefault();
        //        if (role != null)
        //        { SignalrContext.Groups.Remove(connectionId, role.Value); } 
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }         
        //    return base.OnDisconnected(request, connectionId, stopCalled);
        //}

        //protected override Task OnReceived(IRequest request, string connectionId, string data)
        //{
        //    using (SmartCattleContext context = new SmartCattleContext())
        //    {
        //        try
        //        {
        //            if (request.User.Identity.IsAuthenticated)
        //            {
        //                var res = data.Split(':');
        //                int notificationId;
        //                int.TryParse(res[1], out notificationId);
        //                var notif = context.UserNotifications.Where(n => n.ID == notificationId).FirstOrDefault();
        //                if (notif != null)
        //                {
        //                    switch (res[0])
        //                    {
        //                        case "Recieved":
        //                            notif.Received = true;
        //                            break;
        //                        case "Seen":
        //                            notif.Seen = true;
        //                            notif.SeenDate = DateTime.Now;
        //                            break;
        //                    }
        //                    context.saveChanges();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorSignal.FromCurrentContext().Raise(ex);
        //        }
        //    }         
        //    return base.OnReceived(request, connectionId, data); 
        //}

        //protected override Task OnReconnected(IRequest request, string connectionId)
        //{
        //    return base.OnReconnected(request, connectionId);
        //}

        //protected override IList<string> OnRejoiningGroups(IRequest request, IList<string> groups, string connectionId)
        //{
        //    return base.OnRejoiningGroups(request, groups, connectionId);
        //}

        //protected override IList<string> GetSignals(string userId, string connectionId)
        //{
        //    return base.GetSignals(userId, connectionId);
        //}

        //public class OnlineUsers
        //{
        //    public string UserId { get; set; }
        //    public string FarmId { get; set; }
        //    public string RoleId { get; set; }
        //    public string ConnecionId { get; set; }
        //}
       
    }
}
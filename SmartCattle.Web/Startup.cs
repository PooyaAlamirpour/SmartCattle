using Microsoft.Owin;
using Owin;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using SmartCattle.DataAccess;
using SmartCattle.Service;
using SmartCattle.Core;
using SmartCattle.Web.Push;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SmartCattle.Web.Startup))]
namespace SmartCattle.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(SmartCattle.Web.Startup).Assembly);  // register controllers in Autofac
            builder.RegisterGeneric(typeof(BaseServices<>)).InstancePerLifetimeScope();  // register services in Autofac
            builder.RegisterGeneric(typeof(GenericUnitOfWork<>)).InstancePerLifetimeScope(); // register GenericUnitOfWork in Autofac
            builder.RegisterType<SmartCattleContext>().As<IDbContext>().InstancePerRequest();  // register Database Context in Autofac
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacMiddleware(container);
            #region SignalR setting
            var config = new ConnectionConfiguration()
            {
                EnableJSONP = true
            };
            
            app.MapSignalR<PushHub>("/Chat", config);
            app.Map("/Chat",
                 map =>
                 {
                     map.UseCors(CorsOptions.AllowAll);
                     map.RunSignalR<PushHub>();

                 }
             );
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            #endregion
            #region Ipars Get Data
             MyTimer.StartTimer();
            #endregion 
        }
    }
}

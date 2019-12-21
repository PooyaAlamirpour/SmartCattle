using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using SmartCattle.DataAccess;
using SmartCattle.Service;
using System.Reflection;
using System.Web.Http; 

[assembly: OwinStartup(typeof(SmartCattle.WebApi.Startup))]

namespace SmartCattle.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                   Assembly.GetExecutingAssembly())
                   .Where(t => !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t))
                    .InstancePerLifetimeScope();
           
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            var config = new HttpConfiguration();
            config.DependencyResolver = resolver;
            builder.RegisterGeneric(typeof(BaseServices<>));
            //// Get your HttpConfiguration. In OWIN, you'll create one
            //// rather than using GlobalConfiguration.
            //var config = new HttpConfiguration();

            //var builder = new ContainerBuilder();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IService<>)).InstancePerRequest();
            //builder.RegisterGeneric(typeof(GenericUnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerRequest();
            //builder.RegisterType<SmartCattleContext>().As<IDbContext>().InstancePerRequest();

            //var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //// OWIN WEB API SETUP:

            //// Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            //// and finally the standard Web API middleware.
            //app.UseAutofacMiddleware(container);
            //app.UseAutofacWebApi(config);
            //app.UseWebApi(config);

        }
    }
}

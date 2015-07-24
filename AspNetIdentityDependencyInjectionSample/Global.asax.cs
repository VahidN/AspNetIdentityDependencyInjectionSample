using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNetIdentityDependencyInjectionSample.DataLayer.Context;
using AspNetIdentityDependencyInjectionSample.IocConfig;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace AspNetIdentityDependencyInjectionSample
{
    public class MvcApplication : HttpApplication
    {
        public IContainer Container
        {
            get
            {
                return (IContainer)HttpContext.Current.Items["_Container"];
            }
            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }
        public void Application_BeginRequest()
        {
            Container = NewObjectFactory.Container.GetNestedContainer();
        }
        public void Application_EndRequest()
        {
            Container.Dispose();
            Container = null;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            setDbInitializer();
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(() => Container ?? NewObjectFactory.Container));
        }

        /*protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }*/

        public class StructureMapDependencyResolver : IDependencyResolver
        {
            private readonly Func<IContainer> _factory;

            public StructureMapDependencyResolver(Func<IContainer> factory)
            {
                _factory = factory;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == null)
                {
                    return null;
                }

                var factory = _factory();

                return serviceType.IsAbstract || serviceType.IsInterface
                    ? factory.TryGetInstance(serviceType)
                    : factory.GetInstance(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return _factory().GetAllInstances(serviceType).Cast<object>();
            }
        }

        private static void setDbInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            NewObjectFactory.Container.GetInstance<IUnitOfWork>().ForceDatabaseInitialize();
        }
    }
}
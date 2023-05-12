using KartStatsV2.BLL.Interfaces;
using KartStatsV2.BLL;
using KartStatsV2.DAL.Interfaces;
using KartStatsV2.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.Lifetime;
using Unity.AspNet.Mvc;
using System.Configuration;

namespace KartStatsV2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            container.RegisterInstance<IConfiguration>(new DAL.Interfaces.Configuration(connectionString), new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupRepository, GroupRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGroupService, GroupService>(new HierarchicalLifetimeManager());
            container.RegisterType<IInviteService, InviteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IInviteRepository, InviteRepository>(new HierarchicalLifetimeManager());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}

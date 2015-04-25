﻿using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models;
using CocktionMVC.Functions;

namespace CocktionMVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<CocktionContext>(null);
           ///Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            Database.SetInitializer<ApplicationDbContext>(null);
            Notificator.Pusher = new PushSharp.PushBroker();
           // Notificator.RegisterAppleService();
            AuctionChecker.StartChecking();
        }
    }
}

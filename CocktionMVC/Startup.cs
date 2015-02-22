using CocktionMVC.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(CocktionMVC.Startup))]
namespace CocktionMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            
            app.MapSignalR();
        }

       
    }
}

using Microsoft.Owin;
using Owin;
using CocktionMVC.Functions;
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

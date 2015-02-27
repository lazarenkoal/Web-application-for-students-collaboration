using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
namespace CocktionMVC
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional });
            
            config.SuppressDefaultHostAuthentication();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            //This will used the HTTP header: "Authorization"      Value: "Bearer 1234123412341234asdfasdfasdfasdf"
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}

using CocktionMVC.Functions;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PushSharp;
namespace CocktionMVC.Controllers.ApiControllers
{
    public class TestController : ApiController
    {
        public class Cool
        {
            public string message { get; set; }
            public int badge { get; set; }
            public string category { get; set; }
        }


        [HttpPost]
        public void Send(Cool cool)
        {
            Notificator.Pusher.QueueNotification(new AppleNotification()
                    .ForDeviceToken("95cb3fede67eaf1465eb9a2450b654fa325765f98d6f3729eba3f9d2f70c5dd9")
                    .WithAlert(cool.message)
                    .WithBadge(cool.badge).WithCategory(cool.category));
        }
    }
}

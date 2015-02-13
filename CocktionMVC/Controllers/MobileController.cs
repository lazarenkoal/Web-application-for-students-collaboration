using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CocktionMVC.Controllers
{
    public class MobileController : Controller
    {
        // Post: Mobile
        [HttpPost]
        public JsonResult Index()
        {
            MobileTest test = new MobileTest();
            test.title = "Ебаный в рот";
            test.image = "http://avatarbox.net/avatars/img19/zhou_ming_avatar_picture_49967.jpg";
            test.rating = 8.4;
            test.releaseYear = 2014;
            test.genre = new List<string>();
            test.genre.Add("sdfdsf");
            test.genre.Add("dsfdsf");
            test.genre.Add("sdfsdf");
            
            return Json(test);
        }



        public class MobileTest
        {
            public string title { get; set; }
            public string image { get; set; }
            public double rating { get; set; }
            public int releaseYear { get; set; }
            public List<string> genre { get; set; }
        }
    }
}
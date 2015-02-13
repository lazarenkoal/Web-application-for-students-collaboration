using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CocktionMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Аукционы, в которых вы еще не участвовали!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Наши контакты.";

            return View();
        }
    }
}
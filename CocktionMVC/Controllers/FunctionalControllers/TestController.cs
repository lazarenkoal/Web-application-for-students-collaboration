using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models;
using System.Net.Mail;
namespace CocktionMVC.Controllers
{
    public class TestController : Controller
    {
        // GET: TestApi
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
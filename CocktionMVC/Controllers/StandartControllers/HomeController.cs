using System.Web.Mvc;

namespace CocktionMVC.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = 100000)]
        public ActionResult Index()
        {
            return View();
        }
          
        [OutputCache(Duration = 100000)]
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
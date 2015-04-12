using System.Web.Mvc;

namespace CocktionMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("Index2");
            
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
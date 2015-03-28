using System.Web.Mvc;

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
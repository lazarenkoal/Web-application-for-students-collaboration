using System.Web.Mvc;

namespace CocktionMVC.Controllers
{
    public class HelpController : Controller
    {
        /// <summary>
        /// Метод для отображения страницы с ошибкой
        /// Если произошло что-то очень плохое.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
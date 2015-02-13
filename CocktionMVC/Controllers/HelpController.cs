using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
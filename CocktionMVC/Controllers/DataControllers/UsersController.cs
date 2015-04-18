using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;
using Microsoft.AspNet.Identity;

// ReSharper disable once CheckNamespace
namespace CocktionMVC.Controllers
{
    public class UsersController : Controller
    {
        /// <summary>
        /// Показывает страницу со всеми пользвателями
        /// на кокшне
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                CocktionContext db = new CocktionContext();
                string userId = User.Identity.GetUserId();
                List<AspNetUser> users = (from x in db.AspNetUsers
                    where x.Id != userId
                    select x).ToList();
                return View(users);
            }
            else
            {
                CocktionContext db = new CocktionContext();
                List<AspNetUser> users = db.AspNetUsers.ToList();
                return View(users);
            }
        }


        /// <summary>
        /// Показывает профиль конкретного пользователя
        /// </summary>
        /// <param name="id">Айдишник пользователя</param>
        public ActionResult GetUser(string id)
        {
            CocktionContext db = new CocktionContext();

            //находим пользователя
            var user = db.AspNetUsers.Find(id);
            //TODO собираем количество побед

            //TODO интересы

            //TODO подписки

            //TODO количество дней с нами

            return View(user);
        }
    }
}
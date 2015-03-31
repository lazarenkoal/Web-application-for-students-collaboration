using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;

// ReSharper disable once CheckNamespace
namespace CocktionMVC.Controllers
{
    /// <summary>
    /// Отвечает за взаимодействие пользователя с домами
    /// </summary>
    public class AuctionHouseController : Controller
    {
        /// <summary>
        /// Выводит пользователя на страницу со списком всех домов
        /// </summary>
        /// <returns>страницу со списком</returns>
        public ActionResult Index()
        {
            //Мб добавим потом тут какой-то код для связи с БД
            return View();
        }

        /// <summary>
        /// Показывает пользователю страничку со всеми домами конкретного
        /// университета
        /// </summary>
        /// <param name="id">Университет, который надо добавить</param>
        /// <returns>страницу со списком</returns>
        public ActionResult GetUniversityHouses(string id)
        {
            //подключаемся к базе
            CocktionContext db = new CocktionContext();

            //Выбираем все дома, относящиеся к данному ВУЗу
            List<House> houses = (from x in db.Houses
                where x.University == id
                              select x).ToList();
              
            return View(houses);
        }

        /// <summary>
        /// Показывает данынй дом
        /// </summary>
        /// <param name="id">айдишник дома</param>
        /// <returns>страницу с домом, соотв. данному айдишнику</returns>
        public ActionResult GetCurrentAuctionHouse(int id)
        {
            CocktionContext db = new CocktionContext();
            var house = db.Houses.Find(id);
            return View(house);
        }

        /// <summary>
        /// Добавляет комментарий на форум
        /// </summary>
        /// <returns>Джейсончик со статусом и именем автора</returns>
        public JsonResult AddComment()
        {
            try
            {//пробуем добавить сообщение
                //Получаем информацию с формы
                string message = Request.Form.GetValues("message")[0].Trim();
                int houseId = int.Parse(Request.Form.GetValues("houseId")[0].Trim());

                //Подключаемся к базе данных
                CocktionContext db = new CocktionContext();

                //находим дом
                var house = db.Houses.Find(houseId);
                
                //Создаем пост и добавляем в дом
                ForumPost post = new ForumPost(message, User.Identity.Name);
                post.HostHouse = house;
                house.Posts.Add(post);
                db.SaveChanges();

                //возвращаем успех
                return Json(new ForumRespond("Success", User.Identity.Name));
            }
            catch
            {//если произошла какая-то ошибка
                return Json(new ForumRespond("Failed"));
            }
        }

    }
}
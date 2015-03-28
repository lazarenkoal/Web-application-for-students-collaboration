using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;

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
            CocktionContext db = new CocktionContext();
            var houses = db.Houses.ToList();            
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
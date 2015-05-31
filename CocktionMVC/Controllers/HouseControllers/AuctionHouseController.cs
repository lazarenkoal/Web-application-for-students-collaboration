using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
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
        /// Выводит пользователя на страницу со списком всех холдеров
        /// </summary>
        /// <returns>страницу со списком</returns>
        public ActionResult Index()
        {
            //Мб добавим потом тут какой-то код для связи с БД
            CocktionContext db = new CocktionContext();
            var holders = db.HouseHolders.ToList();
            return View(holders);
        }

        /// <summary>
        /// Показывает пользователю страничку со всеми домами конкретного
        /// университета
        /// </summary>
        /// <param name="id">Университет, который надо добавить</param>
        /// <returns>страницу со списком</returns>
        public ActionResult GetUniversityHouses(int id)
        {
            //подключаемся к базе
            CocktionContext db = new CocktionContext();

            //Выбираем все дома, относящиеся к данному ВУЗу
            var holder = db.HouseHolders.Find(id);
            var houses = holder.Houses.ToList();

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
        public async Task<JsonResult> AddComment()
        {
            try
            {
                var strings = Request.Form.GetValues("message"); //получаем значения поля message в форме
                if (strings != null)
                {//если значение в порядке
                    string message = strings[0].Trim();
                    var values = Request.Form.GetValues("houseId"); //получаем значение поля houseId
                    if (values != null)
                    {//если все ок
                        int houseId = int.Parse(values[0].Trim());

                        //Подключаемся к базе данных
                        CocktionContext db = new CocktionContext();

                        //находим дом
                        var house = db.Houses.Find(houseId);

                        //Создаем пост и добавляем в дом
                        ForumPost post = new ForumPost(message, User.Identity.Name) {HostHouse = house};
                        house.Posts.Add(post);

                        //сохранение значений в базу
                        await DbItemsAdder.SaveDb(db);

                        //возвращаем успех
                        return Json(new ForumRespond("Success", User.Identity.Name));
                    }
                }
                //если в какой-то из форм не было значения - ничего не добавляем
                return Json(new ForumRespond("Failed"));
            }
            catch
            {//если произошла какая-то ошибка
                return Json(new ForumRespond("Failed"));
            }
        }

    }
}

using System;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;

namespace CocktionMVC.Controllers.AdminControllers
{
    /// <summary>
    /// Служит для редактирования и добавления новых аукционных домов в базу данных
    /// </summary>
    public class EditHousesController : Controller
    {
        /// <summary>
        /// Возвращает страничку со списком аукционных домов
        /// либо открывает доступ к редактированию их для 
        /// пользователей - администраторов
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            //Возвращаем разные вьюшки с правами, в зав-ти от пользователя
            switch (User.Identity.Name)
            {
                case "darya-coo@cocktion.com":
                    CocktionContext db = new CocktionContext();
                    var houses = db.Houses.ToList();
                    return View(houses);
                default :
                    return View("PageNotFound");
            }
        }

        /// <summary>
        /// Добавляет дом в базу данных
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult AddHouse()
        {
            //Чтение из запроса с клиента данных о доме.
            string university;
            string faculty;
            string adress;
            RequestFormReader.ReadAddHouseForm(Request, out university, out faculty, out adress);

            //Добавляем дом в боазу данных
            CocktionContext db = new CocktionContext();
            db.Houses.Add(new House(adress, university, faculty));
            db.SaveChanges();

            //Создаем ответ для сервера
            HouseRespond resp = new HouseRespond();
            resp.House = String.Format("ВУЗ: {0}, Факультет: {1}, Адрес: {2}", university,
                faculty, adress);

            return Json(resp);
        }
    }
}
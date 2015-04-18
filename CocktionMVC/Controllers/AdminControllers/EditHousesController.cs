using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models;
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
            if (User.Identity.Name == "darya-coo@cocktion.com" || User.Identity.Name == "lazarenko.ale@gmail.com")
            {
                CocktionContext db = new CocktionContext();
                var houses = db.Houses.ToList();
                var holders = db.HouseHolders.ToList();
                Tuple<List<House>, List<HouseHolder>> tuple
                    = new Tuple<List<House>, List<HouseHolder>>(houses, holders);
                return View(tuple);
            }
            return View("PageNotFound");
        }

        /// <summary>
        /// Добавляет дом в базу данных
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult AddHouse()
        {
            //Чтение из запроса с клиента данных о доме.
            string holderId;
            string faculty;
            string adress;
            RequestFormReader.ReadAddHouseForm(Request, out faculty, out adress, out holderId);

            CocktionContext db = new CocktionContext();
            var holder = db.HouseHolders.Find(int.Parse(holderId));

            //обработать фотку
            Picture photo = new Picture();
            PhotoProcessor.CreateAndSavePicture(photo, Request, 200, 200);
                
            //создать дом
            House house = new House(adress, faculty, holder, photo);

            //добавить дом в холдер
            holder.Houses.Add(house);

            //добавить дом в базу
            db.Houses.Add(house);
            db.Pictures.Add(photo);
            db.SaveChanges();
            //вернуть статус

            return Json(new StatusHolder(true));
        }

        public class Q
        {

            public string Status { get; set; }

        }

        [HttpPost]
        [Authorize]
        public JsonResult AddHolder()
        {
            try
            {
                string holderName = Request.Form.GetValues("holderName")[0].Trim();

                Picture photo = new Picture();
                
                PhotoProcessor.CreateAndSavePicture(photo, Request, 200, 200);

                CocktionContext db = new CocktionContext();
                db.Pictures.Add(photo);
                db.HouseHolders.Add(new HouseHolder(holderName, photo));

                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            catch (Exception q)
            {
                return Json(new Q {Status = q.InnerException.InnerException.Message});
            }


        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using CocktionMVC.Functions;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using Microsoft.Owin.Security.Twitter.Messages;

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
            string description;
            RequestFormReader.ReadAddHouseForm(Request, out faculty, out adress, out holderId);

            CocktionContext db = new CocktionContext();
            var holder = db.HouseHolders.Find(int.Parse(holderId));

            //обработать фотку
            Picture photo = new Picture();
            PhotoProcessor.CreateAndSavePicture(photo, Request, 200);
                
            //создать дом
            House house = new House(adress, faculty, holder, photo);
            description = Request.Form.GetValues("description")[0].Trim();
            house.Description = description;

            //добавить дом в холдер
            holder.Houses.Add(house);

            //добавить дом в базу
            db.Houses.Add(house);
            db.Pictures.Add(photo);
            db.SaveChanges();
            //вернуть статус

            return Json(new StatusHolder(true));
        }

        [HttpPost]
        public JsonResult DeleteHouse()
        {
            string houseId = Request.Form.GetValues("houseId")[0].Trim();
            int id = int.Parse(houseId);
            CocktionContext db = new CocktionContext();
            db.Houses.Remove(db.Houses.Find(id));
            db.SaveChanges();
            return Json(new StatusHolder(true));
        }

        [HttpPost]
        public JsonResult DeleteHolder()
        {
            string holderId = Request.Form.GetValues("holderId")[0].Trim();
            int id = int.Parse(holderId);
            CocktionContext db = new CocktionContext();
            db.HouseHolders.Remove(db.HouseHolders.Find(id));
            db.SaveChanges();
            return Json(new StatusHolder(true));
        }

        public class Q
        {

            public string Status { get; set; }

        }

        [HttpPost]
        [Authorize]
        public JsonResult EditHolder()
        {
            Picture selfie = new Picture();
            if (Request.Files.Count != 0)
            {
                PhotoProcessor.CreateAndSavePicture(selfie, Request, 200);
            }

            CocktionContext db = new CocktionContext();
            string holderId = Request.Form.GetValues("holderId")[0].Trim();
            string holderName = Request.Form.GetValues("holderName")[0].Trim();
            string holderCity = Request.Form.GetValues("holderCity")[0].Trim();

            var holder = db.HouseHolders.Find(int.Parse(holderId));
            if (!selfie.FileName.IsEmpty())
            {
                holder.PhotoCard = selfie;
            }

            if (!holderName.IsEmpty())
            {
                holder.Name = holderName;
            }

            if (!holderCity.IsEmpty())
            {
                holder.City = holderCity;
            }

            db.SaveChanges();
            return Json(new StatusHolder(true));
        }

        [HttpPost]
        [Authorize]
        public JsonResult EditHouse()
        {
            Picture selfie = new Picture();
            if (Request.Files.Count != 0)
            {
                PhotoProcessor.CreateAndSavePicture(selfie, Request, 200);
            }
            
            CocktionContext db = new CocktionContext();
            string houseId = Request.Form.GetValues("houseId")[0].Trim();
            string houseName = Request.Form.GetValues("houseName")[0].Trim();
            string houseDescription = Request.Form.GetValues("houseDescription")[0].Trim();
            string houseAdress = Request.Form.GetValues("houseAdress")[0].Trim();

            var house = db.Houses.Find(int.Parse(houseId));
            if (!selfie.FileName.IsEmpty())
            {
                house.Portrait = selfie;
            }

            if (!houseName.IsEmpty())
            {
                house.Faculty = houseName;
            }

            if (!houseDescription.IsEmpty())
            {
                house.Description = houseDescription;
            }

            if (!houseAdress.IsEmpty())
            {
                house.Adress = houseAdress;
            }

            db.SaveChanges();
            return Json(new StatusHolder(true));
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddHolder()
        {
            try
            {
                string holderName = Request.Form.GetValues("holderName")[0].Trim();
                string holderCity = Request.Form.GetValues("holderCity")[0].Trim();
                Picture photo = new Picture();
                
                PhotoProcessor.CreateAndSavePicture(photo, Request, 200);

                CocktionContext db = new CocktionContext();
                db.Pictures.Add(photo);
                db.HouseHolders.Add(new HouseHolder(holderName,holderCity, photo));

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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CocktionMVC.Models.Hubs;
using CocktionMVC.Functions;
using System.Threading.Tasks;
namespace CocktionMVC.Controllers
{
    public class BidAuctionCreatorController : Controller
    {
        /// <summary>
        /// Создает аукцион и записывает его в базу данных
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult CreateAuction()
        {
            //создаем продукт, получаем с клиента информацию о нем
            Product product = new Product();
            product.Name = Request.Form.GetValues("Name")[0].Trim();
            product.Description = Request.Form.GetValues("Description")[0].Trim();
            product.Category = Request.Form.GetValues("Category")[0].Trim();

            //Cтроку получаем в формате !1!1!1!
            //в строке содержатся айдишники локаций
            string locationsIdString = Request.Form.GetValues("LocationsId")[0].Trim('!');
            string[] locIds = locationsIdString.Split('!');
            //переводим массив в массив инта
            int[] locIdsInt = Array.ConvertAll(locIds, x => int.Parse(x));
            //получаем данные с клиента о времени для окончания аукцйиона,
            //обрабатываем их
            //ТУДУ: добавить проверку значений времени
            string minutesString = Request.Form.GetValues("Minutes")[0].Trim();
            string hoursString = Request.Form.GetValues("Hours")[0].Trim();
            int minutes = int.Parse(minutesString);
            int hours = int.Parse(hoursString);

            //получаем файл
            HttpPostedFileBase file = Request.Files[0];

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();

            //объявление свойств продукта
            product.OwnerId = userId;
            product.IsOnAuctionAsALot = true;
            product.OwnerName = userName;

            //добавляю новую вещь в базу данных
            db.Products.Add(product);
            db.SaveChanges();

            //инициализация аукциона
            Auction auction = new Auction();
            auction.IsActive = true;
            auction.OwnerId = userId;
            auction.OwnerName = userName;
            auction.SellProduct = product;
            auction.WinnerChosen = false;
            auction.AuctionToteBoard = new ToteBoard();
            //добавляем все локации списочком к аукциону
            Array.ForEach(locIdsInt, x => auction.GeoLocations.Add(db.Locations.Find(x)));
            //задаем время окончания и начала аукциона
            DateTime auctionsEndTime = DateTime.Now;
            DateTime auctionStartTime;
            TimeZoneInfo tzi; //указываем временную зону
            tzi = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            auctionsEndTime = TimeZoneInfo.ConvertTime(auctionsEndTime, tzi);
            auctionStartTime = auctionsEndTime;
            auctionsEndTime = auctionsEndTime.AddHours(hours);
            auctionsEndTime = auctionsEndTime.AddMinutes(minutes);
            auction.EndTime = auctionsEndTime;
            auction.StartTime = auctionStartTime;

            //добавление фотографии для товара
            Photo photo = new Photo();
            if (file != null)
            {
                //Получаем информацию о том, откуда ж взялся файлик
                string pic = System.IO.Path.GetFileName(file.FileName); //имя файла
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images/Photos/"), pic); //директория, в которую его загрузят
                file.SaveAs(path);

                //получаем thumbnail
                string thumbNailPath = Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailGenerator.ResizeImage(file, thumbNailPath, 90, 90);
                ThumbnailSet thumbNail = new ThumbnailSet();
                thumbNail.FileName = pic;
                thumbNail.FilePath = thumbNailPath + pic;

                //забиваем данные о фотке
                photo.FileName = pic;
                photo.FilePath = path;
                photo.Product = product;
                photo.ThumbnailSets.Add(thumbNail);
            }
            
            //сохраняем все изменения в базу
            db.Photos.Add(photo);
            db.Auctions.Add(auction);
            db.SaveChanges();

            //апдейтим список
            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            //возвращаем статус
            return Json("Успешно добавлено");

        }

        /// <summary>
        /// Метод для загрузки файла
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            //получение информации с формы
            //обработка файла
            HttpPostedFileBase file = Request.Files[0]; //загрузка файла из запроса
            string fileName = System.IO.Path.GetFileName(file.FileName); //получаем имя файла
            string path = Server.MapPath("~/Images/Photos/") + fileName; //путь на сервере
            file.SaveAs(path); //сохранение

            //получаем thumbnail
            string thumbNailPath = Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
            ThumbnailGenerator.ResizeImage(file, thumbNailPath, 60, 60);
            ThumbnailSet thumbNail = new ThumbnailSet();
            thumbNail.FileName = fileName;
            thumbNail.FilePath = thumbNailPath + fileName;

            //обработка данных из формы
            string bidName = Request.Form.GetValues("name")[0].Trim(); //получение имени
            string bidDescription = Request.Form.GetValues("description")[0].Trim();//getting description
            string bidCategory = Request.Form.GetValues("category")[0].Trim();//getting category

            //handling data about auction
            string auctionId = Request.Form.GetValues("auctionId")[0].Trim();//getting Auction's Id

            //Adding new product to the DB
            CocktionContext db = new CocktionContext();

            //Creating Photo for product and adding to the DB
            Photo photo = new Photo();
            photo.FileName = fileName;
            photo.FilePath = path;
            photo.ThumbnailSets.Add(thumbNail);

            //Creating product for the db
            Product product = new Product();
            product.Name = bidName;
            product.Description = bidDescription;
            product.Category = bidCategory;
            int id = int.Parse(auctionId);
            product.OwnerId = User.Identity.GetUserId();
            product.Photos.Add(photo);
            product.OwnerName = User.Identity.Name;
            photo.Product = product;
            db.Auctions.Find(id).BidProducts.Add(product);
            await DbItemsAdder.AddProduct(db, product, photo);
            int result = product.Id;
            return Json(result);
        }

        /// <summary>
        /// Служит для загрузки фотографии на сервер с 
        /// мобильного устройства
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadPhotoFromMobile(int id)
        {
            //Надо будет апдейтнуть список всех аукционов
            HttpPostedFileBase file = Request.Files[0];
            Photo photo = new Photo();
            CocktionContext db = new CocktionContext();
            Product product = db.Products.Find(id);
            if (file != null)
            {
                //Получаем информацию о том, откуда ж взялся файлик
                string pic = System.IO.Path.GetFileName(file.FileName); //имя файла
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images/Photos/"), pic); //директория, в которую его загрузят
                file.SaveAs(path);

                //получаем thumbnail
                string thumbNailPath = Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailGenerator.ResizeImage(file, thumbNailPath, 90, 90);
                ThumbnailSet thumbNail = new ThumbnailSet();
                thumbNail.FileName = pic;
                thumbNail.FilePath = thumbNailPath + pic;

                //забиваем данные о фотке
                photo.FileName = pic;
                photo.FilePath = path;
                photo.Product = product;
                photo.ThumbnailSets.Add(thumbNail);
                db.Photos.Add(photo);
                db.SaveChanges();
                return Json("Успешно загружено!");
            }
            else
            {
                return Json("Ну так себе все!");
            }
        }

        [HttpGet]
        public ActionResult UploadPhotoFromMobile(string id)
        {
            return View(model: id);
        }


        /// <summary>
        /// Метод принимает параметры для продукта
        /// которые принимает от клиента и отсылает
        /// посылать фоточки из мобилки
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveProductInfo()
        {
            //создаем продукт, получаем с клиента информацию о нем
            Product product = new Product();
            product.Name = Request.Form.GetValues("Name")[0].Trim();
            product.Description = Request.Form.GetValues("Description")[0].Trim();
            product.Category = Request.Form.GetValues("Category")[0].Trim();

            //получаем данные с клиента о времени для окончания аукцйиона,
            //обрабатываем их
            //ТУДУ: добавить проверку значений времени
            string minutesString = Request.Form.GetValues("Minutes")[0].Trim();
            string hoursString = Request.Form.GetValues("Hours")[0].Trim();
            int minutes = int.Parse(minutesString);
            int hours = int.Parse(hoursString);
            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;
            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();

            //объявление свойств продукта
            product.OwnerId = userId;
            product.IsOnAuctionAsALot = true;
            product.OwnerName = userName;

            //добавляю новую вещь в базу данных
            db.Products.Add(product);
            db.SaveChanges();

            //инициализация аукциона
            Auction auction = new Auction();
            auction.IsActive = true;
            auction.OwnerId = userId;
            auction.OwnerName = userName;
            auction.SellProduct = product;
            auction.WinnerChosen = false;
            auction.AuctionToteBoard = new ToteBoard();

            //задаем время окончания и начала аукциона
            DateTime auctionsEndTime = DateTime.Now;
            DateTime auctionStartTime;
            TimeZoneInfo tzi; //указываем временную зону
            tzi = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            auctionsEndTime = TimeZoneInfo.ConvertTime(auctionsEndTime, tzi);
            auctionStartTime = auctionsEndTime;
            auctionsEndTime = auctionsEndTime.AddHours(hours);
            auctionsEndTime = auctionsEndTime.AddMinutes(minutes);
            
            auction.EndTime = auctionsEndTime;
            auction.StartTime = auctionStartTime;

            db.Auctions.Add(auction);
            db.SaveChanges();
            int productId = product.Id;
            Link uploadFromMobileLink = new Link(productId);
            return Json(uploadFromMobileLink);
        }

        public struct Link
        {
            public string Path;

            public Link(int id)
            {
                this.Path = "/BidAuctionCreator/UploadPhotoFromMobile/" + id; 
            }
        }
    }
}
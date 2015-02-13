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
    public class FileSaverController : Controller
    {
        [Authorize]
        [HttpPost]
        public JsonResult CreateAuction()
        {

            Product product = new Product();
            product.Name = Request.Form.GetValues("Name")[0];
            product.Description = Request.Form.GetValues("Description")[0];
            product.Category = Request.Form.GetValues("Category")[0];

            string minutesString = Request.Form.GetValues("Minutes")[0];
            string hoursString = Request.Form.GetValues("Hours")[0];

            int minutes = int.Parse(minutesString);
            int hours = int.Parse(hoursString);

            HttpPostedFileBase file = Request.Files[0];

            //НАПИСАТЬ НОРМАЛЬНЫЙ МЕТОД ПЕРЕХОДА ПО ПОЯСАМ

            //переменные для хранения сведений о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

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

            DateTime auctionsEndTime = DateTime.Now;

            TimeZoneInfo tzi;
            tzi = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

            auctionsEndTime = TimeZoneInfo.ConvertTime(auctionsEndTime, tzi);
            auctionsEndTime = auctionsEndTime.AddHours(hours);
            auctionsEndTime = auctionsEndTime.AddMinutes(minutes);
            auction.EndTime = auctionsEndTime;

            //добавляю новый аукцион в базу


            //добавление фотографии для товара
            Photo photo = new Photo();
            if (file != null)
            {
                //Получаем информацию о том, откуда ж взялся файлик
                string pic = System.IO.Path.GetFileName(file.FileName); //имя файла
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images/Photos/"), pic); //директория, в которую его загрузят
                // file is uploaded
                file.SaveAs(path);


                //получаем thumbnail
                string thumbNailPath = Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailGenerator.ResizeImage(file, thumbNailPath, 90, 90);
                ThumbnailSet thumbNail = new ThumbnailSet();
                thumbNail.FileName = pic;
                thumbNail.FilePath = thumbNailPath + pic;

                photo.FileName = pic;
                photo.FilePath = path;
                photo.Product = product;
                photo.ThumbnailSets.Add(thumbNail);
            }
            db.Photos.Add(photo);
            db.Auctions.Add(auction);
            db.SaveChanges();

            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            return Json("Успешно добавлено");

        }

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
            string bidName = Request.Form.GetValues("name")[0]; //получение имени
            string bidDescription = Request.Form.GetValues("description")[0];//getting description
            string bidCategory = Request.Form.GetValues("category")[0];//getting category

            //handling data about auction
            string auctionId = Request.Form.GetValues("auctionId")[0];//getting Auction's Id

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
            //product.Id = db.Products.Count() + 1;
            photo.Product = product;
            db.Auctions.Find(id).BidProducts.Add(product);
            await DbItemsAdder.AddProduct(db, product, photo);
            int result = product.Id;
            return Json(result);
        }
    }
}
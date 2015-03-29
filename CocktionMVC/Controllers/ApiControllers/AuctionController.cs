using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.Hubs;
using CocktionMVC.Models.JsonModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class AuctionController : ApiController
    {
        /// <summary>
        /// Метод, достающий из базы данных
        /// все активные аукционы и обрабатывающий их
        /// для нужд мобильного приложения
        /// </summary>
        /// <returns>Джейсоном лист из аукционов</returns>
        [Authorize]
        [HttpPost]
        public List<AuctionInfo> GetActiveAuctions()
        {
            CocktionContext db = new CocktionContext();

            //Конвертация времени из ютс в местное
            DateTime controlTime = DateTimeManager.GetCurrentTime();

            //Выборка активных в данный момент аукционов
            var auctions = (from x in db.Auctions
                            where ((x.EndTime > controlTime) && (x.IsActive))
                            select x).ToList<Auction>();

            List<AuctionInfo> auctiInfo = new List<AuctionInfo>();
            
            //формируем список в том формате, который нужен для отображения в приложении
            foreach (var auction in auctions)
            {
                //для мобилки нужно количество минут до конца
                int minutes = (int)auction.EndTime.Subtract(controlTime).TotalMinutes;
                auctiInfo.Add(
                       new AuctionInfo
                       {
                           AuctionCategory = auction.SellProduct.Category.Trim(),
                           AuctionDescription = auction.SellProduct.Description.Trim(),
                           AuctionEndTime = minutes,
                           AuctionImage = @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photos.First().FileName,
                           AuctionTitle = auction.SellProduct.Name.Trim(),
                           AuctionId = auction.Id
                       }
                       );
            }//end of foreach
            return auctiInfo;
        }//end of GetActiveAuctions()

        //В АПИ КОНТРОЛЛЕРАХ РУТИНГ ДРУГОЙ!!!

        /// <summary>
        /// Метод отправляет на мобильный клиент информацию о всех товарах, 
        /// которые поставлены на данном аукционе
        /// </summary>
        /// <param name="id">Айди аукциона</param>
        /// <returns>Лист с информацией о продуктах</returns>
        [Authorize]
        [HttpPost]
        public List<ProductInfoMobile> GetAuctionBids(int id)
        {
            CocktionContext db = new CocktionContext();

            //находим аукцион по айди
            Auction auction = db.Auctions.Find(id);
            List<ProductInfoMobile> bidProducts = new List<ProductInfoMobile>();

            //добавляем все в коллекцию
            foreach (var bid in auction.BidProducts)
                bidProducts.Add(new ProductInfoMobile
                    {
                        ProductDescription = bid.Description.Trim(),
                        ProductFileName = @"http://cocktion.com/Images/Thumbnails/" + bid.Photos.First().FileName,
                        ProductName = bid.Name.Trim(),
                        ProductCategory = bid.Category.Trim(),
                        ProductId = bid.Id
                    });
            return bidProducts;
        }

        /// <summary>
        /// Создание аукциона с мобильного клиента
        /// </summary>
        [Authorize]
        [HttpPost]
        public AuctionApiRespondModels.AuctionCreateStatus CreateAuction()
        {
            //создаем продукт и сохраняем информацию о нем.
            Product product = new Product();

            product.Name = HttpContext.Current.Request.Form.GetValues("name")[0].Trim();
            product.Description = HttpContext.Current.Request.Form.GetValues("description")[0].Trim();
            product.Category = HttpContext.Current.Request.Form.GetValues("category")[0].Trim();

            //получаем информацию о времени
            string minutesString = HttpContext.Current.Request.Form.GetValues("minutes")[0].Trim();
            string hoursString = HttpContext.Current.Request.Form.GetValues("hours")[0].Trim();
            int minutes = int.Parse(minutesString);
            int hours = int.Parse(hoursString);
            
            //Заводим возвращалку статуса
            AuctionApiRespondModels.AuctionCreateStatus info =
                new AuctionApiRespondModels.AuctionCreateStatus();

            //получаем файлик
            string filePath = "";
            string fileName = "";
            HttpPostedFile postedFile = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    postedFile = httpRequest.Files[file];
                    string extension = Path.GetExtension(postedFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = HttpContext.Current.Server.MapPath("~/Images/Photos/" + fileName);
                    postedFile.SaveAs(filePath);

                }
                info.Status = "success";
            }
            else
            {
                info.Status = "failed";
            }

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();

            //объявление свойств продукта
            product.OwnerId = userId;
            product.IsOnAuctionAsALot = true;
            product.OwnerName = userName;

            //инициализация аукциона
            Auction auction = new Auction();
            auction.IsActive = true;
            auction.OwnerId = userId;
            auction.OwnerName = userName;
            auction.SellProduct = product;
            auction.WinnerChosen = false;
            auction.AuctionToteBoard = new ToteBoard();

            //Совершаем манипуляции со временем
            DateTime auctionsEndTime = DateTimeManager.GetCurrentTime();
            DateTime auctionStartTime = auctionsEndTime;
            auctionsEndTime = auctionsEndTime.AddHours(hours);
            auctionsEndTime = auctionsEndTime.AddMinutes(minutes);
            auction.EndTime = auctionsEndTime;
            auction.StartTime = auctionStartTime;

            //добавляем фоточку и фабмнейл
            Photo photo = new Photo();
            string thumbNailPath = HttpContext.Current.Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
            ThumbnailGenerator.ResizeImage(postedFile, thumbNailPath, 90, 90);
            ThumbnailSet thumbNail = new ThumbnailSet();
            thumbNail.FileName = fileName;
            thumbNail.FilePath = thumbNailPath + fileName;

            //забиваем данные о фотке
            photo.FileName = fileName;
            photo.FilePath = filePath;
            photo.Product = product;
            photo.ThumbnailSets.Add(thumbNail);

            db.Photos.Add(photo);
            db.Products.Add(product);
            db.Auctions.Add(auction);
            db.SaveChanges();

            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            info.PhotoPath = "http://cocktion.com/Images/Thumbnails/" + fileName;

            return info;
        }
        
    }
}

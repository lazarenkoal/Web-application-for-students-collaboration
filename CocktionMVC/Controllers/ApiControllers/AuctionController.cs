using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web;
using CocktionMVC.Functions;
using CocktionMVC.Models.Hubs;
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
            //выбираем все аукционы, которые активны в данный момент
            DateTime controlTime = DateTime.Now;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            controlTime = TimeZoneInfo.ConvertTime(controlTime, TimeZoneInfo.Local, tst);
            var auctions = (from x in db.Auctions
                            where ((x.EndTime > controlTime) && (x.IsActive == true))
                            select x).ToList<Auction>();
            List<AuctionInfo> auctiInfo = new List<AuctionInfo>();
            
            //формируем список в том формате, который нужен для отображения в приложении
            foreach (var auction in auctions)
            {

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

        /// <summary>
        /// Метод позволяет достать все аукционы, где хозяин - владелец
        /// </summary>
        /// <returns>Лист с информацией об аукионах</returns>
        [Authorize]
        [HttpPost]
        public List<AuctionInfo> GetMyAuctions()
        {
            CocktionContext db = new CocktionContext();
            var userId = User.Identity.GetUserId();
            var auctions = (from x in db.Auctions
                            where x.OwnerId == userId
                            select x).ToList<Auction>();
            List<AuctionInfo> auctionInfo = new List<AuctionInfo>();
            DateTime controlTime = DateTime.Now;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            controlTime = TimeZoneInfo.ConvertTime(controlTime, TimeZoneInfo.Local, tst);
            foreach (var auction in auctions)
            {

                int minutes = (int)auction.EndTime.Subtract(controlTime).TotalMinutes;
                auctionInfo.Add(
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

            return auctionInfo;
        }


        //В АПИ КОНТРОЛЛЕРАХ РУТИНГ ДРУГОЙ!!!

        /// <summary>
        /// Метод отправляет на мобильный клиент информацию о всех товарах, 
        /// которые поставлены на данном аукционе
        /// </summary>
        /// <param name="auctionId">Айди аукциона</param>
        /// <returns>Лист с информацией о продуктах</returns>

        [Authorize]
        [HttpPost]
        public List<ProductInfoMobile> GetAuctionBids(int id)
        {
            CocktionContext db = new CocktionContext();
            Auction auction = db.Auctions.Find(id);
            List<ProductInfoMobile> bidProducts = new List<ProductInfoMobile>();
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

        [Authorize]
        [HttpPost]
        public CocktionMVC.Models.JsonModels.AuctionApiRespondModels.AuctionCreateStatus CreateAuction()
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
                    filePath = HttpContext.Current.Server.MapPath("~/Images/Photos/" + postedFile.FileName);
                    fileName = postedFile.FileName;
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

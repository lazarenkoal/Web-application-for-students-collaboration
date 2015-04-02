using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<AuctionApiRespondModels.AuctionCreateStatus> CreateAuction()
        {
            //Получаем данные из запроса
            string minutesString, hoursString, name, description, category;
            RequestFormReader.ReadCreateAuctionFormMobile(HttpContext.Current.Request, out name, out description,
                out category, out minutesString, out hoursString);

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
                    fileName = Guid.NewGuid().ToString() + extension; //генерируем новое имя для фотки
                    filePath = HttpContext.Current.Server.MapPath("~/Images/Photos/" + fileName);
                    postedFile.SaveAs(filePath);

                }
                info.Status = "Success";
            }
            else
            {
                info.Status = "Failure";
            }

            //добавляем фоточку и фабмнейл
            string thumbNailPath = HttpContext.Current.Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
            ThumbnailSet thumbNail = new ThumbnailSet();
            thumbNail.FileName = fileName;
            thumbNail.FilePath = thumbNailPath + fileName;
            ThumbnailGenerator.ResizeImage(postedFile, thumbNail.FilePath, 90, 90);

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();

            //создаем продукт и сохраняем информацию о нем.
            Product product = new Product(name, description, category, userId, true, userName);

            //инициализация аукциона
            Auction auction = new Auction(true, userId, userName, product, false, new ToteBoard());

            //Совершаем манипуляции со временем
            DateTimeManager.SetAuctionStartAndEndTime(auction, hoursString, minutesString);

            //забиваем данные о фотке
            Photo photo = new Photo();
            photo.FileName = fileName;
            photo.FilePath = filePath;
            photo.Product = product;
            photo.ThumbnailSets.Add(thumbNail);

            //все в базу добавляем
            await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo);

            //обновляем список аукционов
            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            //шлем ссылочку на фотку
            info.PhotoPath = "http://cocktion.com/Images/Thumbnails/" + fileName;

            return info;
        }
        
    }
}

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
using CocktionMVC.Models.JsonModels.MobileClientModels;
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
                           description = auction.SellProduct.Description.Trim(),
                           endTime = minutes,
                           photoPath = @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photo.FileName,
                           title = auction.SellProduct.Name.Trim(),
                           auctionId = auction.Id,
                           leaderId = auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                           сategory = auction.SellProduct.Category.Trim()
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
        public List<ProductInfo> GetAuctionBids(IdCont aId)
        {
            CocktionContext db = new CocktionContext();

            //находим аукцион по айди
            Auction auction = db.Auctions.Find(aId.id);
            List<ProductInfo> bidProducts = new List<ProductInfo>();

            //добавляем все в коллекцию
            foreach (var bid in auction.BidProducts)
                bidProducts.Add(new ProductInfo
                    {
                        description = bid.Description.Trim(),
                        photoPath = @"http://cocktion.com/Images/Thumbnails/" + bid.Photo.FileName,
                        title = bid.Name.Trim(),
                        category = bid.Category.Trim(),
                        id = bid.Id,
                    });
            return bidProducts;
        }

        public class CreateAuctionInfo
        {
            public string title { get; set; }

            public string description { get; set; }
            public string category { get; set; }
            public int photoId { get; set; }
            public string timeBound { get; set; }
        }

        /// <summary>
        /// Создание аукциона с мобильного клиента
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<StatusAndPhotoPath> CreateAuction(CreateAuctionInfo auctionInfo)
        {
            //Заводим возвращалку статуса
            StatusAndPhotoPath info =
                new StatusAndPhotoPath();
            try
            {
                //получаем информацию о пользователе
                string userId = User.Identity.GetUserId();

                //подключаемся к базе данных
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(userId);

                //создаем продукт и сохраняем информацию о нем.
                Product product = new Product(auctionInfo.title, auctionInfo.description,
                    auctionInfo.category, true, user);

                //инициализация аукциона
                Auction auction = new Auction(true, product, false, new ToteBoard(), user);

                auction.Rating = (int)(user.Rating * 0.4);
                auction.Owner = user;
                //Совершаем манипуляции со временем
                DateTimeManager.SetAuctionStartAndEndTime(auction, auctionInfo.timeBound);

                //увеличиваем рейтинг пользователя
                RatingManager.IncreaseRating(user, "userMadeAuction");
                Picture photo;
                //забиваем данные о фотке
                if (auctionInfo.photoId != -1)
                {
                    photo = db.Pictures.Find(auctionInfo.photoId);
                }
                else
                {
                    photo = new Picture();
                    const string placeHolderName = "placeholder.jpg";
                    photo.FileName = placeHolderName;
                    string path = Path.Combine(
                        HttpContext.Current.Request.MapPath("~/Content/SiteImages"), placeHolderName);
                    photo.FilePath = path;
                }

                product.Photo = photo;
                //все в базу добавляем
                await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo, user);

                //обновляем список аукционов
                AuctionListHub.UpdateList(product.Name, product.Description, product.Category, auction.EndTime,
                photo.FileName, auction.Id);


                //шлем ссылочку на фотку
                info.PhotoPath = "http://cocktion.com/Images/Thumbnails/" + photo.FileName;
                info.Status = "Success";

            }
            catch
            {
                info.PhotoPath = null;
                info.Status = "Failure";
            }
            return info;
        }

        public class ChooseLeaderClass
        {
            public int auctionId { get; set; }
            public int productId { get; set; }
        }

        /// <summary>
        /// Позволяет выбирать лидера с мобильного клиента 
        /// </summary>
        /// <returns>Стандартный статус (удалось или нет)</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder ChooseLeader(ChooseLeaderClass leader)
        {
            CocktionContext db = new CocktionContext();
            string userId = User.Identity.GetUserId();
            try
            {
                var auction = db.Auctions.Find(leader.auctionId);
                var product = db.Products.Find(leader.productId);

                if (userId != auction.Owner.Id)
                    throw new Exception();
                auction.LeadProduct = product;

                auction.WinnerChosen = true;

                db.SaveChanges();
                AuctionHub.SetLider(leader.productId.ToString(), leader.auctionId, product.Name);
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
        }

        public class IdCont
        {
            public int id { get; set; }
        }

        /// <summary>
        /// Завершает аукцион с мобильника
        /// </summary>
        /// <returns>Стандартный статус</returns>
        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> EndAuction(IdCont aId)
        {
            //TODO решить проблему взаимодействия с клиентами. Они не могут узнать, что аукцион завершен с мобильника
            CocktionContext db = new CocktionContext();
            string userId = User.Identity.GetUserId();
            try
            {
                //находим аукцион
                var auction = db.Auctions.Find(aId.id);

                //Проверяем, можно ли его закончить

                //является ли пользователь владельцем?
                bool isUserOwner = (userId == auction.Owner.Id);
                bool canEnd = auction.IsActive & auction.WinnerChosen;
                if ((!isUserOwner) & canEnd)
                    throw new Exception();

                await AuctionHub.FinishAuctionMobile(auction.Id);

                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }

        }

        public class prI
        {
            public string title { get; set; }

            public string description { get; set; }

            public string category { get; set; }

            public int auctionId { get; set; }

            public int photoId { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> AddBid(prI info)
        {
            try
            {
                //получаем информацию о пользователе
                string userId = User.Identity.GetUserId();

                //подключаемся к базе данных
                CocktionContext db = new CocktionContext();

                var user = db.AspNetUsers.Find(userId);

                //создаем продукт и сохраняем информацию о нем.
                Product product = new Product(info.title, info.description, info.category, false, user);

                Picture photo;
                //забиваем данные о фотке
                if (info.photoId != -1)
                {
                    photo = db.Pictures.Find(info.photoId);
                }
                else
                {
                    photo = new Picture();
                    const string placeHolderName = "placeholder.jpg";
                    photo.FileName = placeHolderName;
                    string path = Path.Combine(
                        HttpContext.Current.Request.MapPath("~/Content/SiteImages"), placeHolderName);
                    photo.FilePath = path;
                }
                //добавляем продукт
                //Коннектимся к базе
                var auction = db.Auctions.Find(info.auctionId);
                auction.BidProducts.Add(product);
                product.SelfAuction = auction;

                product.Photo = photo;

                //находи кластер
                BidCluster bidCluster = new BidCluster();
                bidCluster.UserId = userId;
                bidCluster.HostAuction = auction;
                bidCluster.Products.Add(product);
                product.SelfAuction = auction;

                user.HisProducts.Add(product);

                //обновляем рейтинг аукциона
                RatingManager.IncreaseRating(auction, user, "userBeted");

                //обновляем рейтинг пользователя
                RatingManager.IncreaseRating(user, "userPlacedBet");

                //сохраняем все в базу
                await DbItemsAdder.AddProduct(db, product, photo, bidCluster);

                //добавляем нодики на клиенты
                AuctionHub.AddNodesToClients(info.title, photo.FileName, info.auctionId, product.Id);

                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
            
        }
    }
}

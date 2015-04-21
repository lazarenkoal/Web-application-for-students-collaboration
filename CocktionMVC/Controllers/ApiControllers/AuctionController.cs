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
                           photoPath = @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photos.First().FileName,
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
                        description = bid.Description.Trim(),
                        photoPath = @"http://cocktion.com/Images/Thumbnails/" + bid.Photos.First().FileName,
                        name = bid.Name.Trim(),
                        category = bid.Category.Trim(),
                        productId = bid.Id,
                    });
            return bidProducts;
        }

        public class CreateAuctionInfo
        {
            public string name { get; set; }

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
        public async Task<AuctionApiRespondModels.AuctionCreateStatus> CreateAuction(CreateAuctionInfo auctionInfo)
        {
            //Заводим возвращалку статуса
            AuctionApiRespondModels.AuctionCreateStatus info =
                new AuctionApiRespondModels.AuctionCreateStatus();
            try
            {
                //получаем информацию о пользователе
                string userId = User.Identity.GetUserId();

                //подключаемся к базе данных
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(userId);

                //создаем продукт и сохраняем информацию о нем.
                Product product = new Product(auctionInfo.name, auctionInfo.description,
                    auctionInfo.category, true, user);

                //инициализация аукциона
                Auction auction = new Auction(true, product, false, new ToteBoard(), user);

                auction.Rating = (int)(user.Rating * 0.4);
                auction.Owner = user;
                //Совершаем манипуляции со временем
                DateTimeManager.SetAuctionStartAndEndTime(auction, auctionInfo.timeBound);

                //увеличиваем рейтинг пользователя
                RatingManager.IncreaseRating(user, "userMadeAuction");
                Photo photo;
                //забиваем данные о фотке
                if (auctionInfo.photoId != -1)
                {
                    photo = db.Photos.Find(auctionInfo.photoId);
                }
                else
                {
                    photo = new Photo();
                    const string placeHolderName = "placeholder.jpg";
                    photo.FileName = placeHolderName;
                    string path = Path.Combine(
                        HttpContext.Current.Request.MapPath("~/Content/SiteImages"), placeHolderName);
                    photo.FilePath = path;

                    ThumbnailSet thumbNail = new ThumbnailSet();
                    thumbNail.FileName = placeHolderName;
                    thumbNail.FilePath = path;
                    photo.ThumbnailSets.Add(thumbNail);
                }

                product.Photos.Add(photo);
                //все в базу добавляем
                await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo, user);

                //обновляем список аукционов
                AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

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

        public class AuctionId
        {
            public int auctionId { get; set; }
        }

        /// <summary>
        /// Проверялка на то, является ли пользователь владельцем аукциона
        /// </summary>
        /// <returns>логический статус</returns>
        [HttpPost]
        [Authorize]
        public IsOwnerResponder CheckIfOwner(AuctionId auctId)
        {
            try
            {
                CocktionContext db = new CocktionContext();

                //находим аукцион и пользователя
                var auction = db.Auctions.Find(auctId.auctionId);
                string currentUserId = User.Identity.GetUserId();

                //проверяем является ли пользователь владельцем
                bool isOwner = auction.Owner.Id == currentUserId ? true : false;

                return new IsOwnerResponder(isOwner);
            }
            catch
            {
                return new IsOwnerResponder(null);
            }
        }

        /// <summary>
        /// Завершает аукцион с мобильника
        /// </summary>
        /// <returns>Стандартный статус</returns>
        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> EndAuction(AuctionId aId)
        {
            //TODO решить проблему взаимодействия с клиентами. Они не могут узнать, что аукцион завершен с мобильника
            CocktionContext db = new CocktionContext();
            string userId = User.Identity.GetUserId();
            try
            {
                //находим аукцион
                var auction = db.Auctions.Find(aId.auctionId);

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
            public string name { get; set; }

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
                Product product = new Product(info.name, info.description, info.category, false, user);

                Photo photo;
                //забиваем данные о фотке
                if (info.photoId != -1)
                {
                    photo = db.Photos.Find(info.photoId);
                }
                else
                {
                    photo = new Photo();
                    const string placeHolderName = "placeholder.jpg";
                    photo.FileName = placeHolderName;
                    string path = Path.Combine(
                        HttpContext.Current.Request.MapPath("~/Content/SiteImages"), placeHolderName);
                    photo.FilePath = path;

                    ThumbnailSet thumbNail = new ThumbnailSet();
                    thumbNail.FileName = placeHolderName;
                    thumbNail.FilePath = path;
                    photo.ThumbnailSets.Add(thumbNail);
                }
                //добавляем продукт
                //Коннектимся к базе
                var auction = db.Auctions.Find(info.auctionId);
                auction.BidProducts.Add(product);
                product.Auctions.Add(auction);

                //находи кластер
                BidCluster bidCluster = new BidCluster();
                bidCluster.UserId = userId;
                bidCluster.HostAuction = auction;
                bidCluster.Products.Add(product);

                //обновляем рейтинг аукциона
                RatingManager.IncreaseRating(auction, user, "userBeted");

                //обновляем рейтинг пользователя
                RatingManager.IncreaseRating(user, "userPlacedBet");

                //сохраняем все в базу
                await DbItemsAdder.AddProduct(db, product, photo, bidCluster);

                //добавляем нодики на клиенты
                AuctionHub.AddNodesToClients(info.name, photo.FileName, info.auctionId, product.Id);

                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
            
        }
    }
}

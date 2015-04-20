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
            ThumbnailGenerator.ResizeImage(postedFile, thumbNail.FilePath, 90);

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(userId);

            //создаем продукт и сохраняем информацию о нем.
            Product product = new Product(name, description, category,  true, user);

            //инициализация аукциона
            Auction auction = new Auction(true, product, false, new ToteBoard(), user);

            auction.Rating = (int)(user.Rating*0.4);

            //Совершаем манипуляции со временем
            DateTimeManager.SetAuctionStartAndEndTime(auction, hoursString, minutesString);

            //забиваем данные о фотке
            Photo photo = new Photo();
            photo.FileName = fileName;
            photo.FilePath = filePath;
            photo.Product = product;
            photo.ThumbnailSets.Add(thumbNail);

            //увеличиваем рейтинг пользователя
            RatingManager.IncreaseRating(user, "userMadeAuction");

            //все в базу добавляем
            await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo, user);

            //обновляем список аукционов
            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            //шлем ссылочку на фотку
            info.PhotoPath = "http://cocktion.com/Images/Thumbnails/" + fileName;

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

        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> AddBid()
        {
                string name, description, category, auctionId;
                RequestFormReader.ReadAddProductBetForm(HttpContext.Current.Request, out name,
                    out auctionId, out category, out description);

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
                }

                string thumbNailPath = HttpContext.Current.Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailSet thumbNail = new ThumbnailSet();
                thumbNail.FileName = fileName;
                thumbNail.FilePath = thumbNailPath + fileName;
                ThumbnailGenerator.ResizeImage(postedFile, thumbNail.FilePath, 60);

                //получаем информацию о пользователе
                string userId = User.Identity.GetUserId();

                //подключаемся к базе данных
                CocktionContext db = new CocktionContext();

                var user = db.AspNetUsers.Find(userId);

                //создаем продукт и сохраняем информацию о нем.
                Product product = new Product(name, description, category, false, user);
                //забиваем данные о фотке
                Photo photo = new Photo();
                photo.FileName = fileName;
                photo.FilePath = filePath;
                photo.Product = product;
                photo.ThumbnailSets.Add(thumbNail);

                //добавляем продукт
                //Коннектимся к базе
                int id = int.Parse(auctionId);
                var auction = db.Auctions.Find(id);
                auction.BidProducts.Add(product);
                product.Auctions.Add(auction);

                //находи кластер
                BidCluster bidCluster = new BidCluster();
                bidCluster.UserId = userId;
                bidCluster.HostAuction = db.Auctions.Find(id);
                bidCluster.Products.Add(product);

                //обновляем рейтинг аукциона
                RatingManager.IncreaseRating(db.Auctions.Find(id), user, "userBeted");

                //обновляем рейтинг пользователя
                RatingManager.IncreaseRating(user, "userPlacedBet");


                //сохраняем все в базу
                await DbItemsAdder.AddProduct(db, product, photo, bidCluster);

                //добавляем нодики на клиенты
                AuctionHub.AddNodesToClients(name, photo.FileName, id, product.Id);

                return new StatusHolder(true);
        }
    }
}

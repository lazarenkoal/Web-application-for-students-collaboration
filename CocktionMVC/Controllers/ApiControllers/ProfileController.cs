using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.MobileClientModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class ProfileController : ApiController
    {
        /// <summary>
        /// Контейнер для профайлайди поля
        /// </summary>
        public class ProfileInfo
        {
            public string id { get; set; }
        }

        /// <summary>
        /// Посылает мобильному клиенту информацию о профиле пользователя
        /// </summary>
        /// <param name="profInf">Контейнер с айдишником</param>
        /// <returns>Информацию о пользователе</returns>
        [HttpPost]
        [Authorize]
        public ProfileData GetInfo(ProfileInfo profInf)
        {
            CocktionContext db = new CocktionContext();
            AspNetUser user;
            if (profInf.id == "self")
            {
                string userId = User.Identity.GetUserId();
                user = db.AspNetUsers.Find(userId);
            }
            else
            {
                user = db.AspNetUsers.Find(profInf.id);
            }
            string name = user.UserRealName ?? "none";
            string surname = user.UserRealSurname ?? "none";
            string city = String.IsNullOrEmpty(user.City) ? "none" : user.City;
            string society = String.IsNullOrEmpty(user.SocietyName) ? "none" : user.SocietyName;
            ProfileData data;
            if (user.Selfie == null)
            {
                data = new ProfileData(name, surname, user.UserName,
                user.Rating, user.Eggs, user.HisAuctions.Count, user.HisProducts.Count, city,
                society, @"http://cocktion.com/Content/SiteImages/" + "anonPhoto.jpg");
            }
            else
            {
                   data = new ProfileData(name, surname, user.UserName,
                   user.Rating, user.Eggs, user.HisAuctions.Count, user.HisProducts.Count, city,
                   society, @"http://cocktion.com/Images/Thumbnails/" + user.Selfie.FileName);
            }
            
            return data;

        }

        /// <summary>
        /// Контейнер для информации о доме.
        /// </summary>
        public class ShortHouseData
        {
            public ShortHouseData(int id, string holderName, string houseName, string photoPath)
            {
                this.id = id;
                this.holderName = holderName;
                this.houseName = houseName;
                this.photoPath = @"http://cocktion.com/Images/Thumbnails/" + photoPath;
            }
            public int id { get; set; }
            public string holderName { get; set; }
            public string houseName { get; set; }

            public string photoPath {get; set;}
        }

        /// <summary>
        /// Контейнер для информации о профиле пользователя.
        /// </summary>
        public class ProfileData
        {
            public string name { get; set; }
            public string surname { get; set; }
            public string login { get; set; }
            public int? rating { get; set; }
            public int eggs { get; set; }
            public int auctionsAmount { get; set; }
            public int productsAmount { get; set; }
            public string city { get; set; }
            public string society { get; set; }
            public string photoPath { get; set; }
            public ProfileData(string name, string surname, string login, int? rating,
                int eggs, int auctionsAmount, int productsAmount, string city, string society, string photoPath)
            {
                this.name = name;
                this.surname = surname;
                this.login = login;
                this.rating = rating;
                this.eggs = eggs;
                this.auctionsAmount = auctionsAmount;
                this.productsAmount = productsAmount;
                this.city = city;
                this.society = society;
                this.photoPath = photoPath;
            }
        }

        /// <summary>
        /// Позволяет получить список всех домов,
        /// на которые подписан пользователь
        /// </summary>
        /// <returns>Коллекцию с информацией о домах</returns>
        [HttpPost]
        [Authorize]
        public List<ShortHouseData> GetMyHouses()
        {
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            List<ShortHouseData> data = (from x in user.SubHouses
                                         select new ShortHouseData(x.Id, x.Holder.Name,
                                             x.Faculty, x.Portrait.FileName)).ToList();
            return data;
        }

        /// <summary>
        /// Контейнер для информации об аукционах, которые пользователь хочет
        /// получить
        /// </summary>
        public class UsersAuctionsHouses
        {
            public List<AuctionInfo> active { get; set; }

            public List<AuctionInfo> finished { get; set; }

            public List<AuctionInfo> houses { get; set; }

            public List<AuctionInfo> products { get; set; }

            public UsersAuctionsHouses(List<AuctionInfo> active, List<AuctionInfo> finished,
                List<AuctionInfo> inHouse, List<AuctionInfo> inProducts)
            {
                this.active = active;
                this.finished = finished;
                houses = inHouse;
                products = inProducts;
            }
        }

        /// <summary>
        /// Позволяет получать информацию о различных аукционов для данного пользователя
        /// 1)Активные аукционы пользователя
        /// 2)Завершенные аукционы пользователя
        /// 3)Аукционы в колхозе пользователя
        /// 4)Аукцион
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public UsersAuctionsHouses GetMyAuctions()
        {
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            //Конвертация времени из ютс в местное
            DateTime controlTime = DateTimeManager.GetCurrentTime();

            //Выборка активных в данный момент аукционов,
            //которые принадлежат пользователю            
            List<AuctionInfo> myActive;
            try
            {
                if (user.Selfie == null)
                {
                    myActive = (from x in user.HisAuctions
                                where (x.IsActive && x.EndTime > controlTime)
                                select new AuctionInfo(x.SellProduct.Description,
                                    (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                    @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                    x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                    x.SellProduct.Category.Trim(),
                                    new UserInfo(x.Owner.Id, x.Owner.UserName, "anonPhoto.jpg",
                                        user.Friends.Contains(x.Owner)),
                                    isActive: true)).ToList();
                }
                else
                {
                    myActive = (from x in user.HisAuctions
                                where (x.IsActive && x.EndTime > controlTime)
                                select new AuctionInfo(x.SellProduct.Description,
                                    (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                    @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                    x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                    x.SellProduct.Category.Trim(),
                                    new UserInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName,
                                        user.Friends.Contains(x.Owner)),
                                    isActive: true)).ToList();
                }
                
            }
            catch
            {
                myActive = null;
            }

            //Выборка всех оконченных аукционов пользователя
            List<AuctionInfo> myFinished;
            try
            {
                myFinished = (from x in user.HisAuctions
                              where (x.IsActive == false || x.EndTime < controlTime)
                              select new AuctionInfo(x.SellProduct.Description,
                                  (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                  @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                  x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                  x.SellProduct.Category.Trim(), 
                                  new UserInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName,
                                      user.Friends.Contains(x.Owner)),
                                  isActive: false)).ToList();
            }
            catch
            {
                myFinished = null;
            }


            //Выборка всех в его домах
            List<AuctionInfo> inHisHouses = new List<AuctionInfo>();
            foreach (var house in user.SubHouses)
            {
                var housesInfo = (from x in house.Auctions
                                  where (x.IsActive && x.EndTime > DateTime.Now)
                                  select x).ToList();

                foreach (var hI in housesInfo)
                {
                    AspNetUser thisOwner;
                    try
                    {
                        thisOwner = hI.Owner;
                        var auctInfo = new AuctionInfo(hI.SellProduct.Description,
                                     (int)hI.EndTime.Subtract(controlTime).TotalMinutes,
                                     @"http://cocktion.com/Images/Thumbnails/" + hI.SellProduct.Photo.FileName,
                                     hI.SellProduct.Name.Trim(), hI.Id, hI.LeadProduct == null ? -1 : hI.LeadProduct.Id,
                                     hI.SellProduct.Category.Trim(),
                                    new UserInfo(thisOwner.Id, thisOwner.UserName, thisOwner.Selfie.FileName,
                                        user.Friends.Contains(thisOwner)),
                                    isActive: true);
                        if (auctInfo != null)
                        {
                            if (!inHisHouses.Any(x => x.auctionId == auctInfo.auctionId))
                                inHisHouses.Add(auctInfo);
                        }

                    }
                    catch
                    {
                        var auctInfo = new AuctionInfo(hI.SellProduct.Description,
                                     (int)hI.EndTime.Subtract(controlTime).TotalMinutes,
                                     @"http://cocktion.com/Images/Thumbnails/" + hI.SellProduct.Photo.FileName,
                                     hI.SellProduct.Name.Trim(), hI.Id, hI.LeadProduct == null ? -1 : hI.LeadProduct.Id,
                                     hI.SellProduct.Category.Trim(),
                                    null, isActive: true);
                        if (auctInfo != null)
                        {
                            if (!inHisHouses.Any(x => x.auctionId == auctInfo.auctionId))
                                inHisHouses.Add(auctInfo);
                        }
                    }

                }
            }

            //Выборка аукционов по всем его ставкам
            List<AuctionInfo> myBetsAuctions = new List<AuctionInfo>();
            try
            {
                foreach (var bet in user.HisProducts)
                {
                    if (bet.IsOnAuctionAsALot == false)
                    {
                        var auction = bet.SelfAuction;
                        if (auction != null)
                        {
                            bool isActive = (auction.IsActive && auction.EndTime > controlTime);
                            try
                            {
                                AuctionInfo aI = new AuctionInfo(auction.SellProduct.Description,
                                (int)auction.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photo.FileName,
                                 auction.SellProduct.Name.Trim(), auction.Id,
                                auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                                auction.SellProduct.Category.Trim(),
                                new UserInfo(auction.Owner.Id, auction.Owner.UserName, auction.Owner.Selfie.FileName,
                                    user.Friends.Contains(auction.Owner)),
                                isActive);
                                if (!myBetsAuctions.Any(x => x.auctionId == aI.auctionId))
                                    myBetsAuctions.Add(aI);
                            }
                            catch
                            {
                                AuctionInfo aI = new AuctionInfo(auction.SellProduct.Description,
                                (int)auction.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photo.FileName,
                                auction.SellProduct.Name.Trim(), auction.Id,
                                auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                                auction.SellProduct.Category.Trim(),
                                null, isActive);

                                if (!myBetsAuctions.Any(x => x.auctionId == aI.auctionId))
                                    myBetsAuctions.Add(aI);
                            }

                        }
                    }
                }
            }
            catch 
            {
            }

            return new UsersAuctionsHouses(myActive, myFinished, inHisHouses, myBetsAuctions);
        }


        /// <summary>
        /// Меняет пользовательскую аватарку
        /// </summary>
        /// <returns>Стандартный ответ сервера</returns>
        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> UploadProfilePhoto()
        {
            try
            {
                //получаем файлик
                string fileName = "";
                HttpPostedFile postedFile = null;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        postedFile = httpRequest.Files[file];
                        string extension = Path.GetExtension(postedFile.FileName);
                        fileName = Guid.NewGuid() + extension; //генерируем новое имя для фотки
                    }
                }

                //добавляем фоточку и фабмнейл
                string thumbNailPath = HttpContext.Current.Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailGenerator.ResizeImage(postedFile, thumbNailPath + fileName, 90);

                string userId = User.Identity.GetUserId();
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(userId);
                Picture picture = new Picture();
                picture.FileName = fileName;
                picture.FilePath = thumbNailPath;
                db.Pictures.Add(picture);
                user.Selfie = picture;
                await DbItemsAdder.SaveDb(db);
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }

        }

        /// <summary>
        /// Служит в качестве контейнера для полей, котоыре
        /// пользователь хочет поменять из мобильного клиента
        /// </summary>
        public class ShortProfile
        {
            public string name { get; set; }
            public string surname { get; set; }
            public string society { get; set; }
            public string city { get; set; }
        }

        /// <summary>
        /// Позволяет пользователю редактировать информацию о профиле
        /// Можно менять:
        /// 1) Имя
        /// 2) Фамилию
        /// 3) Город
        /// 4) Общество
        /// </summary>
        /// <param name="editInfo">Параметры для изменения</param>
        /// <returns>Стандартный ответ сервера</returns>
        [HttpPost]
        [Authorize]
        public async Task<StatusHolder> EditProfile(ShortProfile editInfo)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());

                if (!String.IsNullOrEmpty(editInfo.name))
                    user.UserRealName = editInfo.name;

                if (!String.IsNullOrEmpty(editInfo.surname))
                    user.UserRealSurname = editInfo.surname;

                if (!String.IsNullOrEmpty(editInfo.society))
                    user.SocietyName = editInfo.society;

                if (!String.IsNullOrEmpty(editInfo.city))
                    user.City = editInfo.city;

                await DbItemsAdder.SaveDb(db);
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
            
        }

        /// <summary>
        /// Позволяет получить список всех людей, на которых подписался
        /// данный пользователь
        /// </summary>
        /// <returns>Коллекцию информаторов</returns>
        [HttpPost]
        [Authorize]
        public List<UserInfo> GetMyInformators()
        {
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            List<UserInfo> informators = new List<UserInfo>();
            foreach (var i in user.Friends)
            {
                try
                {
                    informators.Add(new UserInfo(i.Id, i.UserName, i.Selfie.FileName, true));
                }
                catch
                {

                }
            }
            return informators;
        }
    }
}

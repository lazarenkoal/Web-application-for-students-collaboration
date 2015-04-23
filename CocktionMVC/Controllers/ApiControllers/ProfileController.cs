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
        public class ProfileInfo
        {
            public string profileId { get; set; }
        }

        [HttpPost]
        [Authorize]
        public ProfileData GetInfo(ProfileInfo profInf)
        {
            CocktionContext db = new CocktionContext();
            AspNetUser user;
            if (profInf.profileId == "self")
            {
                string userId = User.Identity.GetUserId();
                user = db.AspNetUsers.Find(userId);
            }
            else
            {
                user = db.AspNetUsers.Find(profInf.profileId);
            }
            string name = user.UserRealName ?? "none";
            string surname = user.UserRealSurname ?? "none";
            string city = String.IsNullOrEmpty(user.City) ? "none" : user.City;
            string society = String.IsNullOrEmpty(user.SocietyName) ? "none" : user.SocietyName;

            ProfileData data = new ProfileData(name, surname, user.UserName,
                user.Rating, user.Eggs, user.HisAuctions.Count, user.HisProducts.Count, city,
                society, @"http://cocktion.com/Images/Thumbnails/"+user.Selfie.FileName);
            return data;

        }

        public class ShortHouseData
        {
            public ShortHouseData(int id, string holderName, string houseName)
            {
                this.id = id;
                this.holderName = holderName;
                this.houseName = houseName;
            }
            public int id { get; set; }
            public string holderName { get; set; }
            public string houseName { get; set; }
        }
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
                myActive = (from x in user.HisAuctions
                            where (x.IsActive && x.EndTime > DateTime.Now)
                            select new AuctionInfo(x.SellProduct.Description,
                                (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                x.SellProduct.Category.Trim(), new ownerInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName),
                                isActive: true)).ToList();
            }
            catch
            {
                myActive = null;
            }

            ////Выборка всех оконченных аукционов пользователя
            List<AuctionInfo> myFinished;
            try
            {
                myFinished = (from x in user.HisAuctions
                              where (x.IsActive == false || x.EndTime < DateTime.Now)
                              select new AuctionInfo(x.SellProduct.Description,
                                  (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                  @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                  x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                  x.SellProduct.Category.Trim(), new ownerInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName),
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
                                    new ownerInfo(thisOwner.Id, thisOwner.UserName, thisOwner.Selfie.FileName),
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
                            bool isActive = (auction.IsActive && auction.EndTime > DateTime.Now);
                            try
                            {
                                AuctionInfo aI = new AuctionInfo(auction.SellProduct.Description,
                                (int)auction.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photo.FileName,
                                 auction.SellProduct.Name.Trim(), auction.Id,
                                auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                                auction.SellProduct.Category.Trim(),
                                new ownerInfo(auction.Owner.Id, auction.Owner.UserName, auction.Owner.Selfie.FileName),
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

        public class ShortProfile
        {
            public string name { get; set; }
            public string surname { get; set; }
            public string society { get; set; }
            public string city { get; set; }
        }

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

                db.SaveChanges();
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
            
        }
    }
}

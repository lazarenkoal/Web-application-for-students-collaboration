﻿using System;
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
        public ProfileInfoMobile GetInfo(ProfileInfo profInf)
        {
            CocktionContext db = new CocktionContext();
            if (profInf.profileId == "self")
            {
                string userId = User.Identity.GetUserId();
                var userProfile = db.AspNetUsers.Find(userId);
                ProfileInfoMobile info = new ProfileInfoMobile(@"http://cocktion.com/Content/SiteImages/anonPhoto.jpg",
                    userProfile.UserRealName, userProfile.UserRealSurname, userProfile.Eggs, 100, "Шлюхи",
                    @"http://cocktion.com/Content/SiteImages/girl.jpg", "Тачки", @"http://cocktion.com/Content/SiteImages/car.jpg",
                    "Блэкджек", @"http://cocktion.com/Content/SiteImages/blackjack.jpg", "Бабло", @"http://cocktion.com/Content/SiteImages/money.jpg");
                return info;
            }
            else
            {
                var userProfile = db.AspNetUsers.Find(profInf.profileId);
                ProfileInfoMobile info = new ProfileInfoMobile(@"http://cocktion.com/Content/SiteImages/anonPhoto.jpg",
                    userProfile.UserRealName, userProfile.UserRealSurname, userProfile.Eggs, 100, "Шлюхи",
                    @"http://cocktion.com/Content/SiteImages/girl.jpg", "Тачки", @"http://cocktion.com/Content/SiteImages/car.jpg",
                    "Блэкджек", @"http://cocktion.com/Content/SiteImages/blackjack.jpg", "Бабло", @"http://cocktion.com/Content/SiteImages/money.jpg");
                return info;
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
                                x.SellProduct.Category.Trim(), new ownerInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName))).ToList();
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
                                  x.SellProduct.Category.Trim(), new ownerInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName))).ToList();
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
                                    new ownerInfo(thisOwner.Id, thisOwner.UserName, thisOwner.Selfie.FileName));
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
                                    null);
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
                            try
                            {
                                AuctionInfo aI = new AuctionInfo(auction.SellProduct.Description,
                                (int)auction.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photo.FileName,
                                 auction.SellProduct.Name.Trim(), auction.Id,
                                auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                                auction.SellProduct.Category.Trim(),
                                new ownerInfo(auction.Owner.Id, auction.Owner.UserName, auction.Owner.Selfie.FileName));
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
                                null);

                                if (!myBetsAuctions.Any(x => x.auctionId == aI.auctionId))
                                    myBetsAuctions.Add(aI);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                myBetsAuctions = new List<AuctionInfo> { new AuctionInfo(e.Message, 3, "", "", 2, 3, "", null) };
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
    }
}

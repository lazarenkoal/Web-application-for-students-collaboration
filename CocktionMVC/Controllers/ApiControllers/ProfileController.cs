using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
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
                            where x.IsActive
                            select new AuctionInfo(x.SellProduct.Description,
                                (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photos.First().FileName,
                                x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                x.SellProduct.Category.Trim())).ToList();
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
                              where x.IsActive == false
                              select new AuctionInfo(x.SellProduct.Description,
                                  (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                  @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photos.First().FileName,
                                  x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                  x.SellProduct.Category.Trim())).ToList();
            }
            catch
            {
                myFinished = null;
            }
            

            //Выборка всех в его домах
            List<AuctionInfo> inHisHouses = new List<AuctionInfo>();
            try
            {
                foreach (var house in user.SubHouses)
                {
                    var housesInfo = (from x in house.Auctions
                                      where x.IsActive
                                      select new AuctionInfo(x.SellProduct.Description,
                                          (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                          @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photos.First().FileName,
                                          x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                          x.SellProduct.Category.Trim())).ToList();

                    foreach (var hI in housesInfo)
                    {
                        if (!inHisHouses.Contains(hI))
                            inHisHouses.Add(hI);
                    }
                }
            }
            catch
            {
                inHisHouses = null;
            }
            

            //Выборка аукционов по всем его ставкам
            List<AuctionInfo> myBetsAuctions = new List<AuctionInfo>();
            try
            {
                var bets = (from x in db.AuctionBids
                    where x.UserId == user.Id
                    select x);
                foreach (var bet in bets)
                {
                    var auction = bet.HostAuction;
                    var aI = new AuctionInfo(auction.SellProduct.Description,
                        (int) auction.EndTime.Subtract(controlTime).TotalMinutes,
                        @"http://cocktion.com/Images/Thumbnails/" + auction.SellProduct.Photos.First().FileName,
                        auction.SellProduct.Name.Trim(), auction.Id,
                        auction.LeadProduct == null ? -1 : auction.LeadProduct.Id,
                        auction.SellProduct.Category.Trim());

                    if (!myBetsAuctions.Contains(aI))
                        myBetsAuctions.Add(aI);
                }
            }
            catch
            {
                myBetsAuctions = null;
            }
            
            return new UsersAuctionsHouses(myActive, myFinished, inHisHouses, myBetsAuctions);
        }
    }
}

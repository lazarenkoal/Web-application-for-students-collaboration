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
    }
}

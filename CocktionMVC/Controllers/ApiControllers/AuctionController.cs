using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using System.Threading.Tasks;
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
             
                auctiInfo.Add(
                       new AuctionInfo
                       {
                           AuctionCategory = auction.SellProduct.Category.Trim(),
                           AuctionDescription = auction.SellProduct.Description.Trim(),
                           AuctionEndTime = auction.EndTime,
                           AuctionImage = @"http://cocktion1.azurewebsites.net/Images/Thumbnails/" + auction.SellProduct.Photos.First().FileName,
                           AuctionStartTime = auction.StartTime,
                           AuctionTitle = auction.SellProduct.Name.Trim(),
                           AuctionId = auction.Id
                       }
                       );
            }//end of foreach

            return auctiInfo;
        }//end of GetActiveAuctions()

        /// <summary>
        /// Метод для получения конкретного аукциона по айди
        /// </summary>
        /// <param name="id">Айди для доступа к аукциону</param>
        /// <returns>Джейсон с инфой об аукционе</returns>
        [HttpPost]
        public AuctionInfo GetDirectAuction(int id)
        {
            CocktionContext db = new CocktionContext();
            Auction auction = db.Auctions.Find(id);
            return new AuctionInfo
            {
                AuctionCategory = auction.SellProduct.Category.Trim(),
                AuctionDescription = auction.SellProduct.Description.Trim(),
                AuctionEndTime = auction.EndTime,
                AuctionImage = @"http://cocktion1.azurewebsites.net/Images/Thumbnails/" + auction.SellProduct.Photos.First().FileName,
                AuctionStartTime = auction.StartTime,
                AuctionTitle = auction.SellProduct.Name.Trim(),
                AuctionId = auction.Id
            };
        }
        //В АПИ КОНТРОЛЛЕРАХ РУТИНГ ДРУГОЙ!!!

        /// <summary>
        /// Метод отправляет на мобильный клиент информацию о всех товарах, 
        /// которые поставлены на данном аукционе
        /// </summary>
        /// <param name="auctionId">Айди аукциона</param>
        /// <returns>Лист с информацией о продуктах</returns>
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
                        ProductFileName =  @"http://cocktion1.azurewebsites.net/Images/Thumbnails/" + bid.Photos.First().FileName,
                        ProductName = bid.Name.Trim(),
                        ProductCategory = bid.Category.Trim(),
                        ProductId = bid.Id
                    });
            return bidProducts;
        }
    }
}

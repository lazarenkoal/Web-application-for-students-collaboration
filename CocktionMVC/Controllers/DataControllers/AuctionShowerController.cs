using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Controllers
{
    public class AuctionShowerController : Controller
    {
        /// <summary>
        /// Показывает аукционы, прошедшие и закончившиеся 
        /// в данном доме
        /// </summary>
        /// <param name="id">Айдишник дома</param>
        /// <returns>Страничку, показывающую все завершившиеся аукционы</returns>
        [OutputCache(Duration = 100)]
        public ActionResult ShowOldAuctions(int? id)
        {
            List<Auction> auctions = new List<Auction>();
            CocktionContext db = new CocktionContext();
            //получаем все закончившиеся аукционы в доме
            auctions = db.Houses.Find(id).Auctions.Where(x => x.IsActive == false).ToList();
            return View(auctions);
        }

        /// <summary>
        /// Показывает аукционы, проходящие в данный момент
        /// в данном аукционном доме
        /// </summary>
        /// <param name="id">Айдишник дома</param>
        /// <returns>Страничку, показывающую все активные аукционы</returns>
        [OutputCache(Duration = 100)]
        public ActionResult ShowActiveAuctions(int? id)
        {
            List<Auction> auctions = new List<Auction>();
            CocktionContext db = new CocktionContext();
            //получаем все активные аукционы в доме
            auctions = db.Houses.Find(id).Auctions.Where(x => x.IsActive).ToList();
            return View(auctions);
        }
    }
}
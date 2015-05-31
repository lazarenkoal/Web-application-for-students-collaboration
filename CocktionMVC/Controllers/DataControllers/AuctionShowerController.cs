using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Functions;

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
        public ActionResult ShowOldAuctions(int? id)
        {
            CocktionContext db = new CocktionContext();

            //получаем все закончившиеся аукционы в доме
            var currentTime = DateTimeManager.GetCurrentTime();
            var auctions = db.Houses.Find(id).Auctions.Where(x => (x.IsActive == false && x.EndTime < currentTime)).ToList();
            return View(auctions);
        }

        /// <summary>
        /// Показывает аукционы, проходящие в данный момент
        /// в данном аукционном доме
        /// </summary>
        /// <param name="id">Айдишник дома</param>
        /// <returns>Страничку, показывающую все активные аукционы</returns>
        public ActionResult ShowActiveAuctions(int? id)
        {
            CocktionContext db = new CocktionContext();

            //получаем все активные аукционы в доме
            var currentTime = DateTimeManager.GetCurrentTime();
            var auctions = db.Houses.Find(id).Auctions.Where(x => (x.IsActive && x.EndTime >= currentTime)).ToList();
            return View(auctions);
        }
    }
}
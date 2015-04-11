using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;

namespace CocktionMVC.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            CocktionContext db = new CocktionContext();
            List<AspNetUser> users = db.AspNetUsers.ToList();
            return View(users);
        }

        public ActionResult User(string id)
        {
            CocktionContext db = new CocktionContext();
            AspNetUser user;
            //находим пользователя
            user = db.AspNetUsers.Find(id);

            //Собираем рейтинг, яйца, количество аукционов
            int? userRating = user.Rating;
            int userEggs = user.Eggs;

            //TODO собираем количество побед


            //собираем колчество ставок
            int userBets = db.AuctionBids.Count(x => x.UserId == user.Id);

            //TODO интересы

            //TODO подписки

            //TODO количество дней с нами

            //Собираем все его отзывы
            List<UsersFeedback> feeds = db.Feedbacks.Where(x => x.UsersId == user.Id).ToList();

            ProfileViewModel model = new ProfileViewModel(userEggs, user.HisAuctions.Count, userBets,
                user.UserRealSurname, user.UserRealName, userRating, 56, user.HisAuctions.ToList<Auction>(), id, feeds);

            return View(model);
        }
    }
}
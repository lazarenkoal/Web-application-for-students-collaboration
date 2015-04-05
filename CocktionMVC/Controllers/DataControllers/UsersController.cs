using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            //Cобираем имя и фамилию
            string userName = user.UserRealName;
            string userSurname = user.UserRealSurname;

            //Собираем рейтинг, яйца, количество аукционов
            int? userRating = user.Rating;
            int userEggs = user.Eggs;

            //TODO собираем количество побед


            //собираем колчество ставок
            int userBets = db.AuctionBids.Count(x => x.UserId == user.Id);

            //TODO интересы

            //TODO подписки

            //TODO количество дней с нами


            //собираем его аукционы и их количество
            List<Auction> auctions = db.Auctions.Where(x => x.OwnerId == user.Id).ToList();
            int userAuctionsAmount = auctions.Count();

            //Собираем все его отзывы
            List<UsersFeedback> feeds = db.Feedbacks.Where(x => x.UsersId == user.Id).ToList();

            ProfileViewModel model = new ProfileViewModel(userEggs, userAuctionsAmount, userBets,
                userSurname, userName, userRating, 56, auctions, id, feeds);

            return View(model);
        }
    }
}
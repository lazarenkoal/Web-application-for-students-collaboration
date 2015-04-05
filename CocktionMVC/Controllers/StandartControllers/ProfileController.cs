using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CocktionMVC.Controllers.StandartControllers
{
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Выводит пользователя на страничку с информацией о его аккаунте
        /// </summary>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {//если пользватель авторизован
                CocktionContext db = new CocktionContext();
                AspNetUser user;
                //находим пользователя
                    user = db.AspNetUsers.Find(User.Identity.GetUserId());

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
                    userSurname, userName, userRating, 56, auctions, User.Identity.GetUserId(), feeds);

                return View(model);
            }
            else
            {//если пользователь не авторизован
                return RedirectToAction("HowItCouldBe");
            }
        }

        public JsonResult AddUsersFeedback()
        {
            try
            {
                //Получаем текст сообщения и айдишник пользователя, который будет тут задействован
                string message = Request.Form.GetValues("message")[0];
                string userId = Request.Form.GetValues("userId")[0];
                CocktionContext db = new CocktionContext();

                string currentUserId = User.Identity.GetUserId();

                //находим пользователя (который оставляет комментарий)
                var user = db.AspNetUsers.Find(currentUserId);

                UsersFeedback feedback = new UsersFeedback(user.UserRealName, user.UserRealSurname,
                    currentUserId, userId, message);

                db.Feedbacks.Add(feedback);
                db.SaveChanges();

                FeedBackRespond respond = new FeedBackRespond(user.UserRealName, user.UserRealSurname, "success");
                return Json(respond);
            }
            catch (Exception)
            {
                FeedBackRespond respond = new FeedBackRespond(null, null, "failure");
                return Json(respond);
            }
        }

        class FeedBackRespond
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Status { get; set; }

            public FeedBackRespond(string name, string surname, string status)
            {
                Name = name;
                Surname = surname;
                Status = status;
            }
        }

        /// <summary>
        /// Показывает пользователю то, какой могла бы быть его страничка
        /// </summary>
        public ActionResult HowItCouldBe()
        {
            //TODO сделать эту страниу
            return View();
        }

     }
}
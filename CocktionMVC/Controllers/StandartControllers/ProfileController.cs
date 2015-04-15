using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.StandartControllers
{
    public class ProfileController : Controller
    {
        /// <summary>
        /// Выводит пользователя на страничку с информацией о его аккаунте
        /// </summary>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {//если пользватель авторизован
                CocktionContext db = new CocktionContext();

                //находим пользователя
                    var user = db.AspNetUsers.Find(User.Identity.GetUserId());

                //TODO собираем количество побед
                

                //собираем колчество ставок
                int userBets = db.AuctionBids.Count(x => x.UserId == user.Id);

                //TODO интересы

                //TODO подписки

                //TODO количество дней с нами
                

                //собираем его аукционы и их количество
                List<Auction> auctions = user.HisAuctions.ToList();

                //Собираем все его отзывы
                List<UsersFeedback> feeds = db.Feedbacks.Where(x => x.UsersId == user.Id).ToList();

                ProfileViewModel model = new ProfileViewModel(user.Eggs, user.HisAuctions.Count, userBets,
                    user.UserRealSurname, user.UserRealName, user.Rating, 56, auctions, user.Id, feeds, user.SubHouses.ToList(),
                    user.Friends.ToList());

                return View(model);
            }

            //если пользователь не авторизован
            return RedirectToAction("HowItCouldBe");
        }

        /// <summary>
        /// Добавляет отзыв о пользователе в базу
        /// </summary>
        public async Task<JsonResult> AddUsersFeedback()
        {
            try
            {
                //Получаем текст сообщения и айдишник пользователя, который будет тут задействован
                var messageHolder = Request.Form.GetValues("message");
                if (messageHolder != null)
                {
                    string message = messageHolder[0].Trim();
                    var userIdHolder = Request.Form.GetValues("userId");
                    if (userIdHolder != null)
                    {
                        string userId = userIdHolder[0].Trim();
                        CocktionContext db = new CocktionContext();

                        string currentUserId = User.Identity.GetUserId();

                        //находим пользователя (который оставляет комментарий)
                        var user = db.AspNetUsers.Find(currentUserId);

                        //инициализация фидбека
                        UsersFeedback feedback = new UsersFeedback(user.UserRealName, user.UserRealSurname,
                            currentUserId, userId, message);
                        db.Feedbacks.Add(feedback);

                        //сохранение значений в базу
                        await DbItemsAdder.SaveDb(db);

                        return Json(new FeedBackRespond(user.UserRealName, user.UserRealSurname, "success"));
                    }
                }
                return Json(new FeedBackRespond(null, null, "failure"));
            }
            catch
            {
                return Json(new FeedBackRespond(null, null, "failure"));
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
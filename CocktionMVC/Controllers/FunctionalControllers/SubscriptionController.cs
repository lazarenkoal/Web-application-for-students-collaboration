using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.FunctionalControllers
{
    public class SubscriptionController : Controller
    {
        /// <summary>
        /// Позволяет отписаться от дома
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult UsubscribeFromHouse()
        {
            var strings = Request.Form.GetValues("houseId");
            if (strings != null)
            {
                int houseId = int.Parse(strings[0]);
               
                CocktionContext db = new CocktionContext();
                
                var house = db.Houses.Find(houseId);
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                
                user.SubHouses.Remove(house);
                house.Inhabitants.Remove(user);
                
                db.SaveChanges();
                
                return Json(new StatusHolder(true));
            }
            return Json(new StatusHolder(false));
        }

        /// <summary>
        /// Позволяет подписаться на дом
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult SubscribeOnHouse()
        {
            var strings = Request.Form.GetValues("houseId");
            if (strings != null)
            {
                int houseId = int.Parse(strings[0]);
                CocktionContext db = new CocktionContext();
                
                var house = db.Houses.Find(houseId);
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                
                user.SubHouses.Add(house);
                house.Inhabitants.Add(user);
                
                if (user.SocietyName == null)
                    user.SocietyName = house.Holder.Name;

                if (user.City == null)
                    user.City = house.Holder.City;
                
                //добавляем дому немного рейтинга
                RatingManager.IncreaseRating(house, "subscriberAdded");

                db.SaveChanges();
                
                return Json(new StatusHolder(true));
            }
            return Json(new StatusHolder(false));
        }

        /// <summary>
        /// Позволяет проверить, подписан ли пользователь
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult CheckHouseSubscription()
        {
            var strings = Request.Form.GetValues("modelId");
            if (strings != null)
            {
                int houseId = int.Parse(strings[0]);
                
                CocktionContext db = new CocktionContext();
                
                var house = db.Houses.Find(houseId);
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                
                if (user.SubHouses.Contains(house))
                {
                    return Json(new StatusHolder(true));
                }
                return Json(new StatusHolder(false));
            }
            return Json(new StatusHolder(false));
        }

        /// <summary>
        /// Пощзволяет отписаться от пользователя
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult UnsubscribeFromUser()
        {

            var userIdContainer = Request.Form.GetValues("userId");
            if (userIdContainer != null)
            {
                string userId = userIdContainer[0];
                var currentUserId = User.Identity.GetUserId();
                
                CocktionContext db = new CocktionContext();
                
                var userToAdd = db.AspNetUsers.Find(userId);
                var currentUser = db.AspNetUsers.Find(currentUserId);
                
                currentUser.Friends.Remove(userToAdd);
                
                db.SaveChanges();
                
                return Json(new StatusHolder(true));
            }
            return Json(new StatusHolder(false));
        }

        /// <summary>
        /// Позволяет подписаться на пользователя
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult SubscribeOnUser()
        {
            var userIdContainer = Request.Form.GetValues("userId");
            if (userIdContainer != null)
            {
                string userId = userIdContainer[0];
                var currentUserId = User.Identity.GetUserId();

                CocktionContext db = new CocktionContext();
                
                var userToAdd = db.AspNetUsers.Find(userId);
                var currentUser = db.AspNetUsers.Find(currentUserId);
                
                currentUser.Friends.Add(userToAdd);

                //добавляем рейтиг пользовтелю
                RatingManager.IncreaseRating(currentUser, "userGotSubscriber");

                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            return Json(new StatusHolder(false));
        }

        /// <summary>
        /// Позволяет провертить, подписан ли данный пользователь на того, 
        /// который прислан в поле юсерАйди
        /// </summary>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public JsonResult CheckUsersSubscription()
        {
            var strings = Request.Form.GetValues("userId");
            if (strings != null)
            {
                string userId = strings[0];
               
                CocktionContext db = new CocktionContext();
                
                var user = db.AspNetUsers.Find(userId);
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                
                if (currentUser.Friends.Contains(user))
                {
                    return Json(new StatusHolder(true));
                }
                return Json(new StatusHolder(false));
            }
            return Json(new StatusHolder(false));
        }
    }
}
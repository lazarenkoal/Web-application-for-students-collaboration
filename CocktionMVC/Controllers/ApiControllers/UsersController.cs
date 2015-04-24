using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels.MobileClientModels;
namespace CocktionMVC.Controllers.ApiControllers
{
    public class UsersController : ApiController
    {
        public class UserIdHolder
        {
            public string id { get; set; }
        }

        /// <summary>
        /// Позволяет подписаться на пользователя с мобильного клиента
        /// </summary>
        /// <param name="id">Айдишник пользователя, на которого надо подписаться</param>
        /// <returns>Стандартный ответ сервера</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder SubscribeOnUser(UserIdHolder id)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                var userToSubscribeOn = db.AspNetUsers.Find(id.id);
                if (!user.Friends.Contains(userToSubscribeOn))
                {
                    user.Friends.Add(userToSubscribeOn);
                    db.SaveChanges();
                }
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
        }

        /// <summary>
        /// Позволяет отписаться от пользователя
        /// </summary>
        /// <param name="id">Айдишник пользователя, от которого надо отписаться</param>
        /// <returns>Стандартный ответ сервера</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder UnsubscribeFromUser(UserIdHolder id)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                var userToUnsubscribeOn = db.AspNetUsers.Find(id.id);
                if (user.Friends.Contains(userToUnsubscribeOn))
                {
                    user.Friends.Remove(userToUnsubscribeOn);
                    db.SaveChanges();
                }
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
        }

        /// <summary>
        /// Посылает в ответ на запрос список всех пользователей кокшна
        /// </summary>
        /// <returns>Коллекцию пользователей</returns>
        [HttpPost]
        [Authorize]
        public List<UserInfo> GetAllUsers()
        {
            CocktionContext db = new CocktionContext();
            List<UserInfo> users = new List<UserInfo>();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            foreach (var i in db.AspNetUsers.ToList())
            {
                try
                {
                    users.Add(new UserInfo(i.Id, i.UserName, i.Selfie.FileName, user.Friends.Contains(i)));
                }
                catch
                {

                }
            }
            return users;
        }
    }
}

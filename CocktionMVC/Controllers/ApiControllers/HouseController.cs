using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.HouseRelatedModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.ApiControllers
{
    /// <summary>
    /// Контейнер для методов, посылающих дома и все, что с ними связано мобильным клиентам
    /// </summary>
    public class HouseController : ApiController
    {
        /// <summary>
        /// Отправляет на мобильный клиент список всех доступных 
        /// хаус-холдеров
        /// </summary>
        /// <returns>Коллекцию хаусхолдеров</returns>
        [HttpPost]
        [Authorize]
        public List<Guild> GetGuilds()
        {
            List<Guild> guilds = new List<Guild>();

            CocktionContext db = new CocktionContext();

            var holders = db.HouseHolders.ToList();
            int amountOfHolders = holders.Count;

            for (int i = 0; i < amountOfHolders; i++)
            {
                guilds.Add(new Guild(holders[i].Name, "http://cocktion.com/Images/Thumbnails/" + holders[i].PhotoCard.FileName,
                    holders[i].Id));
            }

            return guilds;
        }


        public class IdContainer
        {
            public int id { get; set; }
        }

        /// <summary>
        /// Посылает на мобилку коллекцию с домами хаусхолдера
        /// </summary>
        /// <returns>Коллекцию</returns>
        [HttpPost]
        [Authorize]
        public List<GuildsHouse> GetGuildsHouses(IdContainer gn)
        {
            List<GuildsHouse> houses = new List<GuildsHouse>();
            CocktionContext db = new CocktionContext();
            var holders = db.HouseHolders.Find(gn.id).Houses.ToList();
            foreach (var holder in holders)
            {
                houses.Add(new GuildsHouse(holder.Holder.Name, holder.Id, holder.Adress,
                    "http://cocktion.com/Images/Thumbnails/" + holder.Portrait.FileName));
            }
            return houses;
        }


        /// <summary>
        /// Посылает на мобилку информаию о доме
        /// </summary>
        /// <returns>Класс с инфой о доме</returns>
        [HttpPost]
        [Authorize]
        public HouseMobile GetHouse(IdContainer hsNum)
        {
            //TODO добавить везде проверки
            CocktionContext db = new CocktionContext();

            //обработка guildId из формы
            House house = db.Houses.Find(hsNum.id);
            HouseMobile mobileHouse = new HouseMobile(@"http://cocktion.com/Images/Thumbnails" + house.Portrait.FileName, house.Likes,
                0, house.Rating, house.Inhabitants.Count, house.Auctions.Count, house.Description);

            return mobileHouse;
        }

        /// <summary>
        /// Шлет все посты с форума на мобилку
        /// </summary>
        /// <returns>Коллекцию с постами для мобилки</returns>
        [HttpPost]
        [Authorize]
        public List<ForumPostMobile> GetHouseForumPosts(IdContainer idContainer)
        {
            CocktionContext db = new CocktionContext();
            var forumPosts = db.Houses.Find(idContainer.id).Posts;
            List<ForumPostMobile> posts = new List<ForumPostMobile>(forumPosts.Count);
            
            posts.AddRange(forumPosts.Select(post => new ForumPostMobile(post.AuthorName, post.Message, post.Likes)));

            return posts;
        }

        public class Message
        {
            public string message { get; set; }
            public int houseId { get; set; }
        }

        /// <summary>
        /// Позволяет отписываться от дома.
        /// </summary>
        /// <param name="id">Айдишник дома, от которого надо отписаться</param>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder UnsubscribeFromHouse(IdContainer id)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                var house = db.Houses.Find(id.id);
                if (user.SubHouses.Contains(house))
                {
                    user.SubHouses.Remove(house);
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
        /// Позволяет подписаться на дом
        /// </summary>
        /// <param name="id">Айдишник дома, на который надо подписаться</param>
        /// <returns>Стандартный ответ</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder SubscribeOnHouse(IdContainer id)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                var house = db.Houses.Find(id.id);
                if (!user.SubHouses.Contains(house))
                {
                    user.SubHouses.Add(house);
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
        /// Добавляет пост на форум конкретного дома
        /// </summary>
        /// <returns>Статус удалось или нет</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder SendPost(Message msg)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                
                //Получаем информацию для постав
                string authorName = User.Identity.Name;

                //Находим дом
                var house = db.Houses.Find(msg.houseId);

                //добавляем пост
                house.Posts.Add(new ForumPost(msg.message, authorName));
                db.SaveChanges();

                //возвращаем статус
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
        }
    }
}

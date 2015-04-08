using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.HouseRelatedModels;

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

            //TODO сделать в базе данных несколько другую модель с этим добром
            guilds.Add(new Guild("МГИМО", @"http://cocktion.com/Content/UniversityLogos/" + "mgimoLogo.jpg", 0));
            guilds.Add(new Guild("МФТИ", @"http://cocktion.com/Content/UniversityLogos/" + "mftiLogo.jpg", 1));
            guilds.Add(new Guild("НИУ ВШЭ", @"http://cocktion.com/Content/UniversityLogos/" + "hseLogo.jpg", 2));
            guilds.Add(new Guild("МГУ", @"http://cocktion.com/Content/UniversityLogos/" + "msuLogo.jpg", 3));
            guilds.Add(new Guild("НИЯУ МИФИ", @"http://cocktion.com/Content/UniversityLogos/" + "mifiLogo.jpg", 4));
            guilds.Add(new Guild("МГТУ", @"http://cocktion.com/Content/UniversityLogos/" + "mgtuLogo.jpg", 5));

            return guilds;
        }

        /// <summary>
        /// Делает коллекцию домов в мобило-читаемом виде
        /// </summary>
        /// <param name="db">База, из которой надо доставать эти значения</param>
        /// <param name="universityName">Университет, который хаусхолдит</param>
        /// <returns>Коллекцию, пригодную для отображения на телефоне</returns>
        private List<GuildsHouse> BuildHouseForMobile(CocktionContext db, string universityName)
        {
            List<House> houses = (from x in db.Houses
                where x.University == universityName
                select x).ToList();   
            int amountOfHouses = houses.Count;
            List<GuildsHouse> guildsHouses = new List<GuildsHouse>();
            for (int i = 0; i < amountOfHouses; i++)
            {
                guildsHouses.Add(new GuildsHouse(houses[i].Faculty, houses[i].Id, houses[i].Adress,
                    @"http://cocktion.com/Content/SiteImages/house1.jpg"));
            }
            return guildsHouses;
        }
    
        /// <summary>
        /// Посылает на мобилку коллекцию с домами хаусхолдера
        /// </summary>
        /// <returns>Коллекцию</returns>
        [HttpPost]
        [Authorize]
        public List<GuildsHouse> GetGuildsHouses()
        {
            //TODO сделать нормальную версию, когда можно будет дома добавить
            List<GuildsHouse> houses = new List<GuildsHouse>();
            CocktionContext db = new CocktionContext();

            //обработка guildId из формы
            string guildId = HttpContext.Current.Request.Form.GetValues("guildId")[0];
            switch (guildId)
            {
                case ("0"):
                    houses = BuildHouseForMobile(db, "МГИМО");
                    break;
                case ("1"):
                    houses = BuildHouseForMobile(db, "МФТИ");
                    break;
                case ("2"):
                    houses = BuildHouseForMobile(db, "НИУ ВШЭ");
                    break;
                case ("3"):
                    houses = BuildHouseForMobile(db, "МГУ");
                    break;
                case ("4"):
                    houses = BuildHouseForMobile(db, "НИЯУ МИФИ");
                    break;
                case ("5"):
                    houses = BuildHouseForMobile(db, "МГТУ");
                    break;
            }
            return houses;
        }

        /// <summary>
        /// Посылает на мобилку информаию о доме
        /// </summary>
        /// <returns>Класс с инфой о доме</returns>
        [HttpPost]
        [Authorize]
        public HouseMobile GetHouse()
        {
            //TODO добавить везде проверки
            CocktionContext db = new CocktionContext();

            //обработка guildId из формы
            string houseId = HttpContext.Current.Request.Form.GetValues("houseId")[0];
            House house = db.Houses.Find(int.Parse(houseId));
            HouseMobile mobileHouse = new HouseMobile(@"http://cocktion.com/Content/SiteImages/house1.jpg", house.Likes,
                0, house.Rating, 10, house.Auctions.Count, "Крутой дом");

            return mobileHouse;
        }

        /// <summary>
        /// Шлет все посты с форума на мобилку
        /// </summary>
        /// <returns>Коллекцию с постами для мобилки</returns>
        [HttpPost]
        [Authorize]
        public List<ForumPostMobile> GetHouseForumPosts()
        {
            string houseId = HttpContext.Current.Request.Form.GetValues("houseId")[0];
            CocktionContext db = new CocktionContext();
            var forumPosts = db.Houses.Find(int.Parse(houseId)).Posts;
            List<ForumPostMobile> posts = new List<ForumPostMobile>(forumPosts.Count);
            foreach (var post in forumPosts)
            {
                posts.Add(new ForumPostMobile(post.AuthorName, post.Message, post.Likes));
            }

            return posts;
        }

        /// <summary>
        /// Добавляет пост на форум конкретного дома
        /// </summary>
        /// <returns>Статус удалось или нет</returns>
        [HttpPost]
        [Authorize]
        public StatusHolder SendPost()
        {
            try
            {
                CocktionContext db = new CocktionContext();
                
                //Получаем информацию для постав
                string houseId = HttpContext.Current.Request.Form.GetValues("houseId")[0];
                string message = HttpContext.Current.Request.Form.GetValues("message")[0];
                string authorName = User.Identity.Name;

                //Находим дом
                var house = db.Houses.Find(int.Parse(houseId));

                //добавляем пост
                house.Posts.Add(new ForumPost(message, authorName));
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

using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using WebGrease;
namespace CocktionMVC.Functions
{
    /// <summary>
    /// Содержит методы для работы с рейтингом у 
    /// 1) Аукционов
    /// 2) Домов 
    /// 3) Пользователей
    /// </summary>
    public class RatingManager
    {
        /// <summary>
        /// Увеличивает рейтинг аукциона
        /// 
        /// В строке reasonOfInc передается параметр, в 
        /// зав-ти от которого увеличивается рейтинг аукциона.
        /// Есть следующие варианты:
        /// </summary>
        /// <param name="auction">Аукцион, который надо увеличить</param>
        /// <param name="user">Инстанс пользователя</param>
        /// <param name="reasonOfInc">
        /// Параметр для увеличения
        /// 1) userBeted - если пользователь поставил ставку/создал аукцион -
        /// увеличиваем рейтинг аукциона на 0.4 * рейтинг пользователя
        ///        
        /// 2) newVisit - кто-то посмотрел аукцион - добавляем 20 очков
        /// </param>
        [Authorize]
        public static void IncreaseRating(Auction auction,  AspNetUser user, string reasonOfInc)
        {
            switch (reasonOfInc)
            {
                case "userBeted":
                    auction.Rating += (int) (user.Rating*0.4);
                    break;
                case "newVisit":
                    auction.Rating += 20;
                    break;
            }
        }


        /// <summary>
        /// Увеличивает рейтинг пользователя
        /// 
        /// В строке reasonOfInc подается причина увеличения рейтинга
        /// всего может быть три варианта:
        /// </summary>
        /// <param name="user">Пользователь, которому надо увеличить рейтинг</param>
        /// <param name="reasonOfInc">
        /// Строка, содержащая причину
        /// 1) userMadeAuction => добавляем 16 очков
        /// 2) userGotSubscriber => добавляем 16 очков
        /// 3) userPlacedBet => добаляем 8 очков
        /// </param>
        [Authorize]
        public static void IncreaseRating(AspNetUser user,  string reasonOfInc)
        {
            switch (reasonOfInc)
            {
                case "userMadeAuction":
                    user.Rating += 16;
                    break;
                case "userGotSubscriber":
                    user.Rating += 16;
                    break;
                case "userPlacedBet":
                    user.Rating += 8;
                    break;
            }
        }

        /// <summary>
        /// Увеличивает рейтинг дома
        /// </summary>
        /// <param name="house">Дом, у которого надо увеличить рейтинг</param>
        /// <param name="reasonOfInc">Причина увеличения рейтинга
        /// 1) auctionAdded => + 16
        /// 2) subscriberAdded => + 16
        /// 3) likeAdded => +8</param>
        [Authorize]
        public static void IncreaseRating(House house, string reasonOfInc)
        {
            switch (reasonOfInc)
            {
                case "auctionAdded":
                    house.Rating += 16;
                    break;
                case "subscriberAdded":
                    house.Rating += 16;
                    break;
                case "likeAdded":
                    house.Rating += 8;
                    break;
            }
        }
    }
}
using System;
using System.Linq;
using CocktionMVC.Models;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;

namespace CocktionMVC.Functions
{
    /// <summary>
    /// Контейнер для методов, связанных с окончанием тотализатора и 
    /// получением его результатов
    /// </summary>
    public class ToteResultsManager
    {
        /// <summary>
        /// Получает результаты тотализатора.
        /// Инициализирует необходимые поля в классе,
        /// который потом вернется джейсоном клиентам
        /// (Версия для проигравших аукцион)
        /// </summary>
        /// <param name="auction">Аукцион, на котором был тотализатор</param>
        /// <param name="userId">Айди пользователя</param>
        /// <param name="currentBidseller">Существующий объект BidSeller'a</param>
        /// <param name="currentUser">Текущий пользователь</param>
        /// <param name="winProductName">Название продукта - победителя аукциона</param>
        public static void GetToteResults(Auction auction, string userId, BidSeller currentBidseller,
            ApplicationUser currentUser, string winProductName)
        {
            //Выбираем результаты тотализатора этого пользователя
            var q = (from x in auction.AuctionToteBoard.ToteResultsForUsers
                     where (x.UserId == userId)
                     select x);

            if (q.Count() != 0)
            {//если он ставил яйца
                //считаем прибыль
                currentBidseller.ProfitFromTote = q.First().Profit;
                currentBidseller.Message = String.Format("Дорогой, аукцион закончен и победил товар {0}", winProductName) +
                             String.Format("Вы срубили бабла {0}", currentBidseller.ProfitFromTote) +
                             String.Format("У вас бабла тепрь {0}", currentUser.Eggs);
            }
            else
            {//если он не ставил яйца
                currentBidseller.Message = String.Format("Дорогой, аукцион закончен и победил товар {0}", winProductName);
            }
        }

        /// <summary>
        /// Получает результаты тотализатора.
        /// Инициализирует необходимые поля в классе,
        /// который потом вернется джейсоном клиентам
        /// (Версия для победителя аукциона)
        /// </summary>
        /// <param name="auction">Аукцион, на котором был тотализатор</param>
        /// <param name="userId">Айдишник пользователя, который победил</param>
        /// <param name="owner">объект BidSeller'a</param>
        /// <param name="phone">Телефон владельца аукциона</param>
        /// <param name="currentUser">Текущий пользователь</param>
        public static void GetToteResults(Auction auction, string userId, BidSeller owner, string phone,
            ApplicationUser currentUser)
        {
            //Выбираем результаты тотализатора этого пользователя
            var q = (from x in auction.AuctionToteBoard.ToteResultsForUsers
                     where (x.UserId == userId)
                     select x);

            if (q.Count() != 0)
            {//если он что-нибудь стваил
                //считаем прибыль
                owner.ProfitFromTote = q.First().Profit;
                owner.Message = "Аукцион закончен, вам необходимо связаться с продавцом! " + phone + "/n" +
                                String.Format("Вы срубили бабла {0} ", owner.ProfitFromTote) +
                                String.Format("У вас бабла тепрь {0}", currentUser.Eggs);
            }
            else
            {//если он ничего не ставил
                owner.Message = "Аукцион закончен, вам необходимо связаться с продавцом! " + phone;
            }
        }
    }
}
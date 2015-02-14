using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Класс, который реализует тотализатор.
    /// </summary>
    public class ToteBoard
    {
        /// <summary>
        /// Айдишник для базы данных.
        /// </summary>
        public int Id { get; set; }
 
        /// <summary>
        /// Общее количество яиц.
        /// </summary>
        public int TotalEggs { get; set; }

        /// <summary>
        /// Словарь для хранения информации о том,
        /// сколько яиц поставлено на конкретный товар.
        /// </summary>
        public Dictionary<string, int> EggsOnBidsAmount { get; set; } 

        /// <summary>
        /// Словарь для хранения информации о том,
        /// сколько яиц поставил конкретный пользователь.
        /// </summary>
        public Dictionary<string, int> UserEggsAmount { get; set; }

        /// <summary>
        /// Словарь для хранения информации о том, 
        /// какой пользователь поставил на какой товар.
        /// </summary>
        public Dictionary<string, int> UserBidsConnection { get; set; }

        /// <summary>
        /// Аукцион, для которого организован тотализатор.
        /// </summary>
        public virtual Auction ToteAuction { get; set; }

        /// <summary>
        /// Метод для подсчета количества яиц, поставленных на
        /// конкретный товар.
        /// </summary>
        /// <param name="productId">Айдишник продукта</param>
        /// <returns>Сумму яиц </returns>
        private int CountEggsOnProduct(string productId)
        {
            return 0;
        }

        /// <summary>
        /// Метод для подсчета коэффициента для конкретного продукта
        /// 
        /// Коэффициент = яица на продукте / общее количество яиц в тотале
        /// </summary>
        /// <param name="productId">Айдишник продукта</param>
        /// <param name="eggsOnProduct">Количество яиц, поставленных на продукт</param>
        /// <param name="totalAmountOfEggs">Общее количество яиц в аукциона</param>
        /// <returns></returns>
        public double CountCoefficientForProduct(string productId, int eggsOnProduct, 
                                                                    int totalAmountOfEggs)
        {
            return 0;
        }

        /// <summary>
        /// Метод для подсчета коэффициента для конкретного пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns></returns>
        private double CountCoefficientForUser(string userName)
        {
            return 0;
        }

        /// <summary>
        /// Метод для расчета прибыли для конкретного пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns></returns>
        private int CountProfitForUser(string userName)
        {
            return 0;
        }

        /// <summary>
        /// Метод для расчета количества яиц во всем
        /// тотализаторе
        /// </summary>
        /// <returns>Количесто яиц в тоталихаторе</returns>
        private int CountTotalAmountOfEggs()
        {
            return 0;
        }

    }
}
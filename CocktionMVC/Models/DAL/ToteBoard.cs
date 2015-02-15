using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Класс, который реализует тотализатор.
    /// </summary>
    public class ToteBoard
    {
        public ToteBoard()
        {
            this.IsActive = false;
            this.EggsOnProductsAmount = new Dictionary<string, int>();
            this.ProductCoefficients = new Dictionary<string, double>();
            this.UserEggsAmount = new Dictionary<string, int>();
            this.UserProductsConnection = new Dictionary<string, int>();
        }

        /// <summary>
        /// Айдишник для базы данных.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Индикатор активности тотализатора
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Общее количество яиц.
        /// </summary>
        public int TotalEggs { get; set; }

        /// <summary>
        /// Словарь для хранения информации о том,
        /// сколько яиц поставлено на конкретный товар.
        /// </summary>
        public Dictionary<string, int> EggsOnProductsAmount { get; set; } 

        /// <summary>
        /// Словарь для хранения информации о том,
        /// сколько яиц поставил конкретный пользователь.
        /// </summary>
        public Dictionary<string, int> UserEggsAmount { get; set; }

        /// <summary>
        /// Словарь для хранения информации о том, 
        /// какой пользователь поставил на какой товар.
        /// </summary>
        public Dictionary<string, int> UserProductsConnection { get; set; }

        /// <summary>
        /// Коеффициенты на каждом товаре
        /// </summary>
        public Dictionary<string, double> ProductCoefficients { get; set; }

        /// <summary>
        /// Аукцион, для которого организован тотализатор.
        /// </summary>
        public virtual Auction ToteAuction { get; set; }


        /// <summary>
        /// Метод для подсчета коэффициента для конкретного продукта
        /// 
        /// Коэффициент = яица на продукте / общее количество яиц в тотале
        /// </summary>
        /// <param name="productId">Айдишник продукта</param>
        /// <param name="eggsOnProduct">Количество яиц, поставленных на продукт</param>
        /// <param name="totalAmountOfEggs">Общее количество яиц в аукциона</param>
        /// <returns></returns>
        public double CountCoefficientForProduct(string productId)
        {
            return this.EggsOnProductsAmount[productId] / this.TotalEggs;
        }

        /// <summary>
        /// Метод для подсчета коэффициента для конкретного пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns></returns>
        private double CountCoefficientForUser(string userId)
        {
            int eggsOnProduct = this.EggsOnProductsAmount[this.UserProductsConnection[userId].ToString()];
            return this.UserEggsAmount[userId] / eggsOnProduct;
        }

        /// <summary>
        /// Метод для расчета прибыли для конкретного пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns></returns>
        private int CountProfitForUser(string userId)
        {
            return (int) CountCoefficientForUser(userId) * this.TotalEggs;
        }

        /// <summary>
        /// Метод для расчета количества яиц во всем
        /// тотализаторе
        /// </summary>
        /// <returns>Количесто яиц в тоталихаторе</returns>
        private void CountTotalAmountOfEggs()
        {
            int sum = 0;
            foreach (var i in this.EggsOnProductsAmount)
            {
                sum += i.Value;
            }
            this.TotalEggs = sum;
        }

        /// <summary>
        /// Считает и записывает коеффициенты для всех товаров
        /// </summary>
        private void CountAllCoefficientsForProducts()
        {
            foreach(var i in this.EggsOnProductsAmount)
            {
                this.ProductCoefficients.Add(i.Key, i.Value / this.TotalEggs);
            }
        }

        /// <summary>
        /// Добавляет ставку пользователя
        /// </summary>
        /// <param name="userId">Айди пользователя</param>
        /// <param name="eggsAmount">Количество яиц</param>
        /// <param name="productId">Айдишник товара</param>
        public async Task<bool> SetRateForUser(string userId, int eggsAmount, int productId, CocktionContext db)
        {
            //добавить проверку на хватит ли у пользователя яиц
            AspNetUser user = db.AspNetUsers.Find(userId);

            if (IsEnoughEggs(user.Eggs, eggsAmount))
            {
                //уменьшить количество яиц у пользовател
                user.Eggs -= eggsAmount;

                //ЗАПИСЬ ВСЕХ ДАННЫХ В СЛОВАРИ
                //запись данных в словарь пользователь - яйца
                AddInfoToDict(this.UserEggsAmount, userId, eggsAmount, "Insert");

                //запись данных в словарь Продукт - количество яиц
                AddInfoToDict(this.EggsOnProductsAmount, productId.ToString(), eggsAmount, "AddToSum");

                //запись данных в словарь пользователь - продукт
                AddInfoToDict(this.UserProductsConnection, userId, productId, "Insert");

                //ПЕРЕСЧИТАТЬ ВСЕ ДАННЫЕ ПО ТОТАЛИЗАТОРУ
                CountTotalAmountOfEggs(); //количество яиц в тотализаторе
                CountAllCoefficientsForProducts(); //коэффициенты для всех товаров
                await Functions.DbItemsAdder.SaveDb(db);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Метод для проверки достаточности яиц на счету у пользователя
        /// </summary>
        /// <param name="userEggs">Количество яиц у пользователя на балансе</param>
        /// <param name="rateEggs">Количество яиц, которые он хочет поставить</param>
        /// <returns>true, если достаточно, false иначе</returns>
        private bool IsEnoughEggs(int userEggs, int rateEggs)
        {
            if (userEggs >= rateEggs)
                return true;
            else return false;
        }

        public void FinishTote(int winProductId)
        {

        }

        /// <summary>
        /// Метод для добавления информации в словарь с верификацией.
        /// В частности, метод написан с целью добавления денег к словарям.
        /// Если typeOfOperation == "Insert", то просто добавляется
        /// значение в словарь по соответствующему ключу.
        /// Если typeOfOperation == "AddToSum", то суммирются значения в
        /// соответствующей ячейке по ключу и значение, которое передано 
        /// в параметире.
        /// </summary>
        /// <param name="dict">Словарь, с которым проводятся операции</param>
        /// <param name="index">Ключ к ячейке, с которой надо проводить операции</param>
        /// <param name="value">Значение, которое необходимо добавлять в словарь</param>
        /// <param name="typeOfOperation">Тип операции вставки</param>
        private void AddInfoToDict(Dictionary<string, int> dict ,string index, int value, string typeOfOperation)
        {
            if (dict.ContainsKey(index))
            {//если в словаре есть искомый индекс
                switch(typeOfOperation)
                {
                    case "Insert": //если нужно просто вставить значение в сущ-ий ключ
                        dict[index] = value;
                        break;
                    case "AddToSum": //если нужно увеличить количество денег в нужном ключе
                        dict[index] += value;
                        break;
                }
            }
            else
            {//если в словаре нет искомого индекса
                dict.Add(index, value);
            }
        }
    }
}
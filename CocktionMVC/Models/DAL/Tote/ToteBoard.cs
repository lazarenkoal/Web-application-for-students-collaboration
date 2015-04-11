﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktionMVC.Functions;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Класс, который реализует тотализатор.
    /// </summary>
    public class ToteBoard
    {
        public ToteBoard()
        {
            IsActive = false;
            Bids = new HashSet<ToteEntity>();
            ToteResultsForUsers = new HashSet<ToteResult>();
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
        /// Аукцион, для которого организован тотализатор.
        /// </summary>
        public virtual Auction ToteAuction { get; set; }

        /// <summary>
        /// результаты тотализатора с точки зрения пользователй
        /// </summary>
        public virtual ICollection<ToteResult> ToteResultsForUsers { get; set; }
        public virtual ICollection<ToteEntity> Bids { get; set; }

        /// <summary>
        /// Метод для подсчета коэффициента для конкретного продукта
        /// 
        /// Коэффициент = яица на продукте / общее количество яиц в тотале
        /// </summary>
        /// <param name="productId">Айдишник продукта</param>
        /// <returns>Коэффициент на аукционе для продукта</returns>
        public double CountCoefficientForProduct(int productId)
        {
            var eggsCollection = (from x in Bids
                        where x.ProductId == productId
                        select x);
            double coefficient = 0;
            foreach (var egg in eggsCollection)
            {
                coefficient += egg.EggsAmount;
            }
            coefficient /= this.TotalEggs;
            return coefficient;
        }

        /// <summary>
        /// Метод для подсчета коэффициента для конкретного пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Коэффициент пользователя</returns>
        private double CountCoefficientForUser(string userId)
        {
            
            //Ставку можно делать только один раз    
            //выбираем все ставки данного пользователя          
            var eggsCollection = (from x in Bids
                                  where x.UserId == userId
                                  select x);

            double coefficient = 0; //вводим коэффициент
            
            //суммируем все его ставки (она должна быть одна, но не важно)
            foreach (var i in eggsCollection)
                coefficient += i.EggsAmount;

            //выбираем все ставки, которые поставили на данный продукт
            var eggsOnProductCollection = (from x in Bids
                                           where x.ProductId == eggsCollection.First().ProductId
                                           select x);
            //суммируем количество яиц, поставленных на данный продукт
            double eggsOnProduct = 0;
            foreach (var i in eggsOnProductCollection)
                eggsOnProduct += i.EggsAmount;

            //рассчитываем коэффициент
            coefficient /= eggsOnProduct;
            return coefficient;
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
            foreach (var i in Bids)
            {
                sum += i.EggsAmount;
            }
            this.TotalEggs = sum;
        }

        /// <summary>
        /// Формирует cловарь, содержащий имя каждого продукта
        /// и расчет коэффициента для него.
        /// </summary>
        private Dictionary<string, double> CountAllCoefficientsForProducts()
        {
            Dictionary<string, double> data = new Dictionary<string, double>();
            foreach (var i in ToteAuction.BidProducts)
            {
                data.Add(i.Name, CountCoefficientForProduct(i.Id));
            }
            return data;
        }

        /// <summary>
        /// Добавляет ставку пользователя
        /// </summary>
        /// <param name="userId">Айди пользователя</param>
        /// <param name="eggsAmount">Количество яиц</param>
        /// <param name="productId">Айдишник товара</param>
        public async Task<bool> SetRateForUser(int auctionId, AspNetUser user, int eggsAmount, int productId, CocktionContext db)
        {
            if (IsEnoughEggs(user.Eggs, eggsAmount))
            {
                Bids.Add(new ToteEntity
                {
                    UserId = user.Id,
                    EggsAmount = eggsAmount,
                    ProductId = productId
                });
                user.Eggs -= eggsAmount;
                CountTotalAmountOfEggs();
                AuctionHub.UpdateToteBoard(auctionId, CountAllCoefficientsForProducts());
                await DbItemsAdder.SaveDb(db);
                return true;
            }
            AuctionHub.UpdateToteBoard(auctionId, CountAllCoefficientsForProducts());
            return false;
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
            return false;
        }

        /// <summary>
        /// Метод, завершающий тотализатор
        /// </summary>
        /// <param name="winProductId">Айдишник продукта - победителя</param>
        public async Task FinishTote(int winProductId, CocktionContext db)
        {
            //Рассчитать для каждого из пользователей результат аукциона.
            foreach (var i in Bids)
            {
                SetResult(winProductId, i, db);
            }

            await DbItemsAdder.SaveDb(db);
        }

        /// <summary>
        ///  Метод устанавливает результат тотализатора
        ///  для каждого пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="winProductId"></param>
        private void SetResult(int winProductId, ToteEntity bid, CocktionContext db)
        {
            //Проверка на успешность
            ToteResult result;
            if (bid.ProductId == winProductId)
            {//если чувак поставил на победителя
                int profit = CountProfitForUser(bid.UserId);
                db.AspNetUsers.Find(bid.UserId).Eggs += profit;
                result = new ToteResult
                {
                    UserId = bid.UserId,
                    IsSucсessful = true,
                    Profit = profit //считаем прибыль, потому что чел успешен
                };
            }
            else
            {//если чувак неправильно поставил
                result = new ToteResult
                {
                    UserId = bid.UserId,
                    IsSucсessful = false,
                    Profit = -bid.EggsAmount //просто добавляем ему отрицательную прибыль!
                };
            }
            //записать все это дело в таблицу с результами
            this.ToteResultsForUsers.Add(result);
        }
       
    }
}
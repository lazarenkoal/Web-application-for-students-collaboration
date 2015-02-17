using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CocktionMVC.Models.Hubs;
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
        public async Task<bool> SetRateForUser(int auctionId, string userId, int eggsAmount, int productId, CocktionContext db)
        {
            //добавить проверку на хватит ли у пользователя яиц
            AspNetUser user = db.AspNetUsers.Find(userId);

            if (IsEnoughEggs(user.Eggs, eggsAmount))
            {
                Bids.Add(new ToteEntity
                {
                    UserId = userId,
                    EggsAmount = eggsAmount,
                    ProductId = productId
                });
                user.Eggs -= eggsAmount;
                CountTotalAmountOfEggs();
                AuctionHub.UpdateToteBoard(auctionId, CountAllCoefficientsForProducts());
                db.SaveChanges();
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

       
    }
}
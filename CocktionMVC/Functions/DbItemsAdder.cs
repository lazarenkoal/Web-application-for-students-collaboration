using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using CocktionMVC.Models.DAL;
namespace CocktionMVC.Functions
{
    /// <summary>
    /// Методные контейнер, для хранения солдат,
    /// работающих с базой данных в асинхроне
    /// </summary>
    public static class DbItemsAdder
    {
        /// <summary>
        /// Добавляет product и photo в базу данных и сохраняет
        /// изменения
        /// </summary>
        /// <param name="db">База, в которую надо добавить изменения</param>
        /// <param name="product">Сущность продукта, которую надо добавить</param>
        /// <param name="photo">Фото, которое надо добваить</param>
        /// <returns></returns>
        public static async Task AddProduct(CocktionContext db, Product product, Photo photo)
        {
            db.Products.Add(product);
            db.Photos.Add(photo);
            await Task.Run(() => db.SaveChangesAsync());
        }

        /// <summary>
        /// Просто асинхронно сохраняет изменения в базе данных
        /// </summary>
        /// <param name="db">база, в которой нужно все поменять</param>
        /// <returns>г</returns>
        public static async Task SaveDb(CocktionContext db)
        {
            await Task.Run(() => db.SaveChangesAsync());
        }

        /// <summary>
        /// Деактивирует аукцион и асинхронно
        /// выполняет сохранение данных в базу
        /// </summary>
        /// <param name="db">База, в которую надо сохранить изменения</param>
        /// <param name="auctionId">Айди аукциона</param>
        /// <returns></returns>
        public static async Task FalseAuctionStatus(CocktionContext db, int auctionId)
        {
            Auction auction = db.Auctions.Find(auctionId);
            auction.IsActive = false;
            await Task.Run(() => db.SaveChangesAsync());
        }
       
    }
}
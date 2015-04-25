﻿using System.Threading.Tasks;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.Hubs;

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
        /// <param name ="cluster">Хранилище ставок, в которое надо вставить данный продукт</param>
        /// <returns></returns>
        public static async Task AddProduct(CocktionContext db, Product product, Picture photo, BidCluster cluster)
        {
            db.Products.Add(product);
            db.Pictures.Add(photo);
            db.AuctionBids.Add(cluster);
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
        /// Асинхронно добавляет аукцион,
        /// товар, фотку в базу
        /// </summary>
        /// <param name="db">База, в которую необходимо добавить информацию</param>
        /// <param name="auction">Аукцион, который необходимо добавить</param>
        /// <param name="product">Товар, который необходимо добавить</param>
        /// <param name="photo">фотка, которую надо добавить</param>
        /// <param name="user"></param>
        public static async Task AddAuctionProductPhotoAsync(CocktionContext db, Auction auction,
            Product product, Picture photo, AspNetUser user)
        {
            db.Auctions.Add(auction);
            db.Products.Add(product);
            db.Pictures.Add(photo);
            user.HisAuctions.Add(auction);
            user.HisProducts.Add(product);
            await Task.Run(() => db.SaveChangesAsync());
        }
       
    }
}
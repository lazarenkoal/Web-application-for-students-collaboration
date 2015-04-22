using Microsoft.AspNet.SignalR;
using System;

namespace CocktionMVC.Models.Hubs
{
    public class AuctionListHub : Hub
    {
        /// <summary>
        /// Метод обновляет страничку с аукционами при
        /// добавлении нового аукциона на страницу.
        /// </summary>
        /// <param name="name">Имя товара для аукциона</param>
        /// <param name="description">Описание товара для аукциона</param>
        /// <param name="category">Категория товара для аукциона</param>
        /// <param name="photoName">Имя фотки</param>
        public static void UpdateList(string name, string description, string category, DateTime time, 
            string photoName, int id)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionListHub>();
            context.Clients.All.addAuctionToTheList(name, description, category,time.ToLongTimeString(), 
                photoName, id);
        }
    }
}
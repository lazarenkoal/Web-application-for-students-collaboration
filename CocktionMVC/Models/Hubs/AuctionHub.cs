﻿using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
namespace CocktionMVC
{
    
    /// <summary>
    /// Хаб используется для работы со страницой 
    /// CurrentAuction. Он посылает сообщения, 
    /// проводит транзакции с окончанием аукциона
    /// и т.д.
    /// </summary>
    public class AuctionHub : Hub
    {
        /// <summary>
        /// Посылает сообщения всем клиентам в данной
        /// группе
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="auctionId">Id аукциона</param>
        public void Send(string name, string message, int auctionId)
        {
            Clients.Group(auctionId.ToString()).addNewMessageToPage(name, message);
        }

        /// <summary>
        /// Асинхронно добавляет новую группу
        /// на сервере
        /// </summary>
        /// <param name="auctionId">Айди аукциона</param>
        public void AddNewRoom(int auctionId)
        {
            Groups.Add(Context.ConnectionId, auctionId.ToString());
        }

        /// <summary>
        /// Добавляет товары в диаграмму на всех клиентах, где
        /// открыта страничка
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="fileName">Название файла с фотографией</param>
        /// <param name="auctionId">Айди аукциона</param>
        public void AddNodesToClients(string name, string fileName, int auctionId, int productId)
        {
            //Добавляем клиентам всей группы Нодики
            Clients.Group(auctionId.ToString()).addNodesToPages(fileName, name, productId);
        }

        /// <summary>
        /// Метод посылает айдишник лидера на все клиенты
        /// </summary>
        /// <param name="leaderId">Айдишник лидера</param>
        /// <param name="auctionId">Айдишник аукциона, на котором все это происходит</param>
        public void SetLider(string leaderId, int auctionId)
        {
            Clients.Group(auctionId.ToString()).showLeaderOnPage(leaderId);
        }



        /// <summary>
        /// Метод заканчивает аукцион
        /// </summary>
        /// <param name="auctionId">Айди аукциона, который нужно закончить</param>
        public async Task FinishAuction(int auctionId)
        {
            //заканчивать здесь аукцион
            CocktionContext db = new CocktionContext();
            await DbItemsAdder.FalseAuctionStatus(db, auctionId);
            Clients.Group(auctionId.ToString()).finishAuction();

        }
        
    }
}
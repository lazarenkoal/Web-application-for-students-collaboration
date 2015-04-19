using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.SignalR;

namespace CocktionMVC.Models.Hubs
{
    public class MessageHub : Hub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="author">Автор сообщения</param>
        /// <param name="userName">Имя пользователя, которому надо отправить сообшение</param>
        /// <param name="authorId">Айдишник того, кто отправляет</param>
        /// <param name="receiverId">Айдишник того, кому отправляют</param>
        /// <returns></returns>
        public async Task Send(string message, string author, string userName,
            string authorId, string receiverId)
        {
            CocktionContext db = new CocktionContext();
            var userA = db.AspNetUsers.Find(authorId);
            var userB = db.AspNetUsers.Find(receiverId);
            PrivateMessage privateMessage = new PrivateMessage(message, userA.UserName, userB.UserName,
                DateTime.Now);
            //userA.ChatMessages.Add(privateMessage);
            //userB.ChatMessages.Add(privateMessage);
            db.Messages.Add(privateMessage);
            await DbItemsAdder.SaveDb(db);
            Clients.Group(userName).addNewMessageToPage(author, message, receiverId);
        }

        /// <summary>
        /// Асинхронно добавляет новую группу
        /// на сервере
        /// </summary>
        /// <param name="auctionId">Айди аукциона</param>
        public void AddNewRoom(string userName)
        {
            Groups.Add(Context.ConnectionId, userName);
        }
    }
}
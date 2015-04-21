﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Results;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

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
            Clients.Group(userName).addNewMessageToPage(author, message,DateTime.Now.ToShortDateString() ,receiverId);
            CocktionContext db = new CocktionContext();
            var userA = db.AspNetUsers.Find(authorId);
            var userB = db.AspNetUsers.Find(receiverId);
            PrivateMessage privateMessage = new PrivateMessage(message, userA.UserName, userB.UserName,
                DateTime.Now);
            //userA.ChatMessages.Add(privateMessage);
            //userB.ChatMessages.Add(privateMessage);
            db.Messages.Add(privateMessage);
            await DbItemsAdder.SaveDb(db);
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

        public void GetListOfReceivers(string userName)
        {
            CocktionContext db = new CocktionContext();
            var msg = (from x in db.Messages
                where x.AuthorName == userName || x.ReceiverName == userName
                select x.AuthorName == userName ? x.ReceiverName : x.AuthorName).ToList();        
            List<string> authors = new List<string>();
            foreach (var message in msg)
            {
                if (!authors.Contains(message) && message != userName)
                    authors.Add(message);
            }
            int i = 0;
            authors.ForEach(x => Clients.Group(userName).addAuthors(x));

        }

        public void GetMessages(string userName, string thisUserName)
        {
            CocktionContext db = new CocktionContext();

            var msg = (from x in db.Messages
                       where (x.AuthorName == thisUserName && x.ReceiverName == userName) ||
                       (x.AuthorName == userName && x.ReceiverName == thisUserName)
                       select x).ToList();

            var userToSend = (from x in db.AspNetUsers
                where x.UserName == userName
                select x).First();
            foreach (var message in msg)
            {
               // Clients.Group(userName).addNewMessageToPage(author, message, receiverId);
                Clients.Group(thisUserName).appendMessageToPage(message.AuthorName, message.Content, message.DateOfPublishing.ToShortDateString(),
                    userToSend.Id);
            }
        }
    }
}
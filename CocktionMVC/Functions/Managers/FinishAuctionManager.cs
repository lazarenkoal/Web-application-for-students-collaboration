using CocktionMVC.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CocktionMVC.Functions.Managers
{
    public class FinishAuctionManager
    {
        /// <summary>
        /// Рассылает сообщения владельцу и победителю аукциона
        /// </summary>
        /// <param name="auction"></param>
        private static void AddAndSendMsgToOwnerAndWinner(Auction auction)
        {
            //сообщение владельцу
            string messageToOwner = "Привет, я - тот, кого ты выбрал!;) (мгновенная связь с победителем)";
            PrivateMessage toOwner = new PrivateMessage(messageToOwner,
                auction.LeadProduct.Owner.UserName, auction.Owner.UserName, DateTimeManager.GetCurrentTime());
            auction.LeadProduct.Owner.ChatMessages.Add(toOwner);
            auction.Owner.ChatMessages.Add(toOwner);
            if (auction.Owner.MobileDevice != null)
                Notificator.SendNotification(auction.Owner.MobileDevice, messageToOwner, 5);


            //сообщение победителю
            string messageToWinner = "Привет! Я - владелец! (мгновенная связь с владельцем)";
            PrivateMessage toWinner = new PrivateMessage("Привет! Я - владелец!",
                auction.Owner.UserName, auction.LeadProduct.Owner.UserName, DateTimeManager.GetCurrentTime());
            auction.LeadProduct.Owner.ChatMessages.Add(toWinner);
            auction.Owner.ChatMessages.Add(toWinner);
            if (auction.LeadProduct.Owner.MobileDevice != null)
                Notificator.SendNotification(auction.LeadProduct.Owner.MobileDevice, messageToWinner, 5);
        }

        /// <summary>
        /// Деактивирует аукцион и асинхронно
        /// выполняет сохранение данных в базу
        /// </summary>
        /// <param name="db">База, в которую надо сохранить изменения</param>
        /// <param name="auction">Аукцион, который необходимо завершить</param>
        public static void FalseAuctionStatus(CocktionContext db, Auction auction)
        {

            //заканчиваем аукцион
            auction.IsActive = false;

            //вырубаем тотализатор
            auction.AuctionToteBoard.FinishTote(auction.LeadProduct.Id, db);

            //отправляем сообщение владельцу и победителю
            AddAndSendMsgToOwnerAndWinner(auction);
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
            //заканчиваем аукцион
            Auction auction = db.Auctions.Find(auctionId);
            auction.IsActive = false;

            //отправляем сообщения
            AddAndSendMsgToOwnerAndWinner(auction);

            //вырубаем тотализатор
            auction.AuctionToteBoard.FinishTote(auction.LeadProduct.Id, db);

            //сохраняем изменения в базу данных
            await Task.Run(() => db.SaveChangesAsync());
        }

    }
}
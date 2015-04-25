using CocktionMVC.Functions.Managers;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace CocktionMVC.Functions
{
    public static class AuctionChecker
    {
        /// <summary>
        /// Запускает таймер для проверок аукйионов
        /// </summary>
        public static void StartChecking()
        {
            Timer aTimer = new Timer(3600000);
            aTimer.Elapsed += CheckAuctions;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            //1 сек = 1000 млс
            //1 min = 60 000
            //1 hour = 3 600 000
        }

        /// <summary>
        /// Проверяет аукционы на то, закончились они или нет
        /// </summary>
        /// <param name="obj"></param>
        public static void CheckAuctions(Object source, ElapsedEventArgs e)
        {
            CocktionContext db = new CocktionContext();
            DateTime controllTime = DateTimeManager.GetCurrentTime();
            var acol = db.Auctions.ToList();
            foreach(Auction auction in acol)
            {
                //Если он активен, а время закончилось - завершаем аукцион, посылаем хозяину имейликкк
                if (auction.IsActive && controllTime >= auction.EndTime)
                {
                    if (!auction.WinnerChosen)
                    {
                        //MessageHub.Send("Ваш аукцион закончен! Необходимо выбрать лидера!", "Кокшн",
                          //  auction.Owner.UserName, null, auction.Owner.Id);
                        PrivateMessage message = new PrivateMessage("Ваш аукцион закончен и необходимо выбрать лидера",
                            "Коки", auction.Owner.UserName, DateTimeManager.GetCurrentTime());
                        auction.Owner.ChatMessages.Add(message);
                    }
                    else
                    {
                        FinishAuctionManager.FalseAuctionStatus(db, auction);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}
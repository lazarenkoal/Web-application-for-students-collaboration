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
            Timer aTimer = new Timer(18000000);
            aTimer.Elapsed += CheckAuctions;
            // Hook up the Elapsed event for the timer. 
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            //1 час = 3 600 000 млсек
            //3 min = 180 000 млск
			
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
                    }
                    else
                    {
                        DbItemsAdder.FalseAuctionStatus(db, auction);
                    }
                }
            }
            //EmailSender.SendEmail("xyi", "auctionInfo", "lazarenko.ale@gmail.com");
        }
    }
}
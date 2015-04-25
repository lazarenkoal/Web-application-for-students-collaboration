using CocktionMVC.Models.DAL;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CocktionMVC.Functions
{
    public class Notificator
    {
        /// <summary>
        /// Брокер для служб пуш-нотификаций
        /// </summary>
        public static PushBroker Pusher { get; set;}


        /// <summary>
        /// Регистрирует службу для нотификаций эппл
        /// </summary>
        public static void RegisterAppleService()
        {
            try
            {
                Pusher = new PushBroker();
                WebClient client = new WebClient();
                var appleCert = client.DownloadData(@"http://cocktion.com/Content/Cocktion.Push.Development.p12");
                client.Dispose();
                Pusher.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, "hateMicrosoftlove$"));
            }
            catch (Exception e)
            {
                EmailSender.SendEmail(e.Message, "auctionInfo", "lazarenko.ale@gmail.com");
            }

        }
        

        /// <summary>
        /// Посылает пуш на устройство
        /// </summary>
        /// <param name="device">Устройство, на которое надо послать</param>
        /// <param name="message">Сообщение в пуше, которое надо отобразить</param>
        /// <param name="badge">Бэдж, который надо показать</param>
        public static void SendNotification(Device device, string message, int badge)
        {
            if (Pusher == null)
                RegisterAppleService();

            switch(device.Type)
            {
                case "ios":
                    Pusher.QueueNotification(new AppleNotification()
                .ForDeviceToken(device.Token)
                .WithAlert(message)
                .WithBadge(badge)
                .WithSound("sound.caf"));
                    break;
                case "wp":
                    //do smth here
                    break;
                case "android":
                    //do smth here
                    break;
            }
        }
    }
}
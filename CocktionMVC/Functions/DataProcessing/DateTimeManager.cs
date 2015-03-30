using System;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Functions
{
    /// <summary>
    /// Контейнер для функции, отвечающих за работу со временем
    /// </summary>
    public static class DateTimeManager
    {
        /// <summary>
        /// Конвертирует время в UTC во время в текущем часовом поясе
        /// (Пока что только Москва)
        /// </summary>
        /// <returns>Время в данный момент, в часовом поясе Москвы</returns>
        public static DateTime GetCurrentTime()
        {
            DateTime controlTime = DateTime.Now;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            controlTime = TimeZoneInfo.ConvertTime(controlTime, TimeZoneInfo.Local, tst);
            return controlTime;
        }

        /// <summary>
        /// Устанавливает время окончания аукциона и время
        /// его начала
        /// </summary>
        /// <param name="auction">Аукцион, у которого надо все установить</param>
        /// <param name="hoursString">Длительность в часах</param>
        /// <param name="minutesString">Длительность в минутах</param>
        public static void SetAuctionStartAndEndTime(Auction auction, string hoursString, string minutesString)
        {
            //получаем текущее время и устанавливаем время старта
            DateTime auctionsEndTime = GetCurrentTime();
            DateTime auctionStartTime = auctionsEndTime;

            //преобразование времени из строк в числа
            int minutes = int.Parse(minutesString);
            int hours = int.Parse(hoursString);

            //вычисляем время окончания аукциона
            auctionsEndTime = auctionsEndTime.AddHours(hours);
            auctionsEndTime = auctionsEndTime.AddMinutes(minutes);

            //устанавливаем время окончания и начала аукциона
            auction.EndTime = auctionsEndTime;
            auction.StartTime = auctionStartTime;
        }
    }
}
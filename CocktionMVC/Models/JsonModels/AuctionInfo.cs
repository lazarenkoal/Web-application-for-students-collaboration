using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для модели аукциона, которая будет отсылаться
    /// мобильным клиентам.
    /// </summary>
    public class AuctionInfo
    {
        /// <summary>
        /// Описание аукциона
        /// </summary>
        public string AuctionDescription { get; set; }

        /// <summary>
        /// Название аукциона
        /// </summary>
        public string AuctionTitle { get; set; }

        /// <summary>
        /// Путь к картинке для аукциона
        /// </summary>
        public string AuctionImage { get; set; }

        /// <summary>
        /// Категория, к которой относится товар на аукционе
        /// </summary>
        public string AuctionCategory { get; set; }

        /// <summary>
        /// Время начала аукциона
        /// </summary>
        public DateTime AuctionStartTime { get; set; }

        /// <summary>
        /// Время окончания аукциона
        /// </summary>
        public DateTime AuctionEndTime { get; set; }

        /// <summary>
        /// Айдишник аукциона
        /// </summary>
        public int AuctionId { get; set; }
    }
}
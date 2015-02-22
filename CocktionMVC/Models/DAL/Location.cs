using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Класс, олицетворяюший сущность местоположения на карте.
    /// Сделано так, чтобы можно было разу ставить точки по координатам
    /// </summary>
    public class Location
    {
        public Location()
        {
            Auctions = new HashSet<Auction>();
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// Айдишник для базы данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Широта (идет первая в координатах для карт)
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота (идет вторая в координатах для карт)
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Контент иконки (это подпись на баллуне)
        /// </summary>
        public string IconContent { get; set; }

        /// <summary>
        /// Контент баллуна (это надпись, которая раскрывается при клике)
        /// </summary>
        public string BaloonContent { get; set; }

        /// <summary>
        /// Опция для баллуна, если указать
        /// preset: twirl#blackStretchyIcon
        /// будет растягиваться под контент.
        /// Надо будет ставить для каждого вуза свой цвет.
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// Университет, к которому привязана локация.
        /// Конвенция: все указываем одинаково. 
        /// ВШЭ/МГУ/МГТУ/МФТИ/РГТУ
        /// </summary>
        public string University { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
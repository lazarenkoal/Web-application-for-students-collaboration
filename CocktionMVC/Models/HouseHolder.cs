using System.Collections.Generic;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Models
{
    /// <summary>
    /// Модель для отображения хаус холдера 
    /// 
    /// Хаус холдер - контейнер для домов
    /// </summary>
    public class HouseHolder
    {
        public HouseHolder()
        {
            Houses = new HashSet<House>();
            Users = new HashSet<AspNetUser>();
        }

        public HouseHolder(string name, string city, Picture photo)
        {
            Houses = new HashSet<House>();
            Users = new HashSet<AspNetUser>();
            Name = name;
            PhotoCard = photo;
            City = city;
        }

        /// <summary>
        /// Айдишник для базы данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название холдера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Город, в котором нахоидтся холдер
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Дома, которые находятся в этом хаусхолдере
        /// </summary>
        public virtual ICollection<House> Houses { get; set; }

        /// <summary>
        /// Пользователи в хаус холдере
        /// </summary>
        public virtual ICollection<AspNetUser> Users { get; set; }

        /// <summary>
        /// Аваторчка холдера
        /// </summary>
        public virtual Picture PhotoCard { get; set; }
    }
}
﻿namespace CocktionMVC.Models.JsonModels.HouseRelatedModels
{
    /// <summary>
    /// Контейнер с информацией о доме для мобильного клиента
    /// </summary>
    public class GuildsHouse
    {
        public GuildsHouse() { }

        public GuildsHouse(string houseName, int houseId, string houseAdress, string housePhotoPath)
        {
            name = houseName;
            id = houseId;
            adress = houseAdress;
            photoPath = housePhotoPath;
        }
        /// <summary>
        /// Имя дома (факультет/общага)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Айдишник дома
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string adress { get; set; }

        /// <summary>
        /// Путь к фотке дома
        /// </summary>
        public string photoPath { get; set; }
    }
}
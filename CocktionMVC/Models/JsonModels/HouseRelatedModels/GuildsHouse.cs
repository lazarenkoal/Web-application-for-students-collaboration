using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels.HouseRelatedModels
{
    /// <summary>
    /// Контейнер с информацией о доме для мобильного клиента
    /// </summary>
    public class GuildsHouse
    {
        public GuildsHouse() { }

        public GuildsHouse(string houseName, int houseId, string houseAdress, string housePhotoPath)
        {
            HouseName = houseName;
            HouseId = houseId;
            HouseAdress = houseAdress;
            HousePhotoPath = housePhotoPath;
        }
        /// <summary>
        /// Имя дома (факультет/общага)
        /// </summary>
        public string HouseName { get; set; }

        /// <summary>
        /// Айдишник дома
        /// </summary>
        public int HouseId { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string HouseAdress { get; set; }

        /// <summary>
        /// Путь к фотке дома
        /// </summary>
        public string HousePhotoPath { get; set; }
    }
}
namespace CocktionMVC.Models.JsonModels.HouseRelatedModels
{
    /// <summary>
    /// Контейнер для данных о доме для посылки на мобилку
    /// </summary>
    public class HouseMobile
    {
        public HouseMobile() { }

        /// <summary>
        /// Конструирует инфу о доме по всем полям
        /// </summary>
        /// <param name="housePhotoPath">Путь к фотке для дома</param>
        /// <param name="likes">Лайки дома</param>
        /// <param name="dislikes">Дизлайки дома</param>
        /// <param name="rating">Рэйтинг дома</param>
        /// <param name="peopleInHouse">Люди в доме</param>
        /// <param name="auctionsInHouse">Аукционов в доме</param>
        /// <param name="houseDescription">Описание дома</param>
        public HouseMobile(string housePhotoPath, int likes, int dislikes, int rating, int peopleInHouse,
            int auctionsInHouse, string houseDescription)
        {
            HousePhotoPath = housePhotoPath;
            HouseLikes = likes;
            HouseDislikes = dislikes;
            HouseRating = rating;
            HousePeopleAmount = peopleInHouse;
            HouseAuctionsAmount = auctionsInHouse;
            HouseDescription = houseDescription;
        }

        /// <summary>
        /// Путь к фоточке
        /// </summary>
        public string HousePhotoPath { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int HouseLikes { get; set; }

        /// <summary>
        /// Количество дизлайков
        /// </summary>
        public int HouseDislikes { get; set; }

        /// <summary>
        /// Рэйтинг дома
        /// </summary>
        public int HouseRating { get; set; }

        /// <summary>
        /// Количество человек в доме
        /// </summary>
        public int HousePeopleAmount { get; set; }

        /// <summary>
        /// Количество аукционов в доме
        /// </summary>
        public int HouseAuctionsAmount { get; set; }

        /// <summary>
        /// Описание дома
        /// </summary>
        public string HouseDescription { get; set; }
    }
}
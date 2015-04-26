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
        /// <param name="photoPath">Путь к фотке для дома</param>
        /// <param name="likes">Лайки дома</param>
        /// <param name="dislikes">Дизлайки дома</param>
        /// <param name="rating">Рэйтинг дома</param>
        /// <param name="peopleIn">Люди в доме</param>
        /// <param name="auctionsIn">Аукционов в доме</param>
        /// <param name="description">Описание дома</param>
        public HouseMobile(string name ,string photoPath, int likes, int dislikes, int rating, int peopleIn,
            int auctionsIn, string description, bool isSubscribed)
        {
            this.title = name;
            this.photoPath = photoPath;
            this.likes = likes;
            this.dislikes = dislikes;
            this.rating = rating;
            peopleAmount = peopleIn;
            auctionsAmount = auctionsIn;
            this.description = description;
            this.isSubscribed = isSubscribed;
        }

        /// <summary>
        /// Путь к фоточке
        /// </summary>
        public string photoPath { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int likes { get; set; }

        /// <summary>
        /// Количество дизлайков
        /// </summary>
        public int dislikes { get; set; }

        /// <summary>
        /// Рэйтинг дома
        /// </summary>
        public int rating { get; set; }

        /// <summary>
        /// Количество человек в доме
        /// </summary>
        public int peopleAmount { get; set; }

        /// <summary>
        /// Количество аукционов в доме
        /// </summary>
        public int auctionsAmount { get; set; }

        /// <summary>
        /// Описание дома
        /// </summary>
        public string description { get; set; }

        public bool isSubscribed { get; set; }

        public string title { get; set; }
    }
}
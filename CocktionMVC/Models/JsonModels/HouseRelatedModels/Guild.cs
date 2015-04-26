namespace CocktionMVC.Models.JsonModels.HouseRelatedModels
{
    /// <summary>
    /// Контейнер для информации о дом-содержащей организации
    /// </summary>
    public class Guild
    {
        public Guild()
        {

        }

        public Guild(string name, string photoPath, int id)
        {
            this.title = name;
            this.photoPath = photoPath;
            this.id = id;
        }

        /// <summary>
        /// Название дома
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Путь к фоточке
        /// </summary>
        public string photoPath { get; set; }

        /// <summary>
        /// Айдишник организации
        /// </summary>
        public int id { get; set; }

    }
}
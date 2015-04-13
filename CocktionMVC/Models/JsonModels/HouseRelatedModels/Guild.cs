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
            GuildName = name;
            PhotoPath = photoPath;
            GuildId = id;
        }

        /// <summary>
        /// Название дома
        /// </summary>
        public string GuildName { get; set; }

        /// <summary>
        /// Путь к фоточке
        /// </summary>
        public string PhotoPath { get; set; }

        /// <summary>
        /// Айдишник организации
        /// </summary>
        public int GuildId { get; set; }

    }
}
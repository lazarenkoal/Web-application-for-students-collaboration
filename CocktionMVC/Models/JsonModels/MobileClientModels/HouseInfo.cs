namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Контейнер для информации о доме, которая посылается на мобильный клиент
    /// </summary>
    public class HouseInfo
    {
        public string Faculty { get; set; }
        public string University { get; set; }
        public string Adress { get; set; }
        public int Rating { get; set; }
        public int Likes { get; set; }
    }
}
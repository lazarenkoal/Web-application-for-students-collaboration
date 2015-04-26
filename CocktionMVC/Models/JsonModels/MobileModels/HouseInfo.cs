namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Контейнер для информации о доме, которая посылается на мобильный клиент
    /// </summary>
    public class HouseInfo
    {
        public string title { get; set; }
        public string adress { get; set; }
        public int rating { get; set; }
        public int likes { get; set; }
        public bool isSubscribed { get; set; }
    }
}
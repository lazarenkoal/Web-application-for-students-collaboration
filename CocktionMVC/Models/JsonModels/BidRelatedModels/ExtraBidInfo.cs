namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для отображения информации о 
    /// дополнительном довеске к аукциону
    /// </summary>
    public class ExtraBidInfo
    {
        /// <summary>
        /// Айдишник самой первой ставки в кластере
        /// </summary>
        public int FirstBidId { get; set; }

        /// <summary>
        /// Айдишник данной добавки в кластер
        /// </summary>
        public int ThisBidId { get; set; }
    }
}
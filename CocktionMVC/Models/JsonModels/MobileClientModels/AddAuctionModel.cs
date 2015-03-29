namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Служит в качестве класса, который поступает джейсоном
    /// из мобильных клиентов на сервер с информацией об аукционе.
    /// </summary>
    public class AddAuctionModel
    {
        /// <summary>
        /// Айдишник аукциона
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание товара
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Категория, к которой относится товар
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Количество минут, которые нужны
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Количество часов, которые будет длиться аукцион
        /// </summary>
        public int Hours { get; set; }
    }
}
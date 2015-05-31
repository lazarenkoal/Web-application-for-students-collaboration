namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Класс для вывода информации о продукте
    /// на мобильный клиент
    /// </summary>
    public class  ProductInfo
    {
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Описание продукта
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string photoPath { get; set; }

        /// <summary>
        /// Айди товара ставки
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public string category { get; set; }

    }
}
namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для вывода информации о продукте
    /// на мобильный клиент
    /// </summary>
    public class ProductInfoMobile
    {
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string name { get; set; }

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
        public int productId { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public string category { get; set; }

        public int leaderId { get; set; }
    }
}
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
        public string ProductName { get; set; }

        /// <summary>
        /// Описание продукта
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string ProductFileName { get; set; }

        /// <summary>
        /// Айди товара ставки
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public string ProductCategory { get; set; }
    }
}
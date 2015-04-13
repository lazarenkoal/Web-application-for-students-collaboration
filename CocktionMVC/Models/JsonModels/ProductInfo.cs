namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Контейнер для информации о товаре
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание продукта
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public string Category { get; set; }

    }
}
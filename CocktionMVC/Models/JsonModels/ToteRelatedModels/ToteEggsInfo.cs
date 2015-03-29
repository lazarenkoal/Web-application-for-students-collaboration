namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    ///  Контейнер для информации о яйцах
    /// </summary>
    public class ToteEggsInfo
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Количество яиц у пользователя
        /// </summary>
        public int UsersAmountOfEggs { get; set; }
    }
}
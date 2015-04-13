namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    public class AuctionApiRespondModels
    {
        /// <summary>
        /// Контейнер для информации со статусом добавления аукциона 
        /// с мобильника
        /// </summary>
        public class AuctionCreateStatus
        {
            /// <summary>
            /// Статус
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Путь к фотке
            /// </summary>
            public string PhotoPath { get; set; }
        }
    }
}
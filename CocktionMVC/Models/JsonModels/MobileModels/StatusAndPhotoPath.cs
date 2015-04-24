namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Контейнер для информации со статусом добавления аукциона 
    /// с мобильника
    /// </summary>
    public class StatusAndPhotoPath
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
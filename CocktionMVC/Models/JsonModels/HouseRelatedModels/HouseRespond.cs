 // ReSharper disable once CheckNamespace
namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Моделирует дом, который вернется создателю дома (пока что админу)
    /// при создании какого-то конкретного дома!
    /// </summary>
    public class HouseRespond
    {
        /// <summary>
        /// Информация о доме
        /// </summary>
        public string House { get; set; }
    }
}
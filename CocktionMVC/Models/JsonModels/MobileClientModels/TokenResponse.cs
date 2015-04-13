namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Контейнер для токена для мобильника
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Токен, который шлем в ответочку телефону
        /// </summary>
        public string Token { get; set; }
    }
}
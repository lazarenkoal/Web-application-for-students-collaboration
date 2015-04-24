namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Логин/Пароль контейнер для мобильника
    /// </summary>
    public class LoginCredentials
    {
        /// <summary>
        /// Почтовый адрес, он же - логин
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

    }
}
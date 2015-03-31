namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Логин/Пароль контейнер для мобильника
    /// </summary>
    public class MobileUserLogin
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
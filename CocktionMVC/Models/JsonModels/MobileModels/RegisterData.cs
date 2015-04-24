namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Контейнер для данных для регистрации с мобильного клиента
    /// </summary>
    public class RegisterData : LoginCredentials
    {
        //нужны поля Email и Password c телефона

        /// <summary>
        /// Настоящее имя пользователя
        /// </summary>
        public string UserRealName { get; set; }
        
        /// <summary>
        /// Настоящая фамилия пользователя
        /// </summary>
        public string UserRealSurname { get; set; }

        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        public string PhoneNumber { get; set; }


    }
}
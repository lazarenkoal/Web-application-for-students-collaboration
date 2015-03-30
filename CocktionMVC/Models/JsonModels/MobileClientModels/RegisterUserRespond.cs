namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Моделирует ответ при регистрации пользователя 
    /// с мобильного клиента
    /// </summary>
    public class RegisterUserRespond
    {
        /// <summary>
        /// Поле для статуса о регистрации
        /// </summary>
        public string StatusOfRegistration { get; set; }

        /// <summary>
        /// Создает объект с нужным статусом
        /// </summary>
        /// <param name="str">Строчка со статусиком</param>
        public RegisterUserRespond(string str)
        {
            StatusOfRegistration = str;
        }
    }
}
﻿namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Логин/Пароль контейнер для мобильника
    /// </summary>
    public class LoginCredentials
    {
        /// <summary>
        /// Почтовый адрес, он же - логин
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string password { get; set; }

    }
}
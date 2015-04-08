using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Класс для Хренения ответа являеется ли пользователь
    /// владельцем аукциона
    /// </summary>
    public class IsOwnerResponder
    {
        /// <summary>
        /// Информация о владении
        /// </summary>
        public bool? IsOwner { get; set; }

        public IsOwnerResponder() { }

        public IsOwnerResponder(bool? isOwner)
        {
            IsOwner = isOwner;
        }
    }
}
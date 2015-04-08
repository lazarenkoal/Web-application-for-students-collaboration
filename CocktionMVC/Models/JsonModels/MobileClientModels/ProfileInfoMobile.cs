using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Для просмотра инфы о пользователе с мобилки
    /// </summary>
    public class ProfileInfoMobile
    {
        public ProfileInfoMobile() { }
        
        /// <summary>
        /// Конструирует объект с инфой о пользователе по всем полям
        /// </summary>
        /// <param name="photoPath">Путь к фотке с аватаркой</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="userSurname">Фамилия пользователя</param>
        /// <param name="userEggsAmount">Количество яиц у пользователя</param>
        /// <param name="userAuctionAmount">Количество аукционов у пользователя</param>
        /// <param name="interest1Name">Название первого интереса</param>
        /// <param name="interest1PhotoPath">Путь к фотке с первым интересом</param>
        /// <param name="interest2Name">Название второго интереса</param>
        /// <param name="interest2PhotoPath">Путь к фотке со вторым интересом</param>
        /// <param name="interest3Name">Название третьего интереса</param>
        /// <param name="interest3PhotoPath">Путь к фотке третьего интереса</param>
        /// <param name="interest4Name">Название четверотого интереса</param>
        /// <param name="interest4PhotoPath">Путь к фотке четвертого интереса</param>
        public ProfileInfoMobile(string photoPath, string userName, string userSurname, int userEggsAmount,
            int userAuctionAmount, string interest1Name, string interest1PhotoPath,
            string interest2Name, string interest2PhotoPath, string interest3Name, string interest3PhotoPath,
            string interest4Name, string interest4PhotoPath)
        {
            PhotoPath = photoPath;

            UserName = userName;
            UserSurname = userSurname;
            UserEggsAmount = userEggsAmount;
            UserAuctionAmount = userAuctionAmount;

            Interest1Name = interest1Name;
            Interest1PhotoPath = interest1PhotoPath;

            Interest2Name = interest2Name;
            Interest2PhotoPath = interest2PhotoPath;

            Interest3Name = interest3Name;
            Interest3PhotoPath = interest3PhotoPath;

            Interest4Name = interest4Name;
            Interest4PhotoPath = interest4PhotoPath;
        }

        /// <summary>
        /// Путь к фоточке пользователя
        /// </summary>
        public string PhotoPath { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string UserSurname { get; set; }

        /// <summary>
        /// Количество яиц у пользователя
        /// </summary>
        public int UserEggsAmount { get; set; }

        /// <summary>
        /// Количество аукционов у пользователя
        /// </summary>
        public int UserAuctionAmount { get; set; }

        /// <summary>
        /// Название первого интереса
        /// </summary>
        public string Interest1Name { get; set; }

        /// <summary>
        /// Путь к фоточке с первым интересом
        /// </summary>
        public string Interest1PhotoPath { get; set; }

        /// <summary>
        /// Название второго интереса
        /// </summary>
        public string Interest2Name { get; set; }

        /// <summary>
        /// Путь к фоточке с вторым интересом
        /// </summary>
        public string Interest2PhotoPath { get; set; }

        /// <summary>
        /// Название третьего интереса
        /// </summary>
        public string Interest3Name { get; set; }

        /// <summary>
        /// Путь к фоточке с третьим интересом
        /// </summary>
        public string Interest3PhotoPath { get; set; }

        /// <summary>
        /// Название четвертого интереса
        /// </summary>
        public string Interest4Name { get; set; }

        /// <summary>
        /// Путь к фоточке с четвертым интересом
        /// </summary>
        public string Interest4PhotoPath { get; set; }

    }
}
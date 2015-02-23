using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Модель, для отсылки JSON'a для карт
    /// </summary>
    public class LocationInfo
    {
        /// <summary>
        /// Конструктор, который определяет сразу все поля, что есть
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longtitude">Долгота</param>
        /// <param name="iconContent">Контент иконки (видно сразу на карте)</param>
        /// <param name="baloonContent">Контент балуна (видно сразу при клике)</param>
        /// <param name="options">Опции</param>
        public LocationInfo(double latitude, double longtitude, string iconContent,
                            string baloonContent, string options)
        {
            Coordinates = new List<double>(2);
            Coordinates.Add(latitude);
            Coordinates.Add(longtitude);
            Baloon = new BalloonInfo(iconContent, baloonContent);
            Options = new BaloonOptions(options);
        }

        /// <summary>
        /// Информация о балуне
        /// </summary>
        public BalloonInfo Baloon {get; set;}

        /// <summary>
        /// Опции для балуна
        /// </summary>
        public BaloonOptions Options { get; set; }

        public LocationInfo() { }

        /// <summary>
        /// Массивчик с координатами, их нужно вставлять всего 2
        /// в порядке Latitude, Longtitude
        /// </summary>
        public List<double> Coordinates { get; set; }

        /// <summary>
        /// Класс для формирования надписей в баллуне
        /// </summary>
        public class BalloonInfo
        {
            public string IconContent { get; set; }
            public string BaloonContent { get; set; }

            public BalloonInfo(string iconContent, string baloonContent)
            {
                IconContent = iconContent;
                BaloonContent = baloonContent;
            }

            public BalloonInfo() { }
        }

        /// <summary>
        /// Опции для балуна
        /// </summary>
        public class BaloonOptions
        {
            public string Preset { get; set; }

            public BaloonOptions(string options)
            {
                Preset = options;
            }

            public BaloonOptions() { }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    public class ProductInfo
    {
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание продукта
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }
    }
}
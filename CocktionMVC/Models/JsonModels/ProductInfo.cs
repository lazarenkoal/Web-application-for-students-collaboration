using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для вывода информации о продукте
    /// на клиент
    /// </summary>
    public class ProductInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.ViewModels
{
    /// <summary>
    /// Модель для представления укороченной
    /// информации о ставке для аукциона
    /// </summary>
    public class ProductShortViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FileName { get; set; }
    }
}
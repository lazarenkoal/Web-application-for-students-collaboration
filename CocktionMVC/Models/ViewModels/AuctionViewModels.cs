using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CocktionMVC.Models.DAL;
namespace CocktionMVC.Models
{
    /// <summary>
    /// Класс, который содержит модели для вывода их на экран.
    /// </summary>
    public class AuctionViewModels
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int ProductRating { get; set; }

        public Photo Image { get; set; }

    }
}
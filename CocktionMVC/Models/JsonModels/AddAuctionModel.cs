using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Служит в качестве класса, который поступает джейсоном
    /// из мобильных клиентов на сервер с информацией об аукционе.
    /// </summary>
    public class AddAuctionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public int Minutes { get; set; }

        public int Hours { get; set; }
    }
}
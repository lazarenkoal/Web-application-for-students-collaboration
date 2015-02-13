using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для предоставления информации для клиента
    /// </summary>
    public class BidSeller
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
    }
}
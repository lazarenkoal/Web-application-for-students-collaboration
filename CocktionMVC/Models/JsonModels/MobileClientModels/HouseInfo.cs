using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    public class HouseInfo
    {
        public string Faculty { get; set; }
        public string University { get; set; }
        public string Adress { get; set; }
        public int Rating { get; set; }
        public int Likes { get; set; }
    }
}
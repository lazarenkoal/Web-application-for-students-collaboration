using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Класс для отображения информации о 
    /// дополнительном довеске к аукциону
    /// </summary>
    public class ExtraBidInfo
    {
        public int FirstBidId { get; set; }

        public int ThisBidId { get; set; }
    }
}
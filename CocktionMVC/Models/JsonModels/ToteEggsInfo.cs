using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    ///  Класс для представления информации
    ///  о яичках
    /// </summary>
    public class ToteEggsInfo
    {
        public string Status { get; set; }
        public int UsersAmountOfEggs { get; set; }
    }
}
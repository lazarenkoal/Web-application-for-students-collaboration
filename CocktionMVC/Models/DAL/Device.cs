using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    public class Device
    {
        public int Id { get; set; }

        /// <summary>
        ///  ios/android/wp
        /// </summary>
        public string Type { get; set; }

        public string Token { get; set; }
    }
}
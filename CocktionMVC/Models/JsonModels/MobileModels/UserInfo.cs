using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    public class UserInfo
    {
        public string id { get; set; }
        public string name { get; set; }

        public string photoPath { get; set; }

        public bool isInformator { get; set; }

        public UserInfo(string id, string name, string photoPath, bool isInformator)
        {
            this.id = id;
            this.name = name;
            this.photoPath = @"http://cocktion.com/Images/Thumbnails/" + photoPath;
            this.isInformator = isInformator;
        }
    }
}
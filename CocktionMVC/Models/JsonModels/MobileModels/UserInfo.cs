using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    public class UserInfo
    {
        public string id { get; set; }
        public string title { get; set; }

        public string photoPath { get; set; }

        public bool isInformator { get; set; }

        public UserInfo(string id, string name, string photoPath, bool isInformator)
        {
            this.id = id;
            this.title = name;
            this.photoPath = @"http://cocktion.com/Images/Thumbnails/" + photoPath;
            this.isInformator = isInformator;
        }
    }
}
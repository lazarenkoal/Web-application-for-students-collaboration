using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    public class Picture
    {
        public Picture() { }
        public Picture(string fileName)
        {
            FileName = fileName;
        }
        public int Id { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }
    }
}
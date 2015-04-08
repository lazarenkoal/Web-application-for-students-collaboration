using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    public class StatusHolder
    {
        public StatusHolder() { }

        public StatusHolder(bool isSuccessful)
        {
            if (isSuccessful)
                Status = "Success";
            else
                Status = "Failure";
        }

        public string Status { get; set; }
    }
}
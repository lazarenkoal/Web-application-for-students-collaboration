using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    public class AuctionApiRespondModels
    {
        /// <summary>
        /// Служит в качестве хранителя ответного сообщения
        /// сюда же швыряем ссылочку на фамбнейлик.
        /// </summary>
        public class AuctionCreateStatus
        {
            public string Status { get; set; }

            public string PhotoPath { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Данная модель служит для отправки логического статуса
    /// о том, есть ставка уже у этого пользователя или нет!
    /// </summary>
    public class BidChecker
    {
        /// <summary>
        /// Индикатор того, поставлена ставка или нет!
        /// </summary>
        public bool HaveBid { get; set; }
    }
}
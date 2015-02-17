using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Сущность, олицетворяющая ставку яйцами на аукционе.
    /// </summary>
    public class ToteEntity
    {
        //Номер для хранения в базе данных
        public int Id { get; set; }

        //Айди пользователя
        public string UserId { get; set; }

        //Количество яиц
        public int EggsAmount { get; set; }

        //Айдишник продукта
        public int ProductId { get; set; }

        //связь с отцом-тотализатором
        public virtual ToteBoard OwnerTote { get; set; }
    }
}
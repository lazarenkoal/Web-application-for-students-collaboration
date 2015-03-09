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
        /// <summary>
        /// Номер для хранения в базе данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Айди пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Количество яиц
        /// </summary>
        public int EggsAmount { get; set; }

        /// <summary>
        /// Айдишник продукта
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// связь с отцом-тотализатором
        /// </summary>
        public virtual ToteBoard OwnerTote { get; set; }
    }
}
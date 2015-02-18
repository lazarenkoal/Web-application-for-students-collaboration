using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Класс, содержащий в себе результаты аукциона
    /// </summary>
    public class ToteResult
    {
        /// <summary>
        /// Айди для базы данных
        /// </summary>
        public int Id { get; set; }
  
        /// <summary>
        /// Айди человека, к которому относится результат
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Его выручка в яйцах
        /// </summary>
        public int Profit { get; set; }

        /// <summary>
        /// Успешнометр
        /// </summary>
        public bool IsSucсessful { get; set; }

        /// <summary>
        /// Тотализатор - владелец это ставки
        /// </summary>
        public virtual ToteBoard OwnerTote { get; set; }
    }
}
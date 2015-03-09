using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Моделирует пост на форуме
    /// </summary>
    public class ForumPost
    {
        /// <summary>
        /// Айдишник для базы данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Автор поста
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Само сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int Likes { get; set; }

        public virtual House HostHouse { get; set; }
    }
}
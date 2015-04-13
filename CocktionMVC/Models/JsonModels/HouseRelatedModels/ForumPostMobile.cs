namespace CocktionMVC.Models.JsonModels.HouseRelatedModels
{
    /// <summary>
    /// Контейнер для сообщений с форума
    /// </summary>
    public class ForumPostMobile
    {
        public ForumPostMobile() { }

        public ForumPostMobile(string author, string message, int likes)
        {
            AuthorName = author;
            Message = message;
        }

        /// <summary>
        /// Имя автора
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Сам сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int Likes { get; set; }
    }
}
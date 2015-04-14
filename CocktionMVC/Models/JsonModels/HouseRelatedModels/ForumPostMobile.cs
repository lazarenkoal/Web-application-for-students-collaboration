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
            authorName = author;
            this.message = message;
            this.likes = likes;
        }

        /// <summary>
        /// Имя автора
        /// </summary>
        public string authorName { get; set; }

        /// <summary>
        /// Сам сообщение
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int likes { get; set; }
    }
}
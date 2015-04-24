namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Моделирует пост на форуме
    /// </summary>
    public class ForumPost
    {
        public ForumPost(string message, string authorName)
        {
            Message = message;
            AuthorName = authorName;
            Likes = 0;
        }

        public ForumPost()
        { }

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
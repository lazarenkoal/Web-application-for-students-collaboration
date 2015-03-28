namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Моделирует ответ с сервера о статусе добавления
    /// сообщения на форум
    /// </summary>
    public class ForumRespond
    {
        /// <summary>
        /// Статус отправки
        /// "Success" если успех
        /// "Failed" если провал
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Автор сообщения
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Собирает вариант, в котором важен автор
        /// </summary>
        /// <param name="status">Стутус отправки</param>
        /// <param name="author">Автор сообщения</param>
        public ForumRespond(string status, string author)
        {
            Author = author;
            Status = status;
        }

        /// <summary>
        /// Собирает вариант, в котором важен только статус
        /// </summary>
        /// <param name="status">Статус отправки</param>
        public ForumRespond(string status)
        {
            Status = status;
        }
    }
}
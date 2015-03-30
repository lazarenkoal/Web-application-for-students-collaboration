using System.Collections.Generic;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Моделирует аукционный дом!!!
    /// </summary>
    public class House
    {
        public House()
        {
            Auctions = new HashSet<Auction>();
            Posts = new HashSet<ForumPost>();
        }

        public House(string adress, string university, string faculty)
        {
            Auctions = new HashSet<Auction>();
            Posts = new HashSet<ForumPost>();
            Address = adress;
            Faculty = faculty;
            University = university;
            Likes = 0;
            Rating = 0;
        }

        /// <summary>
        /// Айдишник для базы данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Факультет, но если добавляется общага - 
        /// пишем сюда общагу
        /// </summary>
        public string Faculty { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Университет, к которому привязана локация.
        /// Конвенция: все указываем одинаково. 
        /// ВШЭ/МГУ/МГТУ/МФТИ/РГТУ
        /// </summary>
        public string University { get; set; }

        /// <summary>
        /// Аукционы на этой площадке
        /// </summary>
        public virtual ICollection<Auction> Auctions { get; set; }

        /// <summary>
        /// Сообщения для форума.
        /// </summary>
        public virtual ICollection<ForumPost> Posts { get; set; }
    }
}
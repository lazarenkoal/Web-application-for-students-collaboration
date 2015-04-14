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
            Inhabitants = new HashSet<AspNetUser>();
        }

        /// <summary>
        /// Конструирует дом по заданным параметрам
        /// </summary>
        /// <param name="adress">Адрес дома</param>
        /// <param name="faculty">Факультет / обшага</param>
        /// <param name="holder">Холдер, к которому принадлежит дом</param>
        /// <param name="photo">Фоточка, которая нужна для дома</param>
        public House(string adress, string faculty, HouseHolder holder, Picture photo)
        {
            Auctions = new HashSet<Auction>();
            Posts = new HashSet<ForumPost>();
            Adress = adress;
            Faculty = faculty;
            Likes = 0;
            Rating = 0;
            Holder = holder;
            Portrait = photo;
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
        public string Adress { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rating { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Аукционы на этой площадке
        /// </summary>
        public virtual ICollection<Auction> Auctions { get; set; }

        /// <summary>
        /// Сообщения для форума.
        /// </summary>
        public virtual ICollection<ForumPost> Posts { get; set; }

        public virtual ICollection<AspNetUser> Inhabitants { get; set; }

        public virtual Picture Portrait { get; set; }

        public virtual HouseHolder Holder { get; set; }
    }
}
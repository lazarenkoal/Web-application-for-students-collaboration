using System.Collections.Generic;
using System.ComponentModel;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Models.ViewModels
{
    /// <summary>
    /// Модель для просмотра профиля пользователя
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// TODO Сделать интересы пользователя и подписки
        /// </summary>
        public ProfileViewModel()
        {
        }

        public ProfileViewModel(int eggsAmount, int auctionsAmount, int productsAmount,
            string surname, string name, int? rating, int daysWithUs, List<Auction> hisAuctions, string userId,
            List<UsersFeedback> feeds, List<House> houses)
        {
            EggsAmount = eggsAmount;
            AuctionsAmount = auctionsAmount;
            ProductsAmount = productsAmount;
            Surname = surname;
            Name = name;
            Rating = rating;
            DaysWithUsAmount = daysWithUs;
            HisAuctions = hisAuctions;
            UserId = userId;
            Feeds = feeds;
            Houses = houses;
        }
        public List<House> Houses { get; set; }
        public List<UsersFeedback> Feeds { get; set; }
        public string UserId { get; set; }
        public int EggsAmount { get; set; }
        public int AuctionsAmount { get; set; }
        public int ProductsAmount { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; }
        public int DaysWithUsAmount { get; set; }
        public List<Auction> HisAuctions { get; set; } 
    }
}
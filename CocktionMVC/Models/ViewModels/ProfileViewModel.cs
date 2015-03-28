namespace CocktionMVC.Models.ViewModels
{
    /// <summary>
    /// Модель для просмотра статистики о пользователе
    /// </summary>
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
        }

        public ProfileViewModel(int eggsAmount, int auctionsAmount, int productsAmount)
        {
            this.EggsAmount = eggsAmount;
            this.AuctionsAmount = auctionsAmount;
            this.ProductsAmount = productsAmount;
        }
        public int EggsAmount { get; set; }
        public int AuctionsAmount { get; set; }
        public int ProductsAmount { get; set; }
    }
}
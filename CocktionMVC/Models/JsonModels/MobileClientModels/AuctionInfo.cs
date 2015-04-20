namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Класс для модели аукциона, которая будет отсылаться
    /// мобильным клиентам.
    /// </summary>
    public class AuctionInfo
    {
        public override bool Equals(object obj)
        {
            return this.auctionId == ((AuctionInfo) obj).auctionId;
        }

        /// <summary>
        /// Описание аукциона
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Название аукциона
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Путь к картинке для аукциона
        /// </summary>
        public string photoPath { get; set; }

        /// <summary>
        /// Время окончания аукциона в минутах от текущего
        /// </summary>
        public int endTime { get; set; }

        /// <summary>
        /// Айдишник аукциона
        /// </summary>
        public int auctionId { get; set; }

        public int leaderId { get; set; }
        public string сategory { get; set; }

        public AuctionInfo(string description, int endTime, string photoPath, string title,
            int auctionId, int leaderId, string category)
        {
            this.description = description;
            this.endTime = endTime;
            this.photoPath = photoPath;
            this.title = title;
            this.auctionId = auctionId;
            this.leaderId = leaderId;
            this.сategory = category;
        }

        public AuctionInfo()
        {
        }
    }
}
namespace CocktionMVC.Models.JsonModels.MobileClientModels
{
    /// <summary>
    /// Класс для модели аукциона, которая будет отсылаться
    /// мобильным клиентам.
    /// </summary>
    public class AuctionInfo
    {

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

        public ownerInfo owner { get; set; }

        public AuctionInfo(string description, int endTime, string photoPath, string title,
            int auctionId, int leaderId, string category, ownerInfo info)
        {
            this.description = description;
            this.endTime = endTime;
            this.photoPath = photoPath;
            this.title = title;
            this.auctionId = auctionId;
            this.leaderId = leaderId;
            this.сategory = category;
            this.owner = info;
        }

        public AuctionInfo()
        {
        }
    }

    public class ownerInfo
    {
        public string id { get; set; }
        public string name { get; set; }

        public string photoPath { get; set; }

        public ownerInfo(string id, string name, string photoPath)
        {
            this.id = id;
            this.name = name;
            this.photoPath = @"http://cocktion.com/Images/Thumbnails/" + photoPath;
        }
    }
}
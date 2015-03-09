using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    /// <summary>
    /// Моделирует сущность ставки (товарной) на аукционе.
    /// Нужен для того, чтобы можно было понять, в какие группы
    /// чувачок объединяет товары. И, чтобы можно было их спокойно выводить
    /// на экраны.
    /// </summary>
    public class BidCluster
    {

        public BidCluster()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public virtual Auction HostAuction { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
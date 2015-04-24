using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models.DAL
{
    public class Product
    {
        public Product()
        {
            BidClusters = new HashSet<BidCluster>();
        }

        public Product(string name, string description, string category,
             bool isOnAuctionAsALot, Picture photo, AspNetUser user, Auction auction) 
            : this(name,description, category, isOnAuctionAsALot, user)
        {
            Photo = photo;
            SelfAuction = auction;
        }

        public Product(string name, string description, string category,
             bool isOnAuctionAsALot, AspNetUser user) : this()
        {
            Name = name;
            Description = description;
            Category = category;
            IsOnAuctionAsALot = isOnAuctionAsALot;
            Owner = user;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(350)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public bool IsOnAuctionAsALot { get; set; }

        public virtual AspNetUser Owner { get; set; }

        public virtual ICollection<BidCluster> BidClusters { get; set; }

        public virtual Picture Photo { get; set; }

        public virtual Auction SelfAuction { get; set; }
    }
}

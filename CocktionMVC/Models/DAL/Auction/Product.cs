using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models.DAL
{
    public class Product
    {
        public Product()
        {
           // Auctions = new HashSet<Auction>();
            //BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
        }

        public Product(string name, string description, string category,
             bool isOnAuctionAsALot, AspNetUser user)
        {
            //Auctions = new HashSet<Auction>();
            //BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
            Name = name;
            Description = description;
            Category = category;
            IsOnAuctionAsALot = isOnAuctionAsALot;
            Owner = user;
        }

        public Product(string name, string description, string category, AspNetUser user)
        {
            //Auctions = new HashSet<Auction>();
            //BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
            Description = description;
            Name = name;
            Category = category;
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

       // public virtual ICollection<Auction> Auctions { get; set; }
        //public virtual ICollection<Auction> BidAuctions { get; set; }

        public virtual ICollection<BidCluster> BidClusters { get; set; }

        public virtual Picture Photo { get; set; }

        public virtual Auction SelfAuction { get; set; }
    }
}

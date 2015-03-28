using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CocktionMVC.Models.DAL
{
    public partial class Product
    {
        public Product()
        {
            Auctions = new HashSet<Auction>();
            Photos = new HashSet<Photo>();
            LidAuctions = new HashSet<Auction>();
            BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
        }

        public Product(string name, string description, string category, string ownerId,
             bool isOnAuctionAsALot, string ownerName)
        {
            Auctions = new HashSet<Auction>();
            Photos = new HashSet<Photo>();
            LidAuctions = new HashSet<Auction>();
            BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
            Name = name;
            Description = description;
            Category = category;
            OwnerId = ownerId;
            IsOnAuctionAsALot = isOnAuctionAsALot;
            OwnerName = ownerName;
        }

        public Product(string name, string description, string category, string ownerId,
            string ownerName)
        {
            Auctions = new HashSet<Auction>();
            Photos = new HashSet<Photo>();
            LidAuctions = new HashSet<Auction>();
            BidAuctions = new HashSet<Auction>();
            BidClusters = new HashSet<BidCluster>();
            Description = description;
            Name = name;
            Category = category;
            OwnerId = ownerId;
            OwnerName = ownerName;
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

        public string OwnerId { get; set; }

        public int Rating { get; set; }

        public int Likes { get; set; }

        public int OnAuctionTime { get; set; }

        public bool IsOnAuctionAsALot { get; set; }

        public string OwnerName { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Auction> LidAuctions { get; set; }

        public virtual ICollection<Auction> BidAuctions { get; set; }

        public virtual ICollection<BidCluster> BidClusters { get; set; }
    }
}

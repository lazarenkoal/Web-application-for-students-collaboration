namespace CocktionMVC.Models.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public Product()
        {
            Auctions = new HashSet<Auction>();
            Photos = new HashSet<Photo>();
            LidAuctions = new HashSet<Auction>();
            BidAuctions = new HashSet<Auction>();
            GeoLocations = new HashSet<Location>();
            BidClusters = new HashSet<BidCluster>();
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

        public virtual ICollection<Location> GeoLocations { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Auction> LidAuctions { get; set; }

        public virtual ICollection<Auction> BidAuctions { get; set; }

        public virtual ICollection<BidCluster> BidClusters { get; set; }
    }
}

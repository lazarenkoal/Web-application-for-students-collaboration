using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models.DAL
{
    public partial class Auction
    {
        public Auction()
        {
            BidProducts = new HashSet<Product>();
            Houses = new HashSet<House>();
            UsersBids = new HashSet<BidCluster>();
        }

        public Auction(bool isActive, string ownerId, string ownerName, Product product,
            bool winnerChosen, ToteBoard tote)
        {
            BidProducts = new HashSet<Product>();
            Houses = new HashSet<House>();
            UsersBids = new HashSet<BidCluster>();
            IsActive = isActive;
            OwnerId = ownerId;
            OwnerName = ownerName;
            SellProduct = product;
            WinnerChosen = winnerChosen;
            AuctionToteBoard = tote;
        }
        
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime StartTime { get; set; }

        [Required]
        public string OwnerName { get; set; }

        //public string WinProductId { get; set; }

        ///public string WinProductName { get; set; }

        public bool IsActive { get; set; }
         
        public bool WinnerChosen { get; set; }

        public int SellProductId { get; set; }

        public virtual ToteBoard AuctionToteBoard { get; set; }

        public virtual Product LeadProduct { get; set; }

        public virtual Product SellProduct { get; set; }
        public virtual ICollection<BidCluster> UsersBids { get; set; }
        public virtual ICollection<House> Houses { get; set; }

        public virtual ICollection<Product> BidProducts { get; set; }
    }
}

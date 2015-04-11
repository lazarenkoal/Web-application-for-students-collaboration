using System;
using System.Collections.Generic;

namespace CocktionMVC.Models.DAL
{
    public class Auction
    {
        public Auction()
        {
            BidProducts = new HashSet<Product>();
            Houses = new HashSet<House>();
            UsersBids = new HashSet<BidCluster>();
        }

        public Auction(bool isActive, Product product,
            bool winnerChosen, ToteBoard tote, AspNetUser user)
        {
            BidProducts = new HashSet<Product>();
            Houses = new HashSet<House>();
            UsersBids = new HashSet<BidCluster>();
            IsActive = isActive;
            SellProduct = product;
            WinnerChosen = winnerChosen;
            AuctionToteBoard = tote;
            Owner = user;
        }
        
        public int Id { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime StartTime { get; set; }

        public bool IsActive { get; set; }
         
        public bool WinnerChosen { get; set; }

        public int SellProductId { get; set; }

        public virtual ToteBoard AuctionToteBoard { get; set; }

        public virtual Product LeadProduct { get; set; }
        public virtual AspNetUser Owner { get; set; }

        public virtual Product SellProduct { get; set; }
        public virtual ICollection<BidCluster> UsersBids { get; set; }
        public virtual ICollection<House> Houses { get; set; }

        public virtual ICollection<Product> BidProducts { get; set; }
    }
}

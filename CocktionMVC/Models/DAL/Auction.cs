namespace CocktionMVC.Models.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Auction
    {
        public Auction()
        {
            LidProducts = new HashSet<Product>();
            BidProducts = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        public string OwnerName { get; set; }

        public string WinProductId { get; set; }

        public bool IsActive { get; set; }
         
        public bool? WinnerChosen { get; set; }

        public int SellProduct_Id { get; set; }

        public virtual Product SellProduct { get; set; }

        public virtual ICollection<Product> LidProducts { get; set; }

        public virtual ICollection<Product> BidProducts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models.DAL
{
    public class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetRoles = new HashSet<AspNetRole>();
            HisAuctions = new HashSet<Auction>();
            HisProducts = new HashSet<Product>();
            SubHouses = new HashSet<House>();
            Friends = new HashSet<AspNetUser>();
            ChatMessages = new HashSet<PrivateMessage>();
        }

        public string Id { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public int Eggs { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public string UserRealName { get; set; }

        public string UserRealSurname { get; set; }

        public string SocietyName { get; set; }

        public int? Rating { get; set; }

        public virtual Picture Selfie {get; set; }
        public virtual ICollection<House> SubHouses { get; set; } 
        public virtual ICollection<Auction> HisAuctions { get; set; } 
        public virtual ICollection<Product> HisProducts { get; set; } 

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public virtual ICollection<AspNetUser> Friends { get; set; } 

        public virtual HouseHolder HisHouseHolder { get; set; } 
        public virtual ICollection<Interest> Interests { get; set; }

        public virtual ICollection<PrivateMessage> ChatMessages { get; set; } 
    }
}

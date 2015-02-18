namespace CocktionMVC.Models.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class CocktionContext : DbContext
    {
        public CocktionContext()
            : base("name=DefaultConnection")
        {
        }
        public virtual DbSet<ToteBoard> ToteBoards { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ThumbnailSet> ThumbnailSets { get; set; }
        public virtual DbSet<ToteEntity> ToteEntities { get; set; }
        public virtual DbSet<ToteResult> ToteResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.LidProducts) //Products ---> LidProducts
                .WithMany(e => e.LidAuctions) //Auctions1 ---> LidAuctions
                .Map(m => m.ToTable("AuctionProduct2").MapLeftKey("LidAuctions_Id").MapRightKey("LidProducts_Id"));

            modelBuilder.Entity<Auction>() 
                .HasMany(e => e.BidProducts)  //Products1 ---> BidProducts 
                .WithMany(e => e.BidAuctions)   //Auctions2 ---> BidAuctions
                .Map(m => m.ToTable("ProductAuction1").MapLeftKey("BidAuctions_Id").MapRightKey("BidProducts_Id"));

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.ThumbnailSets)
                .WithRequired(e => e.Photo)
                .HasForeignKey(e => e.Photo_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.Category)
                .IsFixedLength();

            modelBuilder.Entity<Auction>()
                .HasRequired(e => e.AuctionToteBoard);

            modelBuilder.Entity<ToteBoard>()
                .HasMany(e => e.Bids);

            modelBuilder.Entity<ToteBoard>()
                .HasMany(e => e.ToteResultsForUsers);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.SellProduct)
                .HasForeignKey(e => e.SellProduct_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Photos)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_Id)
                .WillCascadeOnDelete(false);
        }
    }
}

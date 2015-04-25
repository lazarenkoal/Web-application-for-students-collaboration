using System.Data.Entity;

namespace CocktionMVC.Models.DAL
{
    public class CocktionContext : DbContext
    {
        public CocktionContext()
            : base("name=DefaultConnection")
        {
        }
        public virtual DbSet<ToteBoard> ToteBoards { get; set; }
        public virtual DbSet<UsersFeedback> Feedbacks { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<BidCluster> AuctionBids { get; set; }
        public virtual DbSet<ToteEntity> ToteEntities { get; set; }
        public virtual DbSet<ToteResult> ToteResults { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<ForumPost> Posts { get; set; }

        public virtual DbSet<Interest> Interests { get; set; }

        public virtual DbSet<HouseHolder> HouseHolders { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<PrivateMessage> Messages { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interest>()
                .HasMany(e => e.Subscribers);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Friends);

            modelBuilder.Entity<HouseHolder>()
                .HasMany(e => e.Houses);

            modelBuilder.Entity<HouseHolder>()
                .HasMany(e => e.Users);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Interests);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.ChatMessages);

            modelBuilder.Entity<House>()
                .HasMany(e => e.Auctions);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.Houses);

            modelBuilder.Entity<House>()
                .HasMany(e => e.Posts);

            modelBuilder.Entity<House>()
                .HasMany(e => e.Inhabitants);
           
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

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.HisAuctions);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.HisProducts);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.SubHouses);

            //modelBuilder.Entity<Auction>() 
            //    .HasMany(e => e.BidProducts)  //Products1 ---> BidProducts 
            //    .WithMany(e => e.BidAuctions)   //Auctions2 ---> BidAuctions
            //    .Map(m => m.ToTable("ProductAuction1").MapLeftKey("BidAuctions_Id").MapRightKey("BidProducts_Id"));



            modelBuilder.Entity<Auction>()
                .HasMany(e => e.UsersBids);

            modelBuilder.Entity<PrivateMessage>()
               .HasMany(e => e.Talkers);

            modelBuilder.Entity<BidCluster>()
                .HasRequired(e => e.HostAuction);

            modelBuilder.Entity<BidCluster>()
                .HasMany(e => e.Products);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.BidClusters);

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

            //modelBuilder.Entity<Auction>()
            //    .HasMany(e => e.BidProducts)
            //    .WithRequired(e => e.SelfAuction)
            //    .WillCascadeOnDelete(true);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.BidProducts);

            //modelBuilder.Entity<Product>()
            //    .HasRequired(e => e.SelfAuction)
            //    .WithMany(e => e.BidProducts)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Product>()
            //    .HasMany(e => e.Auctions)
            //    .WithRequired(e => e.SellProduct)
            //    .HasForeignKey(e => e.SellProductId)
            //    .WillCascadeOnDelete(false);

           
        }
    }
}

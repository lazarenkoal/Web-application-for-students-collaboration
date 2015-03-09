namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfsdf : DbMigration
    {
        public override void Up()
        {
            
            
            
            
          
            
            
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Faculty = c.String(),
                        Address = c.String(),
                        Likes = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        University = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(),
                        Message = c.String(),
                        Likes = c.Int(nullable: false),
                        HostHouse_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HostHouse_Id)
                .Index(t => t.HostHouse_Id);
            
            
           
            
            CreateTable(
                "dbo.AuctionHouses",
                c => new
                    {
                        Auction_Id = c.Int(nullable: false),
                        House_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Auction_Id, t.House_Id })
                .ForeignKey("dbo.Auctions", t => t.Auction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Houses", t => t.House_Id, cascadeDelete: true)
                .Index(t => t.Auction_Id)
                .Index(t => t.House_Id);
            
           
            
        }
        
        public override void Down()
        {
           
            DropForeignKey("dbo.AuctionHouses", "House_Id", "dbo.Houses");
            DropForeignKey("dbo.AuctionHouses", "Auction_Id", "dbo.Auctions");
            DropForeignKey("dbo.ForumPosts", "HostHouse_Id", "dbo.Houses");
            
            
         
            DropIndex("dbo.AuctionHouses", new[] { "House_Id" });
            DropIndex("dbo.AuctionHouses", new[] { "Auction_Id" });
         
          
            DropIndex("dbo.ForumPosts", new[] { "HostHouse_Id" });
           
            
   
            DropTable("dbo.AuctionHouses");
           
            
            DropTable("dbo.ForumPosts");
            DropTable("dbo.Houses");
            
            
        }
    }
}

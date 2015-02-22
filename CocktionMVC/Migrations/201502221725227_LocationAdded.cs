namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        IconContent = c.String(),
                        BaloonContent = c.String(),
                        Option = c.String(),
                        University = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductLocations",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Location_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.AuctionLocations",
                c => new
                    {
                        Auction_Id = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Auction_Id, t.Location_Id })
                .ForeignKey("dbo.Auctions", t => t.Auction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.Auction_Id)
                .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuctionLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.AuctionLocations", "Auction_Id", "dbo.Auctions");
            DropForeignKey("dbo.ProductLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.ProductLocations", "Product_Id", "dbo.Products");
            DropIndex("dbo.AuctionLocations", new[] { "Location_Id" });
            DropIndex("dbo.AuctionLocations", new[] { "Auction_Id" });
            DropIndex("dbo.ProductLocations", new[] { "Location_Id" });
            DropIndex("dbo.ProductLocations", new[] { "Product_Id" });
            DropTable("dbo.AuctionLocations");
            DropTable("dbo.ProductLocations");
            DropTable("dbo.Locations");
        }
    }
}

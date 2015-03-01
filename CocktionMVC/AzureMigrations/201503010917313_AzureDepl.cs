namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AzureDepl : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        OwnerId = c.String(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        OwnerName = c.String(nullable: false),
                        WinProductId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        WinnerChosen = c.Boolean(),
                        SellProduct_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToteBoards", t => t.Id)
                .ForeignKey("dbo.Products", t => t.SellProduct_Id)
                .Index(t => t.Id)
                .Index(t => t.SellProduct_Id);
            
            CreateTable(
                "dbo.ToteBoards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        TotalEggs = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToteEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        EggsAmount = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OwnerTote_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToteBoards", t => t.OwnerTote_Id)
                .Index(t => t.OwnerTote_Id);
            
            CreateTable(
                "dbo.ToteResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Profit = c.Int(nullable: false),
                        IsSucсessful = c.Boolean(nullable: false),
                        OwnerTote_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToteBoards", t => t.OwnerTote_Id)
                .Index(t => t.OwnerTote_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Description = c.String(nullable: false, maxLength: 350, fixedLength: true),
                        Category = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        OwnerId = c.String(),
                        Rating = c.Int(nullable: false),
                        Likes = c.Int(nullable: false),
                        OnAuctionTime = c.Int(nullable: false),
                        IsOnAuctionAsALot = c.Boolean(nullable: false),
                        OwnerName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.ThumbnailSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Photo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .Index(t => t.Photo_Id);
            
         
           
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
                "dbo.ProductAuction1",
                c => new
                    {
                        BidAuctions_Id = c.Int(nullable: false),
                        BidProducts_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BidAuctions_Id, t.BidProducts_Id })
                .ForeignKey("dbo.Auctions", t => t.BidAuctions_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.BidProducts_Id, cascadeDelete: true)
                .Index(t => t.BidAuctions_Id)
                .Index(t => t.BidProducts_Id);
            
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
            
            CreateTable(
                "dbo.AuctionProduct2",
                c => new
                    {
                        LidAuctions_Id = c.Int(nullable: false),
                        LidProducts_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LidAuctions_Id, t.LidProducts_Id })
                .ForeignKey("dbo.Auctions", t => t.LidAuctions_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.LidProducts_Id, cascadeDelete: true)
                .Index(t => t.LidAuctions_Id)
                .Index(t => t.LidProducts_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuctionProduct2", "LidProducts_Id", "dbo.Products");
            DropForeignKey("dbo.AuctionProduct2", "LidAuctions_Id", "dbo.Auctions");
            DropForeignKey("dbo.AuctionLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.AuctionLocations", "Auction_Id", "dbo.Auctions");
            DropForeignKey("dbo.ProductAuction1", "BidProducts_Id", "dbo.Products");
            DropForeignKey("dbo.ProductAuction1", "BidAuctions_Id", "dbo.Auctions");
            DropForeignKey("dbo.Photos", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ThumbnailSet", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.ProductLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.ProductLocations", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Auctions", "SellProduct_Id", "dbo.Products");
            DropForeignKey("dbo.Auctions", "Id", "dbo.ToteBoards");
            DropForeignKey("dbo.ToteResults", "OwnerTote_Id", "dbo.ToteBoards");
            DropForeignKey("dbo.ToteEntities", "OwnerTote_Id", "dbo.ToteBoards");
    
            DropIndex("dbo.AuctionProduct2", new[] { "LidProducts_Id" });
            DropIndex("dbo.AuctionProduct2", new[] { "LidAuctions_Id" });
            DropIndex("dbo.AuctionLocations", new[] { "Location_Id" });
            DropIndex("dbo.AuctionLocations", new[] { "Auction_Id" });
            DropIndex("dbo.ProductAuction1", new[] { "BidProducts_Id" });
            DropIndex("dbo.ProductAuction1", new[] { "BidAuctions_Id" });
            DropIndex("dbo.ProductLocations", new[] { "Location_Id" });
            DropIndex("dbo.ProductLocations", new[] { "Product_Id" });
          
            DropIndex("dbo.ThumbnailSet", new[] { "Photo_Id" });
            DropIndex("dbo.Photos", new[] { "Product_Id" });
            DropIndex("dbo.ToteResults", new[] { "OwnerTote_Id" });
            DropIndex("dbo.ToteEntities", new[] { "OwnerTote_Id" });
            DropIndex("dbo.Auctions", new[] { "SellProduct_Id" });
            DropIndex("dbo.Auctions", new[] { "Id" });
          
            DropTable("dbo.AuctionProduct2");
            DropTable("dbo.AuctionLocations");
            DropTable("dbo.ProductAuction1");
            DropTable("dbo.ProductLocations");
          
            DropTable("dbo.ThumbnailSet");
            DropTable("dbo.Photos");
            DropTable("dbo.Locations");
            DropTable("dbo.Products");
            DropTable("dbo.ToteResults");
            DropTable("dbo.ToteEntities");
            DropTable("dbo.ToteBoards");
            DropTable("dbo.Auctions");
  
        }
    }
}

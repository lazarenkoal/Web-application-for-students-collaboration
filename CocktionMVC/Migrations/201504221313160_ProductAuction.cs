namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAuction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductAuction1", "BidAuctions_Id", "dbo.Auctions");
            DropForeignKey("dbo.ProductAuction1", "BidProducts_Id", "dbo.Products");
            DropForeignKey("dbo.Auctions", "SellProductId", "dbo.Products");
            DropIndex("dbo.ProductAuction1", new[] { "BidAuctions_Id" });
            DropIndex("dbo.ProductAuction1", new[] { "BidProducts_Id" });
            AddColumn("dbo.Products", "Auction_Id", c => c.Int());
            CreateIndex("dbo.Products", "Auction_Id");
            AddForeignKey("dbo.Products", "Auction_Id", "dbo.Auctions", "Id");
            AddForeignKey("dbo.Auctions", "SellProductId", "dbo.Products", "Id");
            DropTable("dbo.ProductAuction1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductAuction1",
                c => new
                    {
                        BidAuctions_Id = c.Int(nullable: false),
                        BidProducts_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BidAuctions_Id, t.BidProducts_Id });
            
            DropForeignKey("dbo.Auctions", "SellProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "Auction_Id", "dbo.Auctions");
            DropIndex("dbo.Products", new[] { "Auction_Id" });
            DropColumn("dbo.Products", "Auction_Id");
            CreateIndex("dbo.ProductAuction1", "BidProducts_Id");
            CreateIndex("dbo.ProductAuction1", "BidAuctions_Id");
            AddForeignKey("dbo.Auctions", "SellProductId", "dbo.Products", "Id");
            AddForeignKey("dbo.ProductAuction1", "BidProducts_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.ProductAuction1", "BidAuctions_Id", "dbo.Auctions", "Id");
        }
    }
}

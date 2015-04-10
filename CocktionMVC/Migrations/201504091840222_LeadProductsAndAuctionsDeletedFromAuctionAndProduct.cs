namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeadProductsAndAuctionsDeletedFromAuctionAndProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuctionProduct2", "LidAuctions_Id", "dbo.Auctions");
            DropForeignKey("dbo.AuctionProduct2", "LidProducts_Id", "dbo.Products");
            DropIndex("dbo.AuctionProduct2", new[] { "LidAuctions_Id" });
            DropIndex("dbo.AuctionProduct2", new[] { "LidProducts_Id" });
            DropTable("dbo.AuctionProduct2");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AuctionProduct2",
                c => new
                    {
                        LidAuctions_Id = c.Int(nullable: false),
                        LidProducts_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LidAuctions_Id, t.LidProducts_Id });
            
            CreateIndex("dbo.AuctionProduct2", "LidProducts_Id");
            CreateIndex("dbo.AuctionProduct2", "LidAuctions_Id");
            AddForeignKey("dbo.AuctionProduct2", "LidProducts_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AuctionProduct2", "LidAuctions_Id", "dbo.Auctions", "Id", cascadeDelete: true);
        }
    }
}

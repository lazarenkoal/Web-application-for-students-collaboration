namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeadProductAddedToAuction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "LeadProduct_Id", c => c.Int());
            CreateIndex("dbo.Auctions", "LeadProduct_Id");
            AddForeignKey("dbo.Auctions", "LeadProduct_Id", "dbo.Products", "Id");
            DropColumn("dbo.Auctions", "WinProductId");
            DropColumn("dbo.Auctions", "WinProductName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Auctions", "WinProductName", c => c.String());
            AddColumn("dbo.Auctions", "WinProductId", c => c.String());
            DropForeignKey("dbo.Auctions", "LeadProduct_Id", "dbo.Products");
            DropIndex("dbo.Auctions", new[] { "LeadProduct_Id" });
            DropColumn("dbo.Auctions", "LeadProduct_Id");
        }
    }
}

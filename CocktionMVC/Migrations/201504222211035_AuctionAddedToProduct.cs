namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuctionAddedToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SelfAuction_Id", c => c.Int());
            CreateIndex("dbo.Products", "SelfAuction_Id");
            AddForeignKey("dbo.Products", "SelfAuction_Id", "dbo.Auctions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SelfAuction_Id", "dbo.Auctions");
            DropIndex("dbo.Products", new[] { "SelfAuction_Id" });
            DropColumn("dbo.Products", "SelfAuction_Id");
        }
    }
}

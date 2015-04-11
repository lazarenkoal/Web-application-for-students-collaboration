namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unusedFieldsDeleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Auctions", "SelfOwnerId");
            DropColumn("dbo.Auctions", "OwnerName");
            DropColumn("dbo.Products", "Rating");
            DropColumn("dbo.Products", "Likes");
            DropColumn("dbo.Products", "OnAuctionTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "OnAuctionTime", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Likes", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Auctions", "OwnerName", c => c.String());
            AddColumn("dbo.Auctions", "SelfOwnerId", c => c.String());
        }
    }
}

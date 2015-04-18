namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingAddedToAuction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Auctions", "Rating");
        }
    }
}

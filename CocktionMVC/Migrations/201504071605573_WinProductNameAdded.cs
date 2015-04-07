namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WinProductNameAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "WinProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Auctions", "WinProductName");
        }
    }
}

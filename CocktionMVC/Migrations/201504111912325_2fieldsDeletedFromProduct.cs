namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2fieldsDeletedFromProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auctions", "SelfOwnerId", c => c.String());
            AlterColumn("dbo.Auctions", "OwnerName", c => c.String());
            DropColumn("dbo.Products", "SelfOwnerId");
            DropColumn("dbo.Products", "OwnerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "OwnerName", c => c.String());
            AddColumn("dbo.Products", "SelfOwnerId", c => c.String());
            AlterColumn("dbo.Auctions", "OwnerName", c => c.String(nullable: false));
            AlterColumn("dbo.Auctions", "SelfOwnerId", c => c.String(nullable: false));
        }
    }
}

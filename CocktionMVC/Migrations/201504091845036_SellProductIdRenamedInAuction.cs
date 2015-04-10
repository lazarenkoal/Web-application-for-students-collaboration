namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SellProductIdRenamedInAuction : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Auctions", name: "SellProduct_Id", newName: "SellProductId");
            RenameIndex(table: "dbo.Auctions", name: "IX_SellProduct_Id", newName: "IX_SellProductId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Auctions", name: "IX_SellProductId", newName: "IX_SellProduct_Id");
            RenameColumn(table: "dbo.Auctions", name: "SellProductId", newName: "SellProduct_Id");
        }
    }
}

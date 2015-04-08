namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class winnerChosenUnNullableNow : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auctions", "WinnerChosen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Auctions", "WinnerChosen", c => c.Boolean());
        }
    }
}

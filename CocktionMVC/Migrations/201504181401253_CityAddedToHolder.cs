namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityAddedToHolder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseHolders", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HouseHolders", "City");
        }
    }
}

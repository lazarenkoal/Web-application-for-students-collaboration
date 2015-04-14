namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HouseEddited : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Houses", "University");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Houses", "University", c => c.String());
        }
    }
}

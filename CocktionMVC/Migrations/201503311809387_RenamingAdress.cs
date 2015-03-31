namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingAdress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "Adress", c => c.String());
            DropColumn("dbo.Houses", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Houses", "Address", c => c.String());
            DropColumn("dbo.Houses", "Adress");
        }
    }
}

namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryToGetBack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "University", c => c.String());
            AddColumn("dbo.AspNetUsers", "Dormitory", c => c.String());
            AddColumn("dbo.AspNetUsers", "StudyAdress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Faculty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Faculty");
            DropColumn("dbo.AspNetUsers", "StudyAdress");
            DropColumn("dbo.AspNetUsers", "Dormitory");
            DropColumn("dbo.AspNetUsers", "University");
        }
    }
}

namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "University");
            DropColumn("dbo.AspNetUsers", "Dormitory");
            DropColumn("dbo.AspNetUsers", "StudyAdress");
            DropColumn("dbo.AspNetUsers", "Faculty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Faculty", c => c.String());
            AddColumn("dbo.AspNetUsers", "StudyAdress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Dormitory", c => c.String());
            AddColumn("dbo.AspNetUsers", "University", c => c.String());
        }
    }
}

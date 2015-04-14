namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersFriends : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AspNetUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "AspNetUser_Id");
            AddForeignKey("dbo.AspNetUsers", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "AspNetUser_Id" });
            DropColumn("dbo.AspNetUsers", "AspNetUser_Id");
        }
    }
}

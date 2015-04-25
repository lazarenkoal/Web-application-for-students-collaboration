namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebiceAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "MobileDevice_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "MobileDevice_Id");
            AddForeignKey("dbo.AspNetUsers", "MobileDevice_Id", "dbo.Devices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "MobileDevice_Id", "dbo.Devices");
            DropIndex("dbo.AspNetUsers", new[] { "MobileDevice_Id" });
            DropColumn("dbo.AspNetUsers", "MobileDevice_Id");
            DropTable("dbo.Devices");
        }
    }
}

namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CncntBtwnUserAndHous : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUserHouses",
                c => new
                    {
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                        House_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AspNetUser_Id, t.House_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Houses", t => t.House_Id, cascadeDelete: true)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.House_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserHouses", "House_Id", "dbo.Houses");
            DropForeignKey("dbo.AspNetUserHouses", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserHouses", new[] { "House_Id" });
            DropIndex("dbo.AspNetUserHouses", new[] { "AspNetUser_Id" });
            DropTable("dbo.AspNetUserHouses");
        }
    }
}

namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestsAndHouseSubscriptionsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Photocard_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photocard_Id)
                .Index(t => t.Photocard_Id);
            
            CreateTable(
                "dbo.AspNetUserInterests",
                c => new
                    {
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                        Interest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AspNetUser_Id, t.Interest_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Interests", t => t.Interest_Id, cascadeDelete: true)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.Interest_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserInterests", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.AspNetUserInterests", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interests", "Photocard_Id", "dbo.Photos");
            DropIndex("dbo.AspNetUserInterests", new[] { "Interest_Id" });
            DropIndex("dbo.AspNetUserInterests", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Interests", new[] { "Photocard_Id" });
            DropTable("dbo.AspNetUserInterests");
            DropTable("dbo.Interests");
        }
    }
}

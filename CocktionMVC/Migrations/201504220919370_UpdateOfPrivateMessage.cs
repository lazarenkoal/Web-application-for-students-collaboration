namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOfPrivateMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PrivateMessages", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PrivateMessages", new[] { "AspNetUser_Id" });
            CreateTable(
                "dbo.AspNetUserPrivateMessages",
                c => new
                    {
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                        PrivateMessage_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AspNetUser_Id, t.PrivateMessage_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.PrivateMessages", t => t.PrivateMessage_Id, cascadeDelete: true)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.PrivateMessage_Id);
            
            DropColumn("dbo.PrivateMessages", "AspNetUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivateMessages", "AspNetUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUserPrivateMessages", "PrivateMessage_Id", "dbo.PrivateMessages");
            DropForeignKey("dbo.AspNetUserPrivateMessages", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserPrivateMessages", new[] { "PrivateMessage_Id" });
            DropIndex("dbo.AspNetUserPrivateMessages", new[] { "AspNetUser_Id" });
            DropTable("dbo.AspNetUserPrivateMessages");
            CreateIndex("dbo.PrivateMessages", "AspNetUser_Id");
            AddForeignKey("dbo.PrivateMessages", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

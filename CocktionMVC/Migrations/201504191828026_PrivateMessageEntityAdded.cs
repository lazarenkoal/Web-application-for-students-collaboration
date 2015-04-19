namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateMessageEntityAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrivateMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        DateOfPublishing = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Receiver_Id = c.String(maxLength: 128),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Receiver_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateMessages", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateMessages", "Receiver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateMessages", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PrivateMessages", new[] { "AspNetUser_Id" });
            DropIndex("dbo.PrivateMessages", new[] { "Receiver_Id" });
            DropIndex("dbo.PrivateMessages", new[] { "Author_Id" });
            DropTable("dbo.PrivateMessages");
        }
    }
}

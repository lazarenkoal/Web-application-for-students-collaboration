namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingEasierMessageModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PrivateMessages", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateMessages", "Receiver_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PrivateMessages", new[] { "Author_Id" });
            DropIndex("dbo.PrivateMessages", new[] { "Receiver_Id" });
            AddColumn("dbo.PrivateMessages", "ReceiverName", c => c.String());
            AddColumn("dbo.PrivateMessages", "AuthorName", c => c.String());
            DropColumn("dbo.PrivateMessages", "Author_Id");
            DropColumn("dbo.PrivateMessages", "Receiver_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivateMessages", "Receiver_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PrivateMessages", "Author_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.PrivateMessages", "AuthorName");
            DropColumn("dbo.PrivateMessages", "ReceiverName");
            CreateIndex("dbo.PrivateMessages", "Receiver_Id");
            CreateIndex("dbo.PrivateMessages", "Author_Id");
            AddForeignKey("dbo.PrivateMessages", "Receiver_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PrivateMessages", "Author_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCustomisedAndHouseHolderAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HouseHolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhotoCard_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.PhotoCard_Id)
                .Index(t => t.PhotoCard_Id);
            
            AddColumn("dbo.AspNetUsers", "HisHouseHolder_Id", c => c.Int());
            AddColumn("dbo.Houses", "Holder_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "HisHouseHolder_Id");
            CreateIndex("dbo.Houses", "Holder_Id");
            AddForeignKey("dbo.Houses", "Holder_Id", "dbo.HouseHolders", "Id");
            AddForeignKey("dbo.AspNetUsers", "HisHouseHolder_Id", "dbo.HouseHolders", "Id");
            DropColumn("dbo.AspNetUsers", "University");
            DropColumn("dbo.AspNetUsers", "Faculty");
            DropColumn("dbo.AspNetUsers", "Dormitory");
            DropColumn("dbo.AspNetUsers", "StudyAdress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "StudyAdress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Dormitory", c => c.String());
            AddColumn("dbo.AspNetUsers", "Faculty", c => c.String());
            AddColumn("dbo.AspNetUsers", "University", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "HisHouseHolder_Id", "dbo.HouseHolders");
            DropForeignKey("dbo.HouseHolders", "PhotoCard_Id", "dbo.Photos");
            DropForeignKey("dbo.Houses", "Holder_Id", "dbo.HouseHolders");
            DropIndex("dbo.HouseHolders", new[] { "PhotoCard_Id" });
            DropIndex("dbo.Houses", new[] { "Holder_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "HisHouseHolder_Id" });
            DropColumn("dbo.Houses", "Holder_Id");
            DropColumn("dbo.AspNetUsers", "HisHouseHolder_Id");
            DropTable("dbo.HouseHolders");
        }
    }
}

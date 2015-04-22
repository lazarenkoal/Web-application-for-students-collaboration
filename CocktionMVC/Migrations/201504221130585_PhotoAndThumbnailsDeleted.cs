namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoAndThumbnailsDeleted : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ThumbnailSet", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Product_Id1", "dbo.Products");
            DropIndex("dbo.Photos", new[] { "Product_Id1" });
            DropIndex("dbo.ThumbnailSet", new[] { "Photo_Id" });
            AddColumn("dbo.Products", "Photo_Id", c => c.Int());
            CreateIndex("dbo.Products", "Photo_Id");
            AddForeignKey("dbo.Products", "Photo_Id", "dbo.Pictures", "Id");
            //DropTable("dbo.Photos");
            DropTable("dbo.ThumbnailSet");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ThumbnailSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Photo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Product_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Products", "Photo_Id", "dbo.Pictures");
            DropIndex("dbo.Products", new[] { "Photo_Id" });
            DropColumn("dbo.Products", "Photo_Id");
            CreateIndex("dbo.ThumbnailSet", "Photo_Id");
            CreateIndex("dbo.Photos", "Product_Id1");
            AddForeignKey("dbo.Photos", "Product_Id1", "dbo.Products", "Id");
            AddForeignKey("dbo.ThumbnailSet", "Photo_Id", "dbo.Photos", "Id");
        }
    }
}

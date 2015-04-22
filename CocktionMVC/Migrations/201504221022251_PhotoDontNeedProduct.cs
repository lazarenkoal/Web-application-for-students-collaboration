namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoDontNeedProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "Product_Id", "dbo.Products");
            DropIndex("dbo.Photos", new[] { "Product_Id" });
            AddColumn("dbo.Photos", "Product_Id1", c => c.Int());
            CreateIndex("dbo.Photos", "Product_Id1");
            AddForeignKey("dbo.Photos", "Product_Id1", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "Product_Id1", "dbo.Products");
            DropIndex("dbo.Photos", new[] { "Product_Id1" });
            DropColumn("dbo.Photos", "Product_Id1");
            CreateIndex("dbo.Photos", "Product_Id");
            AddForeignKey("dbo.Photos", "Product_Id", "dbo.Products", "Id");
        }
    }
}

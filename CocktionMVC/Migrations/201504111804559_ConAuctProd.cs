namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConAuctProd : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BidClusterProducts", newName: "ProductBidClusters");
            DropPrimaryKey("dbo.ProductBidClusters");
            AddColumn("dbo.Auctions", "SelfOwnerId", c => c.String(nullable: false));
            AddColumn("dbo.Auctions", "Owner_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "SelfOwnerId", c => c.String());
            AddColumn("dbo.Products", "Owner_Id", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.ProductBidClusters", new[] { "Product_Id", "BidCluster_Id" });
            CreateIndex("dbo.Auctions", "Owner_Id");
            CreateIndex("dbo.Products", "Owner_Id");
            AddForeignKey("dbo.Auctions", "Owner_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Products", "Owner_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Auctions", "OwnerId");
            DropColumn("dbo.Products", "OwnerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "OwnerId", c => c.String());
            AddColumn("dbo.Auctions", "OwnerId", c => c.String(nullable: false));
            DropForeignKey("dbo.Products", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Auctions", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "Owner_Id" });
            DropIndex("dbo.Auctions", new[] { "Owner_Id" });
            DropPrimaryKey("dbo.ProductBidClusters");
            DropColumn("dbo.Products", "Owner_Id");
            DropColumn("dbo.Products", "SelfOwnerId");
            DropColumn("dbo.Auctions", "Owner_Id");
            DropColumn("dbo.Auctions", "SelfOwnerId");
            AddPrimaryKey("dbo.ProductBidClusters", new[] { "BidCluster_Id", "Product_Id" });
            RenameTable(name: "dbo.ProductBidClusters", newName: "BidClusterProducts");
        }
    }
}

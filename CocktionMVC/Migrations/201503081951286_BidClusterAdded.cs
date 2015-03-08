namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BidClusterAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BidClusters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        HostAuction_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.HostAuction_Id, cascadeDelete: true)
                .Index(t => t.HostAuction_Id);
            
            CreateTable(
                "dbo.BidClusterProducts",
                c => new
                    {
                        BidCluster_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BidCluster_Id, t.Product_Id })
                .ForeignKey("dbo.BidClusters", t => t.BidCluster_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.BidCluster_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BidClusterProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.BidClusterProducts", "BidCluster_Id", "dbo.BidClusters");
            DropForeignKey("dbo.BidClusters", "HostAuction_Id", "dbo.Auctions");
            DropIndex("dbo.BidClusterProducts", new[] { "Product_Id" });
            DropIndex("dbo.BidClusterProducts", new[] { "BidCluster_Id" });
            DropIndex("dbo.BidClusters", new[] { "HostAuction_Id" });
            DropTable("dbo.BidClusterProducts");
            DropTable("dbo.BidClusters");
        }
    }
}

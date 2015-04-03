namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedBackAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersFeedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorsName = c.String(),
                        AuthorsSurname = c.String(),
                        AuthorsId = c.String(),
                        UsersId = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersFeedbacks");
        }
    }
}

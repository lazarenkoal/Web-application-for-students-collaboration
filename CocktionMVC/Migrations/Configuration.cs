namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CocktionMVC.Models.DAL;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using CocktionMVC.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<CocktionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CocktionMVC.Models.DAL.CocktionContext";
            
        }

        
        protected override void Seed(CocktionContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Houses.AddOrUpdate(new House
            {
                Address = "Кочновский проезд, 3",
                Faculty = "ФКН",
                University = "ВШЭ",
                Likes = 0,
                Rating = 0
            });
            context.SaveChanges();
        }
    }
}

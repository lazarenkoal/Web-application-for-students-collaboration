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
    using System.Web;
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

            //context.Houses.AddOrUpdate(new House
            //{
            //    Adress = "Кочновский проезд, 3",
            //    Faculty = "ФКН",
            //    University = "НИУ ВШЭ",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Adress = "Мясницкая, 20",
            //    Faculty = "ГМУ",
            //    University = "НИУ ВШЭ",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Adress = "Кирпичная, 33",
            //    Faculty = "ФБМ",
            //    University = "НИУ ВШЭ",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Adress = "Одинцово, ул.Маковского, 2",
            //    Faculty = "Общага Трилистник (8-ка)",
            //    University = "НИУ ВШЭ",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Adress = "ВНИИСОК, Дениса Давыдова",
            //    Faculty = "Общага Дубки (7 и 9-ка)",
            //    University = "НИУ ВШЭ",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();
        }
    }
}

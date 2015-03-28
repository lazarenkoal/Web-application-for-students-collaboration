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
            //    Address = "���������� ������, 3",
            //    Faculty = "���",
            //    University = "��� ���",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Address = "���������, 20",
            //    Faculty = "���",
            //    University = "��� ���",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Address = "���������, 33",
            //    Faculty = "���",
            //    University = "��� ���",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Address = "��������, ��.����������, 2",
            //    Faculty = "������ ���������� (8-��)",
            //    University = "��� ���",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();

            //context.Houses.AddOrUpdate(new House
            //{
            //    Address = "�������, ������ ��������",
            //    Faculty = "������ ����� (7 � 9-��)",
            //    University = "��� ���",
            //    Likes = 0,
            //    Rating = 0
            //});
            //context.SaveChanges();
        }
    }
}

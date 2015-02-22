namespace CocktionMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CocktionMVC.Models.DAL;
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

            context.Locations.AddOrUpdate(new Location
                {
                    BaloonContent = "Мясо 20",
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Latitude = 55.76132639,
                    Longitude = 37.63286275,
                    Option = "twirl#blueStretchyIcon"
                },
                new Location
                {
                    BaloonContent = "Кирпич",
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Latitude = 55.77880371,
                    Longitude = 37.73312906,
                    Option = "twirl#blueStretchyIcon"
                },
                new Location
                {
                    BaloonContent = "Малый Гнездниковский",
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    Latitude = 55.76176414,
                    Longitude = 37.60605156
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Петровка 12/1",
                    Latitude = 55.76279902,
                    Longitude = 37.61806495
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Ильинка 13/1",
                    Latitude = 55.75594898,
                    Longitude = 37.62784075
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Мясо 18",
                    Latitude = 55.76114173,
                    Longitude = 37.63264935
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Мясо 11",
                    Latitude = 55.76178614,
                    Longitude = 37.63270182
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Покровка 31/3",
                    Latitude = 55.76099578,
                    Longitude = 37.64902250
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Славянская площадь 4/2",
                    Latitude = 55.75343778,
                    Longitude = 37.63626650
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Хитровский 2/8",
                    Latitude = 55.75355478,
                    Longitude = 37.64477350
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Хитровский 4/10",
                    Latitude = 55.75385878,
                    Longitude = 37.64551900
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Малый трехсвят 8/2",
                    Latitude = 55.75409178,
                    Longitude = 37.64655200
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Большой трехсвят 3",
                    Latitude = 55.75536778,
                    Longitude = 37.64647150
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Малая Ордынка 17",
                    Latitude = 55.73751178,
                    Longitude = 37.62630400
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Малая пионерская 12",
                    Latitude = 55.72835778,
                    Longitude = 37.63517050
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Шабаловка 26",
                    Latitude = 55.72058578,
                    Longitude = 37.60922700
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Шаболовка 31",
                    Latitude = 55.72108228,
                    Longitude = 37.61093350
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Мытная 46/5",
                    Latitude = 55.72048878,
                    Longitude = 37.61545250
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Волгоградский проспект",
                    Latitude = 55.70375778,
                    Longitude = 37.72613350
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Варшавское шоссе",
                    Latitude = 55.67458528,
                    Longitude = 37.62483050
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Таллинская 34",
                    Latitude = 55.80313478,
                    Longitude = 37.41045650
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Кочновский проезд",
                    Latitude = 55.80666128,
                    Longitude = 37.54171850
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, Электродная 1",
                    Latitude = 55.75592528,
                    Longitude = 37.75233750
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, Энергетическая 10",
                    Latitude = 55.75926328,
                    Longitude = 37.70502350
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, Студенческая 33",
                    Latitude = 55.73909278,
                    Longitude = 37.54510550
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, Трилистник",
                    Latitude = 55.66985378,
                    Longitude = 37.27968000
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, шестерка",
                    Latitude = 55.66719328,
                    Longitude = 37.28281550
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, девятка - 1",
                    Latitude = 55.65966778,
                    Longitude = 37.22837750
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, девятка - 2",
                    Latitude = 55.65962228,
                    Longitude = 37.22607750
                },
                new Location
                {
                    IconContent = "Вышка",
                    University = "ВШЭ",
                    Option = "twirl#blueStretchyIcon",
                    BaloonContent = "Общага, семерка",
                    Latitude = 55.66040378,
                    Longitude = 37.22888950
                });
        }
    }
}

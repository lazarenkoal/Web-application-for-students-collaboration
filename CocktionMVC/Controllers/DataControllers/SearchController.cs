using System;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Controllers.DataControllers
{
    /// <summary>
    /// Содержит методы, используемые для поиска
    /// университетов, аукционов и т.д.
    /// </summary>
    public class SearchController : Controller
    {
        /// <summary>
        /// Контейнер для информации, которую поиск
        /// хочет рассказать пользователю.
        /// </summary>
        class SearchResults
        {
             public string[] Names { get; set; }
             public bool IsSearchEmpty { get; set; }

            public SearchResults(string[] names, bool isSearchEmpty)
            {
                Names = names;
                IsSearchEmpty = isSearchEmpty;
            }
        }

        /// <summary>
        /// Позволяет искать университет
        /// </summary>
        /// <returns>Ответ с информацией о том, нашел или нет.</returns>
        [HttpPost]
        [Authorize]
        public JsonResult SearchUniversity()
        {
            string searchLine = Request.Form.GetValues("university")[0];
            CocktionContext db = new CocktionContext();
            string[] names = (from x in db.HouseHolders
                where x.Name.Contains(searchLine)
                select x.Name).ToArray();
            if (names.Length == 0)
            {
                return Json(new SearchResults(null, true));
            }
            return Json(new SearchResults(names, false));
        }

        /// <summary>
        /// Контейнер для выдачи поиском информации о 
        /// факультете.
        /// </summary>
        class FacultyList
        {
            public string[] Names { get; set; }
            public int[] Ids { get; set; }

            public bool IsEmpty { get; set; }
            public FacultyList(string[] names, int[] ids, bool isEmpty)
            {
                Names = names;
                Ids = ids;
                IsEmpty = isEmpty;
            }

        }

        /// <summary>
        /// Позволяет получить список факультетов, которые находятся
        /// в данном вузе
        /// </summary>
        /// <returns>Джейсон с факультетами</returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetFacultyList()
        {
            string university = Request.Form.GetValues("university")[0];
            CocktionContext db = new CocktionContext();
            House[] houses = (from x in db.Houses
                where x.Holder.Name == university
                select x).ToArray();
            if (houses.Length == 0)
            {
                return Json(new FacultyList(null, null, true));
            }
            else
            {
                string[] names = new string[houses.Length];
                int[] ids = new int[houses.Length];
                for (int i = 0; i < houses.Length; i++)
                {
                    names[i] = houses[i].Faculty;
                    ids[i] = houses[i].Id;
                }
                return Json(new FacultyList(names, ids, false));
            }
        }

        /// <summary>
        /// Главная поисковая страница
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ищет по всем-всем аукционам
        /// </summary>
        /// <returns>Аукционы, найденные по строке поиска</returns>
        public JsonResult SearchEverywhere()
        {
            string searchString = Request.Form.GetValues("searchString")[0];
            
            CocktionContext db = new CocktionContext();
            var auS = (from x in db.Auctions
                where x.IsActive & x.SellProduct.Name.Contains(searchString)
                select x).ToArray();
            Info[] auctions = new Info[auS.Length];
            int i = 0;
            Array.ForEach(auS, x =>
            {
                auctions[i++] = new Info(x.SellProduct.Name, x.Id, x.SellProduct.Photo.FileName,
                    x.SellProduct.Description);
            });
            if (auctions.Length == 0)
            {
                return Json(new GlobalSearchResults(null, true));
            }
            return Json(new GlobalSearchResults(auctions, false));
        }

        /// <summary>
        /// Контейнер для информации об аукционе,
        /// который находится поиском
        /// </summary>
        public class Info
        {
            public Info(string name, int id, string photo, string description)
            {
                Name = name;
                Id = id;
                Photo = photo;
                Description = description;
            }
            public string Name { get; set; }
            public int Id { get; set; }
            public string Photo { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// Контейнер для результатов поиска по всем аукционам
        /// </summary>
        public class GlobalSearchResults
        {
            public GlobalSearchResults(Info[] auctions, bool isEmpty)
            {
                Auctions = auctions;
                IsEmpty = isEmpty;
            }
            public Info[] Auctions { get; set; }
            public bool IsEmpty { get; set; }
        }
    }
}
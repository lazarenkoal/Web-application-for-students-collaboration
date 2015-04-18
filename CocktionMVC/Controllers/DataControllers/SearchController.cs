using System;
using System.Linq;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Controllers.DataControllers
{
    public class SearchController : Controller
    {
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

        public ActionResult Index()
        {
            return View();
        }

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
                auctions[i++] = new Info(x.SellProduct.Name, x.Id, x.SellProduct.Photos.First().FileName,
                    x.SellProduct.Description);
            });
            if (auctions.Length == 0)
            {
                return Json(new GlobalSearchResults(null, true));
            }
            return Json(new GlobalSearchResults(auctions, false));
        }

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
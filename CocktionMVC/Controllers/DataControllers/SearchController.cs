using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
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
    }
}
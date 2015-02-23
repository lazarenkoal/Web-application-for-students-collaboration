using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.DAL;
namespace CocktionMVC.Controllers
{
    public class PlacemarkController : Controller
    {
        [HttpPost]
        public JsonResult GetAllPlacemarks()
        {
            CocktionContext db = new CocktionContext();
            List<Location> locations = (from x in db.Locations
                                        select x).ToList<Location>();
            List<LocationInfo> jsonLocations = new List<LocationInfo>();
            foreach (var location in locations)
            {
                LocationInfo placemark = new LocationInfo(location.Latitude, location.Longitude, location.IconContent,
                                                          location.BaloonContent, location.Option);
                jsonLocations.Add(placemark);
            }

            return Json(jsonLocations);
        }
    }
}
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
        /// <summary>
        /// Метод достает из базы данных все локации, которые есть, 
        /// приводит их в удобочитаемый вид для карт.
        /// </summary>
        /// <returns>Коллекцию с информацией о локациях</returns>
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

        [HttpPost]
        public JsonResult GetCurrentPlacemark()
        {
            int placemarkId = int.Parse(Request.Form.GetValues("locationId")[0]);
            CocktionContext db = new CocktionContext();
            Location location = db.Locations.Find(placemarkId);
            LocationInfo placemark = new LocationInfo(location.Latitude, location.Longitude, location.IconContent,
                                                          location.BaloonContent, location.Option);
            return Json(placemark);
        }
    }
}
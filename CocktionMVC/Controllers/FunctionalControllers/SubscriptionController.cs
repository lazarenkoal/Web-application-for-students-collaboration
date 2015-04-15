﻿using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.FunctionalControllers
{
    public class SubscriptionController : Controller
    {
        [HttpPost]
        [Authorize]
        public JsonResult SubscribeOnHouse()
        {
            var strings = Request.Form.GetValues("houseId");
            if (strings != null)
            {
                int houseId = int.Parse(strings[0]);
                CocktionContext db = new CocktionContext();
                var house = db.Houses.Find(houseId);
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                user.SubHouses.Add(house);
                house.Inhabitants.Add(user);
                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            return Json(new StatusHolder(false));
        }

        [HttpPost]
        [Authorize]
        public JsonResult CheckHouseSubscription()
        {
            var strings = Request.Form.GetValues("modelId");
            if (strings != null)
            {
                int houseId = int.Parse(strings[0]);
                CocktionContext db = new CocktionContext();
                var house = db.Houses.Find(houseId);
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (user.SubHouses.Contains(house))
                {
                    return Json(new StatusHolder(true));
                }
                return Json(new StatusHolder(false));
            }
            return Json(new StatusHolder(false));
        }
    }
}
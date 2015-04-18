using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;

namespace CocktionMVC.Controllers.AdminControllers
{
    public class EditInterestsController : Controller
    {
        // GET: EditInterests
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.Name == "darya-coo@cocktion.com" || User.Identity.Name == "lazarenko.ale@gmail.com")
            {
                CocktionContext db = new CocktionContext();
                return View(db.Interests.ToList());
            }
            return View("PageNotFound");
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddInterest()
        {
            try
            {
                string name = Request.Form.GetValues("name")[0].Trim();

                Picture photo = new Picture();

                PhotoProcessor.CreateAndSavePicture(photo, Request, 200);

                CocktionContext db = new CocktionContext();
                db.Pictures.Add(photo);
                db.Interests.Add(new Interest(name, photo));

                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            catch (Exception q)
            {
                return Json(new EditHousesController.Q { Status = q.InnerException.InnerException.Message });
            }

        }
    }
}
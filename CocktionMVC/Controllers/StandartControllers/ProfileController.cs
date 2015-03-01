using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
namespace CocktionMVC.Controllers.StandartControllers
{
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Profile
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                CocktionContext db = new CocktionContext();
                string currentUserId = User.Identity.GetUserId();
                var currentUser = UserManager.FindById(currentUserId);
                int eggsAmount = currentUser.Eggs;
                int auctionsAmount = db.Auctions.Count(x => x.OwnerId == currentUserId);
                int productsAmount = db.Products.Count(x => x.OwnerId == currentUserId);
                ProfileViewModel model = new ProfileViewModel(eggsAmount, auctionsAmount, productsAmount);
                return View(model);
            }
            else
            {
                return RedirectToAction("HowItCouldBe");
            }
        }

        public ActionResult HowItCouldBe()
        {
            return View();
        }
    }
}
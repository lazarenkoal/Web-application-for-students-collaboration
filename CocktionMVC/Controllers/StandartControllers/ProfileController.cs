using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

        /// <summary>
        /// Выводит пользователя на страничку с информацией о его аккаунте
        /// </summary>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {//если пользватель авторизован
                CocktionContext db = new CocktionContext();

                //опознаем пользователя
                string currentUserId = User.Identity.GetUserId();
                var currentUser = UserManager.FindById(currentUserId);

                //Собираем статистику
                int eggsAmount = currentUser.Eggs;
                int auctionsAmount = db.Auctions.Count(x => x.OwnerId == currentUserId);
                int productsAmount = db.Products.Count(x => x.OwnerId == currentUserId);

                //делаем отчет
                ProfileViewModel model = new ProfileViewModel(eggsAmount, auctionsAmount, productsAmount);
                return View(model);
            }
            else
            {//если пользователь не авторизован
                return RedirectToAction("HowItCouldBe");
            }
        }

        /// <summary>
        /// Показывает пользователю то, какой могла бы быть его страничка
        /// </summary>
        public ActionResult HowItCouldBe()
        {
            //TODO сделать эту страниу
            return View();
        }

     }
}
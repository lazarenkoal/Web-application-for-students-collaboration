using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels.MobileClientModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class ProfileController : ApiController
    {
        public class ProfileInfo
        {
            public string profileId { get; set; }
        }
        [HttpPost]
        [Authorize]
        public ProfileInfoMobile GetInfo(ProfileInfo profInf)
        {
            CocktionContext db = new CocktionContext();
            if (profInf.profileId == "self")
            {
                string userId = User.Identity.GetUserId();
                var userProfile = db.AspNetUsers.Find(userId);
                ProfileInfoMobile info = new ProfileInfoMobile(@"http://cocktion.com/Content/SiteImages/anonPhoto.jpg",
                    userProfile.UserRealName, userProfile.UserRealSurname, userProfile.Eggs, 100, "Шлюхи",
                    @"http://cocktion.com/Content/SiteImages/girl.jpg", "Тачки", @"http://cocktion.com/Content/SiteImages/car.jpg",
                    "Блэкджек", @"http://cocktion.com/Content/SiteImages/blackjack.jpg", "Бабло", @"http://cocktion.com/Content/SiteImages/money.jpg");
                return info;
            }
            else
            {
                var userProfile = db.AspNetUsers.Find(profInf.profileId);
                ProfileInfoMobile info = new ProfileInfoMobile(@"http://cocktion.com/Content/SiteImages/anonPhoto.jpg",
                    userProfile.UserRealName, userProfile.UserRealSurname, userProfile.Eggs, 100, "Шлюхи",
                    @"http://cocktion.com/Content/SiteImages/girl.jpg", "Тачки", @"http://cocktion.com/Content/SiteImages/car.jpg",
                    "Блэкджек", @"http://cocktion.com/Content/SiteImages/blackjack.jpg", "Бабло", @"http://cocktion.com/Content/SiteImages/money.jpg");
                return info;
            }
        }
    }
}

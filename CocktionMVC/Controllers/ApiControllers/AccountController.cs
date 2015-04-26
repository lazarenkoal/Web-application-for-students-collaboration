using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CocktionMVC.Models;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.MobileClientModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.Identity;
namespace CocktionMVC.Controllers.ApiControllers
{
    public class AccountController : ApiController
    {
        
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }



        /// <summary>
        /// Регистрирует пользователя с мобильного клиента
        /// </summary>
        /// <param name="data">Данные для регистрации</param>
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        public async Task<StatusHolder> Registrate(LoginCredentials data)
        {
            var user = new ApplicationUser
            {
                UserName = data.email,
                Email = data.email,
                Rating = 1000,
                Eggs = 100
            };
            var resultOfRegistration = await UserManager.CreateAsync(user, data.password);
            if (resultOfRegistration.Succeeded)
            {
                return new StatusHolderWithError(true, null);
            }
            return new StatusHolderWithError(false, "Такой пользователь уже зарегистрирован");
        }

        /// <summary>
        /// Производит аутентификацию пользователя через логин пароль
        /// и возвращает мобильнику его токен
        /// </summary>
        /// <param name="model">Модель с логином и паролем</param>
        /// <returns>Токен для авторизации последующей</returns>
        [HttpPost]
        public TokenResponse Authenticate(LoginCredentials model)
        {
            //полчуаем информацию о пользоватлее
            string user = model.email, password = model.password;

            //Если чего-то нет (логин или парль) авторизация невозможна
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return new TokenResponse { Token = "Failure" };
            
            //Ищем пользователя с таким паролем и логином
            var userIdentity = UserManager.FindAsync(user, password).Result;

            if (userIdentity != null)
            { //если пользователь нашелся
                //проверяем все и возвращаем токен
                var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id));

                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(100));

                string accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
                return new TokenResponse { Token = accessToken };
            }

            //если не нашелся - возвращаем ошибку
            return new TokenResponse { Token = "Failure" };
        }

        public class DeviceInfo
        {
            public string type { get; set; }
            public string token { get; set; }
        }

        [HttpPost]
        [Authorize]
        public StatusHolder AddDevice(DeviceInfo device)
        {
            try
            {
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                switch(device.type)
                {
                    case "ios":
                        user.MobileDevice = new Device(device.type, device.token);
                        break;
                    case "android":
                        //do smth here
                        break;
                    case "wp":
                        //do smth here
                        break;
                }
                db.SaveChanges();
                return new StatusHolder(true);
            }
            catch
            {
                return new StatusHolder(false);
            }
        }
    }
}

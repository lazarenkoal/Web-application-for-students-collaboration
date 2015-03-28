using System;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using CocktionMVC.Models.JsonModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

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
        /// Производит аутентификацию пользователя через логин пароль
        /// и возвращает мобильнику его токен
        /// </summary>
        /// <param name="model">Модель с логином и паролем</param>
        /// <returns>Токен для авторизации последующей</returns>
        [HttpPost]
        public TokenResponse Authenticate(MobileUserLogin model)
        {
            //полчуаем информацию о пользоватлее
            string user = model.Email, password = model.Password;

            //Если чего-то нет (логин или парль) авторизация невозможна
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return new TokenResponse { Token = "failed" };
            
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
            return new TokenResponse { Token = "failed" };
        }
    }
}

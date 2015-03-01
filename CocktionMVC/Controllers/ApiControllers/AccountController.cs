using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Web;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.Owin.Infrastructure;
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
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        public Response Authenticate(UserToLogin model)
        {
            string user = model.Email, password = model.Password;
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return new Response { Token = "failed" };
            
            var userIdentity = UserManager.FindAsync(user, password).Result;

            if (userIdentity != null)
            {
                var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id));
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
                string AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
                return new Response { Token = AccessToken };
            }
            return new Response { Token = "failed" };
        }

        [Authorize]
        [HttpGet]
        [ActionName("ValidateToken")]
        public Respond ValidateToken()
        {
            var user = this.User.Identity;
            if (user != null)
                return new Respond { Status = string.Format("{0} - {1}", user.GetUserId(), user.GetUserName()) };
            else
                return new Respond { Status = "Unable to resolve user id" };

        }
    }
    

  public class Response
  {
      public string Token { get; set; }
  }

  public class Respond
  {
      public string Status { get; set; }
  }

    public class UserToLogin
    {
        
        public string Email { get; set; }

        public string Password { get; set; }

    }
}

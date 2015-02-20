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
namespace CocktionMVC.Controllers.ApiControllers
{
    public class AccountController : ApiController
    {
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        [HttpPost]
        public async Task<Respond> Login(UserToLogin model)
        {
            

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
            Respond resp = new Respond();
            switch (result)
            {
                case SignInStatus.Success:
                    resp.Status = "Success";
                    return resp;
                case SignInStatus.LockedOut:
                    resp.Status = "Lockout";
                    return resp;
                case SignInStatus.RequiresVerification:
                    resp.Status = "RequiresVerification";
                    return resp;
                case SignInStatus.Failure:
                default:
                    resp.Status = "Failure";
                    return resp;
            }
        }

        /*
         * Небходимо дописать методы для регистрации
         * для входа через вкшку-какашку
         */ 
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

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.StandartControllers
{
    public class ProfileController : Controller
    {
        /// <summary>
        /// Выводит пользователя на страничку с информацией о его аккаунте
        /// </summary>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {//если пользватель авторизован
                CocktionContext db = new CocktionContext();

                var user = db.AspNetUsers.Find(User.Identity.GetUserId());

                return View(user);
            }

            //если пользователь не авторизован
            return RedirectToAction("HowItCouldBe");
        }

        /// <summary>
        /// Добавляет отзыв о пользователе в базу
        /// </summary>
        public async Task<JsonResult> AddUsersFeedback()
        {
            try
            {
                //Получаем текст сообщения и айдишник пользователя, который будет тут задействован
                var messageHolder = Request.Form.GetValues("message");
                if (messageHolder != null)
                {
                    string message = messageHolder[0].Trim();
                    var userIdHolder = Request.Form.GetValues("userId");
                    if (userIdHolder != null)
                    {
                        string userId = userIdHolder[0].Trim();
                        CocktionContext db = new CocktionContext();

                        string currentUserId = User.Identity.GetUserId();

                        //находим пользователя (который оставляет комментарий)
                        var user = db.AspNetUsers.Find(currentUserId);

                        //инициализация фидбека
                        UsersFeedback feedback = new UsersFeedback(user.UserRealName, user.UserRealSurname,
                            currentUserId, userId, message);
                        db.Feedbacks.Add(feedback);

                        //сохранение значений в базу
                        await DbItemsAdder.SaveDb(db);

                        return Json(new FeedBackRespond(user.UserRealName, user.UserRealSurname, "success"));
                    }
                }
                return Json(new FeedBackRespond(null, null, "failure"));
            }
            catch
            {
                return Json(new FeedBackRespond(null, null, "failure"));
            }
        }

        public class SelfieRespond : StatusHolder
        {
            public SelfieRespond(string fileName, bool isOk) : base(isOk)
            {
                FileName = fileName;
            }
            public string FileName { get; set; }
        }

        public JsonResult AddPhotoToUser()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(userId);
                Picture selfie = new Picture();
                PhotoProcessor.CreateAndSavePicture(selfie, Request, 200);
                user.Selfie = selfie;
                db.SaveChanges();
                return Json(new SelfieRespond(user.Selfie.FileName, true));
            }
            catch
            {
                return Json(new SelfieRespond(null, false));
            }
        }

        class FeedBackRespond
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Status { get; set; }

            public FeedBackRespond(string name, string surname, string status)
            {
                Name = name;
                Surname = surname;
                Status = status;
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

        [HttpPost]
        [Authorize]
        public JsonResult AddInterests()
        {
            try
            {
                string ids = Request.Form.GetValues("ids")[0];
                string[] sep = {"int_"};
                string[] idcont = ids.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                int[] idint = new int[idcont.Length];
                Array.ForEach(idcont, x => idint[i++] = int.Parse(x));
                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                for (int j = 0; j < idint.Length; j++)
                {
                    user.Interests.Add(db.Interests.Find(idint[j]));
                }
                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            catch
            {
                return Json(new StatusHolder(false));
            }
        }

        public JsonResult EditProfileInformation()
        {
            try
            {
                string name = Request.Form.GetValues("name")[0];
                string surname = Request.Form.GetValues("surname")[0];
                string school = Request.Form.GetValues("school")[0];

                CocktionContext db = new CocktionContext();
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());

                if (name.Length != 0)
                {
                    user.UserRealName = name;
                }
                if (surname.Length != 0)
                {
                    user.UserRealSurname = surname;
                }
                if (school.Length != 0)
                {
                    user.SocietyName = school;
                }
                db.SaveChanges();
                return Json(new StatusHolder(true));
            }
            catch
            {
                return Json(new StatusHolder(false));
            }
        }

     }
}
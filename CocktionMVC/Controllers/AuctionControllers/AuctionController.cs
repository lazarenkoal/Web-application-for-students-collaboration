using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Mvc;
using CocktionMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CocktionMVC.Models.Hubs;
using CocktionMVC.Models.ViewModels;
using System.IO;
using System.Threading.Tasks;
using CocktionMVC.Functions;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.DAL;
using System.Web.Hosting;
namespace CocktionMVC.Controllers
{
    /// <summary>
    /// Данный контроллер содержит в себе только те методы,
    /// которые открывают непосредственно вэб страницы.
    /// 
    /// Методы, которые используются для обработки RealTime 
    /// находятся в контроллере AuctionRealTime
    /// </summary>
    public class AuctionController : Controller
    {
        /// <summary>
        /// Главная страничка, на которой отображаются
        /// все активные в данный момент аукционы.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            /*Сделать страничку с бесконечной прокруткой
             * где будут отображаться все аукционы 
             * 
             * Подумать насчет фильтров (как их лучше прифигачить)
             */
            DateTime controlTime = DateTime.Now;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            controlTime = TimeZoneInfo.ConvertTime(controlTime, TimeZoneInfo.Local, tst);
            CocktionContext db = new CocktionContext();
            var auctions = (from x in db.Auctions
                            where ((x.EndTime > controlTime) && (x.IsActive == true))
                            select x).ToList<Auction>();
            return View(auctions);
        }//end of index

        /// <summary>
        /// Выводит страничку, где нужно создать
        /// аукцион
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Create() //метод для создания находится в контроллере FileSaver
        {
            CocktionContext context = new CocktionContext();
            var locations = context.Houses.ToList<House>();
            return View(locations);
        }//end of create

        /// <summary>
        /// Выводит страничку, которая показывает
        /// все созданные пользователем аукционы
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ShowMyAuctions()
        {
            var db = new CocktionContext();
            string userId = User.Identity.GetUserId();
            List<Auction> auctions = (from o in db.Auctions
                                      where o.OwnerId == userId
                                      select o).ToList<Auction>();
            return View("MyAuctions", auctions);
        }//end of ShowMyAuctions()

        /// <summary>
        /// Выводит страничку, где показан
        /// сам торг
        /// </summary>
        /// <param name="id">Айди аукциона, который надо показать</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CurrentAuction(int? id)
        {
            try
            {
                var db = new CocktionContext();
                Auction auction = db.Auctions.Find(id);
                if (auction == null && id == null) throw new Exception();
                return View(auction);
            }
            catch
            {
                return View("PageNotFound");
            }
        }//end of CurrentAuction


    }//end of AuctionController
}
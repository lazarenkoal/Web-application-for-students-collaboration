using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CocktionMVC.Models.DAL;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CocktionMVC.Controllers
{
    public class AuctionHouseController : Controller
    {
        // GET: AuctionHouse
        public ActionResult Index()
        {
            CocktionContext db = new CocktionContext();
            var houses = db.Houses.ToList<House>();            
            return View(houses);
        }

        public ActionResult GetCurrentAuctionHouse(int id)
        {
            CocktionContext db = new CocktionContext();
            var house = db.Houses.Find(id);
            return View(house);
        }

        public JsonResult AddComment()
        {
            Respond resp = new Respond();
            try
            {
                ForumPost post = new ForumPost();
                post.Message = Request.Form.GetValues("message")[0].Trim();
                post.AuthorName = User.Identity.Name;
                CocktionContext db = new CocktionContext();
                int houseId = int.Parse(Request.Form.GetValues("houseId")[0].Trim());
                var house = db.Houses.Find(houseId);
                post.Likes = 0;
                post.HostHouse = house;
                house.Posts.Add(post);
                db.SaveChanges();
                resp.Status = "Success";
                resp.Author = User.Identity.Name;
                return Json(resp);
            }
            catch
            {
                resp.Status = "Failed";
                return Json(resp);
            }
        }

        public class Respond
        {
            public string Status { get; set; }
            public string Author { get; set; }
        }
    }
}
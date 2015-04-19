using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels.MobileClientModels;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class DataController : ApiController
    {
        [Authorize]
        [HttpPost]
        public List<HouseInfo> GetHouses()
        {
            CocktionContext db = new CocktionContext();
            List<HouseInfo> houses = new List<HouseInfo>();
            foreach (var house in db.Houses)
            {
                houses.Add(new HouseInfo()
                {
                    adress = house.Adress,
                    faculty = house.Faculty,
                    likes = house.Likes,
                    rating = house.Rating,
                });
            }
            return houses;
        }

        [HttpPost]
        public void DeleteAll()
        {
            CocktionContext db = new CocktionContext();
            db.HouseHolders.RemoveRange(db.HouseHolders);
            //db.Houses.RemoveRange(db.Houses);
            //db.AspNetUsers.RemoveRange(db.AspNetUsers);
            //db.Auctions.RemoveRange(db.Auctions);
            //db.Products.RemoveRange(db.Products);
            //db.Photos.RemoveRange(db.Photos);
            //db.Houses.RemoveRange(db.Houses);
            //db.AuctionBids.RemoveRange(db.AuctionBids);
            //db.Feedbacks.RemoveRange(db.Feedbacks);
            //db.
            //db.Interests.RemoveRange(db.Interests);
            db.SaveChanges();
        }
    }
}

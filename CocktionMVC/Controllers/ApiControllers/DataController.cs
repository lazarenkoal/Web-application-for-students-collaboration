using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.MobileClientModels;
using Microsoft.AspNet.Identity;
namespace CocktionMVC.Controllers.ApiControllers
{
    public class DataController : ApiController
    {
        /// <summary>
        /// Посылает все дома на кокшне
        /// человеку
        /// </summary>
        /// <returns></returns>
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

        public class SearchString
        {
            public string name { get; set; }
        }

        /// <summary>
        /// Ищет аукционы по указанной строке
        /// </summary>
        /// <param name="searchString">че ищем</param>
        /// <returns>Коллекцию с аукционами, удовлетворяющими критериям поиска</returns>
        [Authorize]
        [HttpPost]
        public List<AuctionInfo> FindAuctions(SearchString searchString)
        {
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            DateTime controlTime = DateTimeManager.GetCurrentTime();
            List<Auction> aciveAuctions;
            aciveAuctions = (from x in db.Auctions
                            where (x.SellProduct.Name.Contains(searchString.name) || 
                            x.SellProduct.Description.Contains(searchString.name) ||
                            x.SellProduct.Category.Contains(searchString.name))
                            select x).ToList();
            List<AuctionInfo> auctis = new List<AuctionInfo>();
            foreach (var x in aciveAuctions)
            {
                if (x.Owner != null)
                {
                    auctis.Add(new AuctionInfo(x.SellProduct.Description,
                                (int)x.EndTime.Subtract(controlTime).TotalMinutes,
                                @"http://cocktion.com/Images/Thumbnails/" + x.SellProduct.Photo.FileName,
                                x.SellProduct.Name.Trim(), x.Id, x.LeadProduct == null ? -1 : x.LeadProduct.Id,
                                x.SellProduct.Category.Trim(), new UserInfo(x.Owner.Id, x.Owner.UserName, x.Owner.Selfie.FileName,
                                    user.Friends.Contains(x.Owner)),
                                (x.IsActive && (controlTime < x.EndTime))));
                }
            }
            return auctis;
        }


        public class PhotoId
        {
            public int id { get; set; }

            public PhotoId(int _id)
            {
                id = _id;
            }
        }

        public class PhotoType
        {
            public string type { get; set; }
        }


        /// <summary>
        /// Добавляет фотографию на сервак
        /// (только для товара или аукциона)
        /// </summary>
        /// <returns>Возвращает айдишник фотографии в базе данных</returns>
        [HttpPost]
        [Authorize]
        public async Task<PhotoId> UploadPhoto()
        {
            try
            {
                //получаем файлик
                string type = HttpContext.Current.Request.Form.GetValues("type")[0].Trim();
                string filePath = "";
                string fileName = "";
                HttpPostedFile postedFile = null;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        postedFile = httpRequest.Files[file];
                        string extension = Path.GetExtension(postedFile.FileName);
                        fileName = Guid.NewGuid() + extension; //генерируем новое имя для фотки
                        filePath = HttpContext.Current.Server.MapPath("~/Images/Photos/" + fileName);
                        postedFile.SaveAs(filePath);
                    }
                }

                //добавляем фоточку и фабмнейл
                string thumbNailPath = HttpContext.Current.Server.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
               // ThumbnailSet thumbNail = new ThumbnailSet();
               //humbNail.FileName = fileName;
               // thumbNail.FilePath = thumbNailPath + fileName;
                string thumbNailFilePath = thumbNailPath + fileName;

                if (type == "auction")
                {
                    ThumbnailGenerator.ResizeImage(postedFile, thumbNailFilePath, 90);
                }
                else if (type == "product")
                {
                    ThumbnailGenerator.ResizeImage(postedFile, thumbNailFilePath, 60);
                }

                CocktionContext db = new CocktionContext();

                Picture picture = new Picture();
                picture.FileName = fileName;
                picture.FilePath = thumbNailPath;
                db.Pictures.Add(picture);

                await DbItemsAdder.SaveDb(db);

                return new PhotoId(picture.Id);
            }
            catch 
            {
                return new PhotoId(-1);
            }            
        }
        
    }
}

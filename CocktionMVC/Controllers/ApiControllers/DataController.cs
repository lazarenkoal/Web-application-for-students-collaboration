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

        [HttpPost]
        public void DeleteAll()
        {
            CocktionContext db = new CocktionContext();
            db.Auctions.RemoveRange(db.Auctions.ToList());
            //db.Products.RemoveRange(db.Products.ToList());
            db.SaveChanges();
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

                //db.ThumbnailSets.Add(thumbNail);
                //db.Photos.Add(photo);

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

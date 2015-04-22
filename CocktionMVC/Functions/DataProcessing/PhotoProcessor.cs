using System;
using System.IO;
using System.Web;
using CocktionMVC.Models.DAL;

namespace CocktionMVC.Functions.DataProcessing
{
    /// <summary>
    /// Контейнер для методов, которые работают с фоточками
    /// </summary>
    public class PhotoProcessor
    {
        public static void CreateAndSavePicture(Picture picture, HttpRequestBase requestBase, int height)
        {
            HttpPostedFileBase file = null;
            if (requestBase.Files.Count != 0)
            {
                file = requestBase.Files[0];
            }

            if (file != null)
            {
                //Получаем информацию о том, откуда ж взялся файлик
                //string fileName = Path.GetFileName(file.FileName); //имя файла
                string extension = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid() + extension;

                //создаем мини-картинку
                string thumbNailPath = requestBase.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                picture.FileName = fileName;
                picture.FilePath = thumbNailPath + fileName;
                ThumbnailGenerator.ResizeImage(file, picture.FilePath, height);
            }
            else
            {
                const string placeHolderName = "placeholder.jpg";
                picture.FileName = placeHolderName;
                string path = Path.Combine(
                    requestBase.MapPath("~/Content/SiteImages"), placeHolderName);
                picture.FilePath = path;
            }
        }
    }
}
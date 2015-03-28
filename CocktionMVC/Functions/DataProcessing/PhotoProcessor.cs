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
        /// <summary>
        /// Получает фотку из запроса и создает к ней 
        /// миниатюрную
        /// </summary>
        /// <param name="photo">Существующий объект photo, который необходимо отредачить</param>
        /// <param name="requestBase">Запрос с инфой</param>
        /// <param name="width">Ширина фамбнейлика</param>
        /// <param name="height">Высота фамбнейлика</param>
        public static void CreateAndSavePhoto(Photo photo, HttpRequestBase requestBase, int width, int height)
        {
            //получаем файл
            HttpPostedFileBase file = requestBase.Files[0];

            if (file != null)
            {
                //Получаем информацию о том, откуда ж взялся файлик
                string fileName = Path.GetFileName(file.FileName); //имя файла
                string path = Path.Combine(
                    requestBase.MapPath("~/Images/Photos/"), fileName); //директория, в которую его загрузят
                file.SaveAs(path);

                //создаем мини-картинку
                //TODO нормальный размер сделать у нее
                string thumbNailPath = requestBase.MapPath("~/Images/Thumbnails/"); //путь на сервере для сохранения
                ThumbnailGenerator.ResizeImage(file, thumbNailPath, width, height); //переделываем размер картиночки
                
                ThumbnailSet thumbNail = new ThumbnailSet();
                thumbNail.FileName = fileName;
                thumbNail.FilePath = thumbNailPath + fileName;

                //забиваем данные о фотке
                photo.FileName = fileName;
                photo.FilePath = path;
                photo.ThumbnailSets.Add(thumbNail);
            }
        }
    }
}
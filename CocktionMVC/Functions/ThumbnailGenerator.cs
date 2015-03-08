using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.SqlServer;
namespace CocktionMVC.Functions
{
    public class ThumbnailGenerator
    {
        /// <summary>
        /// Метод, который на выходе дает картинку
        /// любого, наперед заданного размера!!!
        /// </summary>
        /// <param name="file">Файл, который надо поменять</param>
        /// <param name="serverPath">Адрес на сервере, в которую надо положить thumbNail</param>
        /// <param name="width">Необходимая ширина картинки</param>
        /// <param name="height">Необходимая высота картинки</param>
        public static void ResizeImage(HttpPostedFileBase file, string serverPath, int width, int height)
        {
            string pic = Path.GetFileName(file.FileName);
            string path = Path.Combine(serverPath, pic);

            //Стрим для сохранения файла после ресайза
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            //Конвертация файла в картинку
            Image originalImage = Image.FromStream(file.InputStream);

            //Создание нового битмапа с размером картинки
            Bitmap bitMapTempImg = new Bitmap(width, height);

            //Создание новой картинки, содержащей настройки качества
            Graphics newImage = Graphics.FromImage(bitMapTempImg);
            newImage.CompositingQuality = CompositingQuality.HighQuality;
            newImage.SmoothingMode = SmoothingMode.HighQuality;
            newImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Cоздаем прямоугольник и рисуем картинку
            Rectangle imageRectangle = new Rectangle(0, 0, width, height);
            newImage.DrawImage(originalImage, imageRectangle);

            //Cохраняем получившееся изображение
            bitMapTempImg.Save(stream, originalImage.RawFormat);

            //Зачистка ресурсов
            newImage.Dispose();
            bitMapTempImg.Dispose();
            originalImage.Dispose();
            stream.Close();
            stream.Dispose();

        }

        public static void ResizeImage(HttpPostedFile file, string serverPath, int width, int height)
        {
            string pic = Path.GetFileName(file.FileName);
            string path = Path.Combine(serverPath, pic);

            //Стрим для сохранения файла после ресайза
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            //Конвертация файла в картинку
            Image originalImage = Image.FromStream(file.InputStream);

            //Создание нового битмапа с размером картинки
            Bitmap bitMapTempImg = new Bitmap(width, height);

            //Создание новой картинки, содержащей настройки качества
            Graphics newImage = Graphics.FromImage(bitMapTempImg);
            newImage.CompositingQuality = CompositingQuality.HighQuality;
            newImage.SmoothingMode = SmoothingMode.HighQuality;
            newImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Cоздаем прямоугольник и рисуем картинку
            Rectangle imageRectangle = new Rectangle(0, 0, width, height);
            newImage.DrawImage(originalImage, imageRectangle);

            //Cохраняем получившееся изображение
            bitMapTempImg.Save(stream, originalImage.RawFormat);

            //Зачистка ресурсов
            newImage.Dispose();
            bitMapTempImg.Dispose();
            originalImage.Dispose();
            stream.Close();
            stream.Dispose();

        }
    }


}
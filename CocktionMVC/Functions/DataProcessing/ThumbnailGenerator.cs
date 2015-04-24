using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace CocktionMVC.Functions
{
    public class ThumbnailGenerator
    {
        /// <summary>
        /// Метод, который на выходе дает картинку
        /// любого, наперед заданного размера!!!
        /// </summary>
        /// <param name="file">Файл, который надо поменять</param>
        /// <param name="fileFullPath">Полный путь и имя файла</param>
        /// <param name="width">Необходимая ширина картинки</param>
        /// <param name="height">Необходимая высота картинки</param>
        public static void ResizeImage(HttpPostedFileBase file, string fileFullPath, int height)
        {
            //Стрим для сохранения файла после ресайза
            FileStream stream = new FileStream(fileFullPath, FileMode.OpenOrCreate);

            //Конвертация файла в картинку
            Image originalImage = Image.FromStream(file.InputStream);

            int width = CountWidth(originalImage.Width, originalImage.Height, height);

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

        /// <summary>
        /// Создает уменьшенную копию фотки заданной ширины и высоты
        /// 
        /// ВЕРСИЯ ДЛЯ МОБИЛЬНОГО КЛИЕНТА
        /// </summary>
        /// <param name="file">Файл с фоткой</param>
        /// <param name="serverFullPath">Полный путь к новой фотке на сервере</param>
        /// <param name="width">Ширина (которую надо сделать)</param>
        /// <param name="height">Высота (которую хотим получить)</param>
        public static void ResizeImage(HttpPostedFile file, string serverFullPath, int height)
        {

            //Стрим для сохранения файла после ресайза
            FileStream stream = new FileStream(serverFullPath, FileMode.OpenOrCreate);

            //Конвертация файла в картинку
            Image originalImage = Image.FromStream(file.InputStream);

            int width = CountWidth(originalImage.Width, originalImage.Height, height);

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

        /// <summary>
        /// Считает оптимальную ширину по заданой высоте
        /// </summary>
        /// <param name="sourceWidth">Исходная ширина</param>
        /// <param name="sourceHeight">Исходная высота</param>
        /// <param name="newHeight">Нужная высота</param>
        /// <returns>Значение нужной ширины</returns>
        private static int CountWidth(int sourceWidth, int sourceHeight, int newHeight)
        {
            return (newHeight*sourceWidth)/sourceHeight;
        }
    }


}
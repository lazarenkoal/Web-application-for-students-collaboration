using System.Web;

namespace CocktionMVC.Functions
{
    /// <summary>
    /// Контейнер для методов, считывающих значения с форм на клиентах
    /// либо получающих инфу о каких-то полях из запроса
    /// </summary>
    public class RequestFormReader
    {
        /// <summary>
        /// Читает данные из формы с добавлением ставки на тотализаторе
        /// </summary>
        /// <param name="request">Запрос, который содержит форму с информацией о ставке на тотализаторе</param>
        /// <param name="auctionId">Айдишник аукциона, к которому добавляют ставку на тотализатор</param>
        /// <param name="productId">Айдишника товара, на который ставят яйца</param>
        /// <param name="eggsAmount">Количество яиц, которые хотят поставить</param>
        public static void ReadAddRateForm(HttpRequestBase request, out int auctionId, 
            out int productId, out int eggsAmount)
        {
            //получаем информацию из формы
            string auctionIdStr = request.Form.GetValues("auctionId")[0];
            string productIdStr = request.Form.GetValues("productId")[0];
            string eggsAmountStr = request.Form.GetValues("eggsAmount")[0];

            //записываем значения
            auctionId = int.Parse(auctionIdStr);
            productId = int.Parse(productIdStr);
            eggsAmount = int.Parse(eggsAmountStr);
        }

        /// <summary>
        /// Читает данные из формы с добавлением лидера аукциона
        /// </summary>
        /// <param name="request">Запрос, который содержит форму с информацией о лидере</param>
        /// <param name="auctionId">Айдишник аукциона, на котором все происходит</param>
        /// <param name="productId">Айдишник товара, на котором все происходит</param>
        public static void ReadAddLiderForm(HttpRequestBase request, out string auctionId,
            out string productId)
        {
            auctionId = request.Form.GetValues("auctionId")[0];
            productId = request.Form.GetValues("productId")[0];
        }

        /// <summary>
        /// Читает данные из формы с созданием аукциона.
        /// </summary>
        /// <param name="request">Запрос, содержащий форму с информацией об аукционе</param>
        /// <param name="name">Наименование товара</param>
        /// <param name="description">Описание товара</param>
        /// <param name="category">Категория, в которую товар плавненько попадает</param>
        /// <param name="housesId">Айдишники домов, в которых торгуется данный товар</param>
        /// <param name="minutes">Количество минут, которые будет длиться аукцион</param>
        /// <param name="hours">Количество часов продолжительности аукциона</param>
        public static void ReadCreateAuctionForm(HttpRequestBase request, out string name,
            out string description, out string category, out string housesId, out string minutes,
            out string hours)
        {
            name = request.Form.GetValues("name")[0].Trim();
            description = request.Form.GetValues("description")[0].Trim();
            category = request.Form.GetValues("category")[0].Trim();
            housesId = request.Form.GetValues("housesId")[0].Trim('!');
            minutes = request.Form.GetValues("minutes")[0].Trim();
            hours = request.Form.GetValues("hours")[0].Trim();
        }

        /// <summary>
        /// Читает данные из формы с созданием аукциона. (Мобильный клиент)
        /// </summary>
        /// <param name="request">Запрос, содержащий форму с информацией об аукционе</param>
        /// <param name="name">Наименование товара</param>
        /// <param name="description">Описание товара</param>
        /// <param name="category">Категория, в которую товар плавненько попадает</param>
        /// <param name="housesId">Айдишники домов, в которых торгуется данный товар</param>
        /// <param name="minutes">Количество минут, которые будет длиться аукцион</param>
        /// <param name="hours">Количество часов продолжительности аукциона</param>
        public static void ReadCreateAuctionFormMobile(HttpRequest request, out string name,
            out string description, out string category, out string minutes,
            out string hours)
        {
            name = request.Form.GetValues("name")[0].Trim();
            description = request.Form.GetValues("description")[0].Trim();
            category = request.Form.GetValues("category")[0].Trim();
            minutes = request.Form.GetValues("minutes")[0].Trim();
            hours = request.Form.GetValues("hours")[0].Trim();
        }

        /// <summary>
        /// Читает данные из формы с добавлением ставки на аукцион
        /// </summary>
        /// <param name="requestBase">Запрос, содержащий форму с информацией о товаре</param>
        /// <param name="name">Наименование товара</param>
        /// <param name="auctionId">Айдишник аукциона</param>
        /// <param name="category">Категория, к которой относится товар</param>
        /// <param name="description">Описание товара</param>
        public static void ReadAddProductBetForm(HttpRequestBase requestBase, out string name,
            out string auctionId, out string category, out string description)
        {
            name = requestBase.Form.GetValues("name")[0].Trim();
            description = requestBase.Form.GetValues("description")[0].Trim();
            category = requestBase.Form.GetValues("category")[0].Trim();
            auctionId = requestBase.Form.GetValues("auctionId")[0].Trim();
        }

        public static void ReadAddHouseForm(HttpRequestBase requestBase, out string university,
            out string faculty, out string adress)
        {
            university = requestBase.Form.GetValues("university")[0].Trim();
            faculty = requestBase.Form.GetValues("faculty")[0].Trim();
            adress = requestBase.Form.GetValues("adress")[0].Trim();
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using CocktionMVC.Models.JsonModels.ToteRelatedModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CocktionMVC.Controllers
{
    /// <summary>
    /// Контроллер используется для отсылки клиентам
    /// данных, связанных с аукционом в реальном времени
    /// </summary>
    public class AuctionRealTimeController : Controller
    {
        /// <summary>
        /// Метод для добавления ставки в тотализаторе
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddToteRate()
        {
            //Получаем инморфмацию из формы о добавленной ставке
            int auctionId;
            int productId;
            int eggsAmount;
            RequestFormReader.ReadAddRateForm(Request, out auctionId, out productId, out eggsAmount);
            
            //Идентифицируем пользователя
            string userId = User.Identity.GetUserId();

            //Подключемся к базе данных
            CocktionContext db = new CocktionContext();

            var user = db.AspNetUsers.Find(userId);

            //Находим в базе этот аукцион
            Auction auction = db.Auctions.Find(auctionId);

            //Активируем тотализатор
            auction.AuctionToteBoard.IsActive = true;

            //TODO ДОБАВИТЬ ПРОВЕРКУ НА ВЫБОР ПОЛЬЗОВАТЕЛЕМ ТОВАРА-ПРОДАВЦА

            //Добавляем ставку пользователя
            //эта же штука и в сайгнал все посылает
            bool x = await auction.AuctionToteBoard.SetRateForUser(auctionId, user, eggsAmount, productId, db);

            //Создаем результат добавления ставки
            ToteEggsInfo info = new ToteEggsInfo
            {
                Status = x,
                UsersAmountOfEggs = user.Eggs
            };
            return Json(info);
        }


        /// <summary>
        /// Записыват лидера в аукцион
        /// Добавляет эти данные в бд.
        /// </summary>
        /// <returns>Строку со статусом добавления</returns>
        [HttpPost]
        public async Task<JsonResult> AddLider()
        {
            //TODO: сделать эту хрень асинхронной
            string auctionId;
            string productId;
            RequestFormReader.ReadAddLiderForm(Request, out auctionId, out productId);
            try
            {
                var db = new CocktionContext();
                Auction auction = db.Auctions.Find(int.Parse(auctionId));
                Product product = db.Products.Find(int.Parse(productId));
                auction.LeadProduct = product;
                auction.WinnerChosen = true;
                await DbItemsAdder.SaveDb(db);
                AuctionHub.SetLider(productId, int.Parse(auctionId), product.Name);

                return Json(new StatusHolder(true, product.Name));
            }
            catch
            {
                return Json(new StatusHolder(false, ""));
            }
        }

        /// <summary>
        /// Универсальный контейнер статуса
        /// </summary>
        class StatusHolder
        {
            //Используется сразу строка для удобства
            public StatusHolder(bool truthKeeper, string liderName)
            {
                Status = truthKeeper.ToString();
                LiderName = liderName;
            }
            public string Status { get; set; }
            public string LiderName { get; set; }
        }

        /// <summary>
        /// Проверяет ставил ли пользователь свою ставку на этом
        /// аукционе
        /// </summary>
        /// <returns>Джейсончик со статусом</returns>
        [HttpPost]
        public JsonResult CheckIfUserBidded()
        {
            BidChecker checker = new BidChecker();
            CocktionContext db = new CocktionContext();
            var userId = User.Identity.GetUserId();
            int auctionId = int.Parse(Request.Form.GetValues("auctionId")[0]);

            //Ищем товар с владельцем с данным айдишником.
            checker.HaveBid = db.Auctions.Find(auctionId).BidProducts.First(x => x.Owner.Id == userId) != null;
            return Json(checker);
        }

        /// <summary>
        /// Получает с клиента айдишник, по нему находит 
        /// искомый продукт и возвращает информацию о нем.
        /// </summary>
        /// <returns>Объект, содержащий информацию о продукте.</returns>
        [HttpPost]
        public async Task<JsonResult> SendInfoAboutProduct()
        {
            string productId = Request.Form.GetValues("Id")[0];
            var db = new CocktionContext();
            Product product = await db.Products.FindAsync(int.Parse(productId));
            ProductInfo info = new ProductInfo
            {
                Name = product.Name,
                Description = product.Description,
                FileName = product.Photo.FileName,
                Category = product.Category
            };
            return Json(info);
        }

        /// <summary>
        /// Посылает результаты аукциона на клиент
        /// </summary>
        /// <returns>Объект JSON, который содержит
        /// в себе всю необходимую информацию.</returns>
        [HttpPost]
        public JsonResult SendAuctionResults()
        {
            //Получаем информацию с клиента
            int auctionId = int.Parse(Request.Form.GetValues("auctionId")[0]);
            
            //ищем аукцион, который завершился в базе
            var db = new CocktionContext();
            Auction auction = db.Auctions.Find(auctionId);

            //раздаем результаты
            if (auction.WinnerChosen == false)
            {//если человек не выбрал победителя
                //TODO рандомно выбираем победителя
                if (User.Identity.IsAuthenticated)
                {//Если пользователь авторизован
                    string userName = User.Identity.Name;
                    if (userName == auction.Owner.UserName)
                    {//если пользователь является создателем
                        BidSeller owner = new BidSeller();
                        owner.Name = userName;
                        owner.Type = "Owner_undfnd";
                        owner.Message = "Необходимо выбрать лидера!!!";
                        return Json(owner);
                    }
                    //если любой другой
                    BidSeller person = new BidSeller();
                    person.Type = "Info";
                    person.Message = "Создатель аукциона совсем не смог выбрать :(";
                    return Json(person);
                }
                else
                {//если пользователь не авторизован
                    BidSeller person = new BidSeller();
                    person.Type = "Info";
                    person.Message = "Создатель аукциона совсем не смог выбрать :(";

                    return Json(person);
                }
            }
            else
            {//если победитель выбран
                //Ищем продукт - победиель
                Product winProduct = auction.LeadProduct;                
                if (User.Identity.IsAuthenticated)
                {//если пользователь авторизован
                    string userName = User.Identity.Name;
                    string userId = User.Identity.GetUserId();
                    var currentUser = db.AspNetUsers.Find(userId);
                    if (currentUser == auction.Owner)
                    {//если пользователь - владелец
                        BidSeller winner = new BidSeller();

                        winner.Id = winProduct.Owner.Id;
                        winner.Name = winProduct.Owner.UserName;
                        string phone = winProduct.Owner.PhoneNumber == null
                            ? "Телефона нет ;("
                            : winProduct.Owner.PhoneNumber == "" ? "Телефона нет" : winProduct.Owner.PhoneNumber;
                        winner.Type = "Winner";
                        winner.Message = "Аукцион закончен, вам необходимо связаться с победителем! " + phone;
                        return Json(winner);
                    }
                    else if (currentUser == winProduct.Owner)
                    {//если пользователь - победитель
                        BidSeller owner = new BidSeller();
                        owner.Id = auction.Owner.Id;
                        owner.Name = auction.Owner.UserName;
                        string phone = auction.Owner.PhoneNumber;
                        owner.Type = "Owner";

                        //Получаем результаты тотализатора
                        ToteResultsManager.GetToteResults(auction, userId, owner, phone, currentUser);

                        return Json(owner);
                    }
                    else
                    {//если пользователь - любой другой пользователь
                        BidSeller looser = new BidSeller();
                        looser.Name = userName;
                        looser.Type = "Looser";

                        //получаем результаты тотализатора
                        ToteResultsManager.GetToteResults(auction, userId, looser, currentUser, winProduct.Name);
                        return Json(looser);
                    }
                }
                else
                {//если пользователь неавторизован
                    BidSeller person = new BidSeller();
                    person.Type = "Info";
                    person.Message = "Аукцион закончился, выйграл товар " + winProduct.Name;

                    return Json(person);
                }
            }//end of else
        }//end of SendAuctionResults
    }//end of AuctionRealTime контроллера
}//конец неймспейса
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
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
        public async Task<JsonResult> AddRate()
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

            //Находим в базе этот аукцион
            Auction auction = db.Auctions.Find(auctionId);

            //Активируем тотализатор
            auction.AuctionToteBoard.IsActive = true;

            //TODO ДОБАВИТЬ ПРОВЕРКУ НА ВЫБОР ПОЛЬЗОВАТЕЛЕМ ТОВАРА-ПРОДАВЦА

            //Добавляем ставку пользователя
            bool x = await auction.AuctionToteBoard.SetRateForUser(auctionId, userId, eggsAmount, productId, db);

            //Получаем инстанс пользователя
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(userId);

            //Получаем количество яиц у пользователя
            int amount = currentUser.Eggs;

            //Создаем результат добавления ставки
            ToteEggsInfo info = new ToteEggsInfo
            {
                Status = x.ToString(),
                UsersAmountOfEggs = amount
            };

            //послать через сайгнал Р информацию на клиенты
            //о состоянии аукциона

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
                auction.WinProductId = productId;
                auction.WinnerChosen = true;
                auction.WinProductName = db.Products.Find(int.Parse(productId)).Name;
                await DbItemsAdder.SaveDb(db);
                AuctionHub.SetLider(productId, int.Parse(auctionId), auction.WinProductName);

                return Json(new StatusHolder(true, auction.WinProductName));
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
            checker.HaveBid = db.Auctions.Find(auctionId).BidProducts.First(x => x.OwnerId == userId) != null;
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
                FileName = product.Photos.First().FileName,
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
                    if (userName == auction.OwnerName)
                    {//если пользователь является создателем
                        BidSeller owner = new BidSeller();
                        owner.Name = userName;
                        owner.Type = "Owner_undfnd";
                        owner.Message = "Необходимо выбрать лидера!!!";

                        return Json(owner);
                    }
                    else
                    {//если любой другой
                        BidSeller person = new BidSeller();
                        person.Type = "Info";
                        person.Message = "Создатель аукциона совсем не смог выбрать :(";

                        return Json(person);
                    }
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
                Product winProduct = db.Products.Find(int.Parse(auction.WinProductId));
                
                //получаем доступ к пользовательским полям
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                
                if (User.Identity.IsAuthenticated)
                {//если пользователь авторизован
                    string userName = User.Identity.Name;
                    string userId = User.Identity.GetUserId();
                    var currentUser = manager.FindById(userId);
                    if (userName == auction.OwnerName)
                    {//если пользователь - владелец
                        BidSeller winner = new BidSeller();
                        winner.Id = winProduct.OwnerId;
                        winner.Name = winProduct.OwnerName;
                        string phone = currentUser.PhoneNumber;
                        winner.Type = "Winner";
                        winner.Message = "Аукцион закончен, вам необходимо связаться с победителем! " + phone;

                        return Json(winner);
                    }
                    else if (userName == winProduct.OwnerName)
                    {//если пользователь - победитель
                        BidSeller owner = new BidSeller();
                        owner.Id = auction.OwnerId;
                        owner.Name = auction.OwnerName;
                        string phone = currentUser.PhoneNumber;
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
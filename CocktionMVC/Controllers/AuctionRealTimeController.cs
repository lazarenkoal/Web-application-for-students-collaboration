using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
            //Получить с сервера информацию об айди
            //аукциона, айди товара, на который
            //поставлена ставка, количество яиц
            int auctionId, productId, eggsAmount;
            string auctionIdStr, productIdStr, eggsAmountStr;
            auctionIdStr = Request.Form.GetValues("auctionId")[0];
            productIdStr = Request.Form.GetValues("productId")[0];
            eggsAmountStr = Request.Form.GetValues("eggsAmount")[0];
            auctionId = int.Parse(auctionIdStr);
            productId = int.Parse(productIdStr);
            eggsAmount = int.Parse(eggsAmountStr);
            
            //получить информацию о том, кто есть
            //пользователь
            string userId = User.Identity.GetUserId();

            //Подключиться к базе данных
            CocktionContext db = new CocktionContext();

            //Найти в базе нужный аукцион
            Auction auction = db.Auctions.Find(auctionId);
            auction.AuctionToteBoard.IsActive = true;

            //проверить, достаточно ли яиц на счете
            //у этого человека
            //произвести все расчеты, связанные 
            //тотализатором
            //добавить все эти данные в словари 
            //вернуть статус добавления
            bool x; 
            x = await auction.AuctionToteBoard.SetRateForUser(userId, eggsAmount, productId, db);
            int amount = db.AspNetUsers.Find(userId).Eggs;

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
            string Auctionid, ProductId;
            Auctionid = Request.Form.GetValues("AuctionId")[0];
            ProductId = Request.Form.GetValues("ProductId")[0];
            try
            {
                var db = new CocktionContext();
                Auction auction = db.Auctions.Find(int.Parse(Auctionid));
                auction.WinProductId = ProductId;
                auction.WinnerChosen = true;
                await DbItemsAdder.SaveDb(db);
                string answer = "Выбор сделан:)";
                return Json(answer);
            }
            catch
            {
                string answer = "Вот дерьмо";
                return Json(answer);
            }
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
                FileName = product.Photos.First().FileName
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

            string userName = User.Identity.Name;

            var db = new CocktionContext();
            Auction auction = db.Auctions.Find(auctionId);
            if (auction.WinnerChosen == false)
            {//если человек не выбрал победителя

                //рандомно выбираем победителя
                if (User.Identity.IsAuthenticated)
                {
                    if (userName == auction.OwnerName)
                    {
                        BidSeller owner = new BidSeller();
                        owner.Name = userName;
                        owner.Type = "Owner_undfnd";
                        owner.Message = "Необходимо выбрать лидера!!!";

                        return Json(owner);
                    }
                    else
                    {
                        BidSeller person = new BidSeller();
                        person.Type = "Info";
                        person.Message = "Создатель аукциона совсем не смог выбрать :(";

                        return Json(person);
                    }
                }
                else
                {
                    BidSeller person = new BidSeller();
                    person.Type = "Info";
                    person.Message = "Создатель аукциона совсем не смог выбрать :(";

                    return Json(person);
                }
            }
            else //если победитель выбран
            {
                Product winProduct = db.Products.Find(int.Parse(auction.WinProductId));

                if (User.Identity.IsAuthenticated)
                {
                    if (userName == auction.OwnerName)
                    {//if user os owner
                        //send to client info about winner
                        BidSeller winner = new BidSeller();
                        winner.Id = winProduct.OwnerId;
                        winner.Name = winProduct.OwnerName;
                        string phone = db.AspNetUsers.Find(winProduct.OwnerId).PhoneNumber;
                        winner.Type = "Winner";
                        winner.Message = "Аукцион закончен, вам необходимо связаться с победителем! " + phone;

                        return Json(winner);
                    }
                    else if (userName == winProduct.OwnerName)
                    {//if user is winner
                        //send to client info about owner
                        BidSeller owner = new BidSeller();
                        owner.Id = auction.OwnerId;
                        owner.Name = auction.OwnerName;
                        string phone = db.AspNetUsers.Find(auction.OwnerId).PhoneNumber;
                        owner.Type = "Owner";
                        owner.Message = "Аукцион закончен, вам необходимо связаться с продавцом! " + phone;

                        return Json(owner);
                    }
                    else
                    {//if user is authenticated
                        BidSeller looser = new BidSeller();
                        looser.Name = userName;
                        looser.Type = "Looser";
                        looser.Message = String.Format("Дорогой, аукцион закончен и победил товар {0}", winProduct.Name);

                        return Json(looser);
                    }
                }//if user is not authenticated
                else
                {
                    BidSeller person = new BidSeller();
                    person.Type = "Info";
                    person.Message = "Аукцион закончился, выйграл товар " + winProduct.Name;

                    return Json(person);
                }
            }//end of else
        }//end of SendAuctionResults
    }//end of AuctionRealTime контроллера
}//конец неймспейса
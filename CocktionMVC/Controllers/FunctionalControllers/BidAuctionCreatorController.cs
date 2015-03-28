using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.Hubs;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers
{
    public class BidAuctionCreatorController : Controller
    {
        /// <summary>
        /// Создает аукцион и записывает его в базу данных
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> CreateAuction()
        {
            //Получаем информацию из формы
            string name, description, category, housesId, minutes, hours;
            RequestFormReader.ReadCreateAuctionForm(Request, out name, out description,
                out category, out housesId, out minutes, out hours);

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();

            //создаем продуктик, ха
            Product product = new Product(name, description, category, userId, true, userName);

            //инициализация аукциона
            Auction auction = new Auction(true, userId, userName, product, false, new ToteBoard());

            //добавляем все локации списочком к аукциону
            int[] locIdsInt = DataFormatter.GetHouseIds(housesId);
            Array.ForEach(locIdsInt, x => auction.Houses.Add(db.Houses.Find(x)));

            //задаем время окончания и начала аукциона
            DateTimeManager.SetAuctionStartAndEndTime(auction, hours, minutes);

            //добавление фотографии для товара
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 90, 90);
            photo.Product = product;

            //сохраняем все изменения в базу
            await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo);

            //апдейтим список c аукционами
            AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            //возвращаем статус
            return Json("Успешно добавлено");
        }

        /// <summary>
        /// Добавляет довесок к товару
        /// </summary>
        [HttpPost]
        public async Task AddExtraBid()
        {
            //Adding new product to the DB
            CocktionContext db = new CocktionContext();

            //Инициализируем и добавляем фоточку
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 60, 60);

            //обработка данных из формы
            string bidName, bidDescription, bidCategory, auctionId;
            RequestFormReader.ReadAddProductBetForm(Request, out bidName, out auctionId,
                out bidCategory, out bidDescription);

            int id = int.Parse(auctionId);
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            //Создаем товар для базы данных
            Product product = new Product(bidName, bidDescription, bidCategory, userId,
                userName);

            product.Photos.Add(photo);
            photo.Product = product;

            var auction = db.Auctions.Find(id);
            auction.BidProducts.Add(product);

            //Выбираем все кластер, где пользователь == данный
            BidCluster cluster = (from x in auction.UsersBids
                                  where x.UserId == userId
                                  select x).First();
            cluster.Products.Add(product);

            //Добавляем продукт в базу данных
            await DbItemsAdder.AddProduct(db, product, photo, cluster);

            //Найдем вхождние товара в кластере первое
            int firstProductId = cluster.Products.First().Id;
            foreach (var bidProduct in cluster.Products)
            {
                int bidProductId = bidProduct.Id;
                if (bidProductId != firstProductId)
                {
                    //Добавляем на всех клиентах довесочек
                    AuctionHub.AddExtraNodeToClients(bidProduct.Name, bidProduct.Photos.First().FileName,
                                               id, firstProductId, bidProductId);
                }
            }
        }//AddExtraBid

        /// <summary>
        /// Метод добавления ставки на аукцион (товара)
        /// </summary>
        [HttpPost]
        public async Task AddProductBet()
        {
            //обработка данных из формы
            string bidName, auctionId, bidCategory, bidDescription;
            RequestFormReader.ReadAddProductBetForm(Request, out bidName, out auctionId, 
                out bidCategory, out bidDescription);

            //Инициализируем фоточку
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 60, 60);
           
            //Создаем товар для базы данных
            Product product = new Product(bidName, bidDescription, bidCategory, User.Identity.GetUserId(),
                User.Identity.Name);
            product.Photos.Add(photo);
            photo.Product = product;
            
            //добавляем продукт
            //Коннектимся к базе
            CocktionContext db = new CocktionContext();
            int id = int.Parse(auctionId);
            db.Auctions.Find(id).BidProducts.Add(product);

            //находи кластер
            BidCluster bidCluster = new BidCluster();
            bidCluster.UserId = User.Identity.GetUserId();
            bidCluster.HostAuction = db.Auctions.Find(id);
            bidCluster.Products.Add(product);

            //сохраняем все в базу
            await DbItemsAdder.AddProduct(db, product, photo, bidCluster);

            //добавляем нодики на клиенты
            AuctionHub.AddNodesToClients(bidName, photo.FileName, id, product.Id);
        }//end of AddProductBet
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Functions.DataProcessing;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.Identity;

// ReSharper disable once CheckNamespace
namespace CocktionMVC.Controllers
{
    public class BidAuctionCreatorController : Controller
    {
        /// <summary>
        /// Создает аукцион и записывает его в базу данных
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> CreateAuction()
        {
            //Получаем информацию из формы
            string name, description, category, timeBound;
            RequestFormReader.ReadCreateAuctionForm(Request, out name, out description,
                out category, out timeBound);

            //получаем информацию о пользователе
            string userId = User.Identity.GetUserId();

            //подключаемся к базе данных
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(userId);

            //создаем продуктик, ха
            Product product = new Product(name, description, category, true, user);

            //инициализация аукциона
            Auction auction = new Auction(true, product, false, new ToteBoard(), user);

            //TODO сделать динамический выбор дома
            auction.Houses.Add(db.Houses.Find(4));

            //задаем время окончания и начала аукциона
            DateTimeManager.SetAuctionStartAndEndTime(auction, timeBound);

            //добавление фотографии для товара
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 90, 90);
            photo.Product = product;

            await DbItemsAdder.AddAuctionProductPhotoAsync(db, auction, product, photo, user);

            //TODO апдейтим список c аукционами
            //AuctionListHub.UpdateList(product.Name, product.Description, product.Category, photo.FileName);

            //возвращаем статус
            return Json(new IdContainer(auction.Id));
        }

        class IdContainer
        {
            public IdContainer(int id)
            {
                Id = id;
            }
            public int Id { get; set; }
        }

        /// <summary>
        /// Добавляет довесок к товару
        /// </summary>
        [HttpPost]
        public async Task AddExtraBid()
        {
            //Adding new product to the DB
            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());

            //Инициализируем и добавляем фоточку
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 60, 60);

            //обработка данных из формы
            string bidName, bidDescription, bidCategory, auctionId;
            RequestFormReader.ReadAddProductBetForm(Request, out bidName, out auctionId,
                out bidCategory, out bidDescription);

            int id = int.Parse(auctionId);

            //Создаем товар для базы данных
            Product product = new Product(bidName, bidDescription, bidCategory, user);

            product.Photos.Add(photo);
            photo.Product = product;

            var auction = db.Auctions.Find(id);
            auction.BidProducts.Add(product);

            //Выбираем все кластер, где пользователь == данный
            //TODO эта херня работать не будет
            BidCluster cluster = (from x in auction.UsersBids
                                  where x.UserId == user.Id
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

            CocktionContext db = new CocktionContext();
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());

            //Инициализируем фоточку
            Photo photo = new Photo();
            PhotoProcessor.CreateAndSavePhoto(photo, Request, 60, 60);
           
            //Создаем товар для базы данных
            Product product = new Product(bidName, bidDescription, bidCategory, user);
            product.Photos.Add(photo);
            photo.Product = product;

            if (user.HisProducts == null)
            {
                user.HisProducts = new HashSet<Product>();
            }

            user.HisProducts.Add(product);
            
            //добавляем продукт
            //Коннектимся к базе
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
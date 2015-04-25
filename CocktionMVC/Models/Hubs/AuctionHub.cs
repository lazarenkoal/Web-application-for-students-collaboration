using System.Collections.Generic;
using System.Threading.Tasks;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.SignalR;
using WebGrease.Extensions;
using CocktionMVC.Functions.Managers;

namespace CocktionMVC
{
    
    /// <summary>
    /// Хаб используется для работы со страницой 
    /// CurrentAuction. Он посылает сообщения, 
    /// проводит транзакции с окончанием аукциона
    /// и т.д.
    /// </summary>
    public class AuctionHub : Hub
    {
        /// <summary>
        /// Посылает сообщения всем клиентам в данной
        /// группе
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="auctionId">Id аукциона</param>
        public void Send(string name, string message, int auctionId)
        {
            Clients.Group(auctionId.ToString()).addNewMessageToPage(name, message);
        }

        /// <summary>
        /// Асинхронно добавляет новую группу
        /// на сервере
        /// </summary>
        /// <param name="auctionId">Айди аукциона</param>
        public void AddNewRoom(int auctionId)
        {
            Groups.Add(Context.ConnectionId, auctionId.ToString());
        }

        /// <summary>
        /// Добавляет товары в диаграмму на всех клиентах, где
        /// открыта страничка
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="fileName">Название файла с фотографией</param>
        /// <param name="auctionId">Айди аукциона</param>
        public static void AddNodesToClients(string name, string fileName, int auctionId, int productId)
        {
            //Добавляем клиентам всей группы Нодики
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
            context.Clients.Group(auctionId.ToString()).addNodesToPages(fileName, name, productId);
        }

        /// <summary>
        /// Добавляет довесок к товару на всех клиентах, где
        /// открыта страничка с аукционом
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="fileName">Название файла с фотографией</param>
        /// <param name="auctionId">Айдишник аукциона</param>
        /// <param name="parentProductId">айдишник товара, к которому добавлен довесок</param>
        /// <param name="childProductId">Айдишник товара, который добавляется в кач-ве довеска</param>
        public static void AddExtraNodeToClients(string name, string fileName, int auctionId, int parentProductId,
                                            int childProductId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();

            context.Clients.Group(auctionId.ToString()).addExtraNodesToPages(fileName, name, parentProductId, childProductId);
        }

        /// <summary>
        /// Метод посылает айдишник лидера на все клиенты
        /// </summary>
        /// <param name="leaderId">Айдишник лидера</param>
        /// <param name="auctionId">Айдишник аукциона, на котором все это происходит</param>
        public static void SetLider(string leaderId, int auctionId, string liderName)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
            context.Clients.Group(auctionId.ToString()).showLeaderOnPage(leaderId, liderName);
        }

        public async Task JoinRoom(int auctionId)
        {
            await Groups.Add(Context.ConnectionId, auctionId.ToString());
        }


        /// <summary>
        /// Метод заканчивает аукцион
        /// </summary>
        /// <param name="auctionId">Айди аукциона, который нужно закончить</param>
        public async Task FinishAuction(int auctionId)
        {
            //заканчивать здесь аукцион
            CocktionContext db = new CocktionContext();
            await FinishAuctionManager.FalseAuctionStatus(db, auctionId);
            Clients.Group(auctionId.ToString()).finishAuction();
        }

        /// <summary>
        /// Метод заканчивает аукцион c мобильника
        /// </summary>
        /// <param name="auctionId">Айди аукциона, который нужно закончить</param>
        public static async Task FinishAuctionMobile(int auctionId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
            //заканчивать здесь аукцион
            CocktionContext db = new CocktionContext();
            await FinishAuctionManager.FalseAuctionStatus(db, auctionId);
            context.Clients.Group(auctionId.ToString()).finishAuction();
        }

        /// <summary>
        /// Метод для отправки на клиент всех значений тотализатора
        /// в данный момент времени
        /// </summary>
        /// <param name="auctionId">Айди аукциона, участникам которого необходимо сообщить</param>
        public static void UpdateToteBoard(int auctionId, Dictionary<string, double> data)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
            string respond;
            respond = DataFormatter.DictionaryConverter(data);
            context.Clients.Group(auctionId.ToString()).updateToteBoard(respond);
        }

        class ToteString 
        {
            public ToteString(string[] text)
            {
                Text = text;
            }
            public string[] Text { get; set; }
        }

        public void GetTote(int auctionId)
        {
            CocktionContext db = new CocktionContext();
            var auction = db.Auctions.Find(auctionId);
            UpdateToteBoard(auctionId, auction.AuctionToteBoard.CountAllCoefficientsForProducts());
        }
        
    }
}
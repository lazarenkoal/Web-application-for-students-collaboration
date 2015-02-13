using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using CocktionMVC.Models.DAL;
namespace CocktionMVC.Functions
{
    /// <summary>
    /// Контейнер для методов, позволяющих
    /// генерировать победителей и проводить с ними
    /// всяческие операции
    /// </summary>
    public class WinnerGenerator
    {
        /// <summary>
        /// Метод, генерирующий рандомно победителя аукциона.
        /// Обновляет результат в базе данных;
        /// </summary>
        /// <param name="db">База, в которую надо все сохранять</param>
        /// <param name="auction">Аукцион, который надо закончить</param>
        /// <returns>Айди победителя (строка)</returns>
        public async Task<string> GenerateWinner(CocktionContext db, Auction auction)
        {
            //получаем количество участников
            int amountOfProducts = auction.BidProducts.Count(); 
            
            //выбор рандомного товара из тех, что есть в аукционе
            Random rnd = new Random();
            int winnersNumber = rnd.Next(0, amountOfProducts);

            //заканчиваем аукцион
            auction.WinnerChosen = true;
            auction.WinProductId = auction.BidProducts.ElementAt(winnersNumber).Id.ToString();

            //завершаем все действия
            await Task.Run(() => db.SaveChangesAsync());
            return auction.WinProductId;
        }
    }
}
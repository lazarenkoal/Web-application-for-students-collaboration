using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.ComponentModel.DataAnnotations;
namespace CocktionMVC.Models
{
    public class AuctionModels
    {
        public class Product
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Без имени нельзя сделать ставку!")]
            [StringLength(150, ErrorMessage = "Имя не должно превышать 150 знаков")]
            [Display(Name = "Название того, что хотите продавать")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Пожалуйста, добавьте все-таки описание")]
            [StringLength(350, ErrorMessage = "Описание не должно превышать 350 символов")]
            [Display(Name = "Описание вашего товара")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Пожалуйста укажите категорию товара")]
            [StringLength(50, ErrorMessage = "Название категории не должно превышать 50 символов!")]
            [Display(Name = "Категория, к которой относится ваш товар")]
            public string Category { get; set; }

            //TODO: Добавить вставку фоток
            //public string PhotoUrl { get; set; }

            public int Rating { get; set; }

            [Required(ErrorMessage = "Необходимо указать длительность аукциона!")]
            [Range(0, 48)]
            [Display(Name = "Длительность аукциона (в часах)")]
            public int OnAuctionTime { get; set; }

        }

        /// <summary>
        /// Всем участникам аукциона видно, кто какие ставит ставки.
        /// У продавца есть возможность указать приемлимую, минимальную
        /// для него цену, которую он готов принять.
        /// Ставка каждого игрока должна быть выше ставки предыдущего.
        /// Игрок с самой высокой ставкой забирает лот.
        /// Продавец сам определяет, какой лот в данный момент для него самый
        /// ценный.
        /// Хочу поставить здесь еще и выбор интервала по времени для него.
        /// 
        /// P.S надо запомнить, что с чем для него ценно, чтобы потом иметь возможность
        /// сравнивать полезность товаров для него.
        /// </summary>
        public class EnglishAuction
        {
            public int Id { get; set; }
            //username property
            public string Owner { get; set; }
            public int ProductId { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }

        }


        /// <summary>
        /// Как видно из названия - это слепой аукцион.
        /// Никто из покупателей не знает ставок других покупателей.
        /// Соответственно, торг кончается тогда, когда продавцу понравится
        /// конкретный товар.
        /// 
        /// Есть возможность поставить таймлимит.
        /// </summary>
        public class BlindAuction
        {

        }
    }
}
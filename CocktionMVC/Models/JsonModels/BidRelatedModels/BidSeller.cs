namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Моделирует пользователя при окончании аукциона
    /// </summary>
    public class BidSeller
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип посетителя аукциона.
        /// Всего их существует несколько типов.
        /// 1)Owner_undfnd - владелец, который не выбрал победителя
        /// 2)Winner - победитель аукциона
        /// 3)Owner - владелец аукциона
        /// 4)Looser - проигравший человек
        /// 5)Info - любой другой пользователь
        /// (неавторизованный / не участник аукциона)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Сообщение, которое будет ему показано
        /// на экране при завершении аукциона
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Айдишник пользователя, для которого
        /// был создан объект BidSeller
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Прибыль, которую пользователь получил по результат тотализатора
        /// </summary>
        public int? ProfitFromTote { get; set; }

        /// <summary>
        /// Количество яиц, которое он получил.
        /// </summary>
        public int? CurrentEggsAmount { get; set; }
    }
}
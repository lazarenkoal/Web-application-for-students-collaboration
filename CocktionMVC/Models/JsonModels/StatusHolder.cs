namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Контенер для стандартного ответа
    /// сервера.
    /// 
    /// СИСТЕМНЫЙ КЛАСС
    /// </summary>
    public class StatusHolder
    {
        public StatusHolder() { }

        public StatusHolder(bool isSuccessful)
        {
            if (isSuccessful)
                Status = "Success";
            else
                Status = "Failure";
        }
        public string Status { get; set; }
    }

    public class StatusHolderWithError : StatusHolder
    {
        public string Message { get; set; }

        public StatusHolderWithError(bool isSuccessful, string message)
            : base(isSuccessful)
        {
            Message = message;
        }
    }
}
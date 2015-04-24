namespace CocktionMVC.Models.JsonModels
{
    /// <summary>
    /// Контенер для стандартного ответа
    /// сервера
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
}
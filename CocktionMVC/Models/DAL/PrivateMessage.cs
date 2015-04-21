using System;
using System.Web.UI.WebControls;

namespace CocktionMVC.Models.DAL
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string ReceiverName { get; set; }
        public string AuthorName { get; set; }
        public DateTime DateOfPublishing { get; set; }

        public PrivateMessage(string message, string author, string receiver,
            DateTime date)
        {
            Content = message;
            AuthorName = author;
            ReceiverName = receiver;
            DateOfPublishing = date;
        }

        public PrivateMessage()
        {

        }
    }
}
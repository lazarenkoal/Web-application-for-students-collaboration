using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CocktionMVC.Models.DAL
{
    public class UsersFeedback
    {
        public int Id { get; set; }
        public string AuthorsName { get; set; }
        public string AuthorsSurname { get; set; }
        public string AuthorsId { get; set; }
        public string UsersId { get; set; }
        public string Message { get; set; }

        public UsersFeedback()
        { }

        public UsersFeedback(string authorsName, string authorsSurname, string authorsId,
            string usersId, string message)
        {
            AuthorsName = authorsName;
            AuthorsSurname = authorsSurname;
            AuthorsId = authorsId;
            UsersId = usersId;
            Message = message;
        }
    }
}
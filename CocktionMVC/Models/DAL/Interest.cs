using System.Collections.Generic;

namespace CocktionMVC.Models.DAL
{
    public class Interest
    {
        public Interest()
        {
            Subscribers = new List<AspNetUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Picture Photocard { get; set; }
        public virtual ICollection<AspNetUser> Subscribers { get; set; }
    }
}
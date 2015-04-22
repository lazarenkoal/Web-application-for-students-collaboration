using System.Collections.Generic;

namespace CocktionMVC.Models.DAL
{
    public class Interest
    {
        public Interest()
        {
            Subscribers = new List<AspNetUser>();
        }
        
        public Interest(string name, Picture photoCard)
        {
            Subscribers = new List<AspNetUser>();
            Name = name;
            Photocard = photoCard;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Picture Photocard { get; set; }
        public virtual ICollection<AspNetUser> Subscribers { get; set; }
    }
}
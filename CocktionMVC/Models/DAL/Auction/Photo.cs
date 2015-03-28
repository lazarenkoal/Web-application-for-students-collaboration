using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models.DAL
{
    public partial class Photo
    {
        public Photo()
        {
            ThumbnailSets = new HashSet<ThumbnailSet>();
        }

        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string FileName { get; set; }

        public int Product_Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ThumbnailSet> ThumbnailSets { get; set; }
    }
}

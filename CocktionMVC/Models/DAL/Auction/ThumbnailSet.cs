using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocktionMVC.Models.DAL
{
    [Table("ThumbnailSet")]
    public partial class ThumbnailSet
    {
        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string FileName { get; set; }

        public int Photo_Id { get; set; }

        public virtual Photo Photo { get; set; }
    }
}

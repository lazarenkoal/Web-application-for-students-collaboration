namespace CocktionMVC.Models.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

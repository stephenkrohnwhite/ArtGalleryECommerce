using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [ForeignKey("ProductRef")]
        public int ProductId { get; set; }
        public Products ProductRef { get; set; }

        [Display(Name = "Date")]
        public DateTime ReviewDate { get; set; }

        public string Artist { get; set; }

        [Display(Name = "Customer Review")]
        public string ReviewText { get; set; }
    }
}
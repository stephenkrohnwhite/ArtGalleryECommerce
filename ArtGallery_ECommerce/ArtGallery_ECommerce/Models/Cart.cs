using ArtGallery_ECommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class Cart
    {
       
            [Key]
            public int Id { get; set; }

            public string CartId { get; set; }
            [ForeignKey("Product")]
            public int ProductId { get; set; }
            public Products Product { get; set; }
           
            public int Count { get; set; }

            [Column(TypeName = "datetime2")]
            public DateTime DateCreated { get; set; }

            
            
    }
}
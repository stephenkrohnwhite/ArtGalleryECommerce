using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
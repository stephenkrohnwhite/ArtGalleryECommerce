using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class Order
    {
        [Key]
        [Display(Name ="Order ID")]
        public int OrderId { get; set; }

        public int Quantity { get; set; }
        [Display(Name = "Tracking Number")]
        public string TrackingId { get; set; }

        public List<Cart> CartItems { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; set; }

        [ForeignKey("OrderStatus")]
        public int StatusId { get; set; }

        [Display(Name = "Order Status")]
        public Status OrderStatus { get; set; }

        public IEnumerable<Status> StatusList { get; set; }

        [ForeignKey("Buyer")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public Customer Buyer { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Order Total")]
        public double Total { get; set; }
        
        

    }
}
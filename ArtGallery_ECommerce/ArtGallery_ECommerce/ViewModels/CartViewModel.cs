using ArtGallery_ECommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.ViewModels
{
    public class CartViewModel
    {
        Customer Buyer { get; set; }
        public IEnumerable<Products> Cart { get; set; }
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public Double CartTotal { get; set; }

    }
}
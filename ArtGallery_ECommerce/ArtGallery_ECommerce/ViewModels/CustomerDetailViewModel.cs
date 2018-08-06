using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.ViewModels
{
    public class CustomerDetailViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Display(Name ="City, State & Zipcode")]
        public string CityStateZip { get; set; }


        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
    }
}
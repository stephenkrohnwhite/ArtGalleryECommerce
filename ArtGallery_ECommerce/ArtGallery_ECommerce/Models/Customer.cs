using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public string UserID { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Products> Cart { get; set; }
        [DataType(DataType.Currency)]
        public double Amount { get; set; }
    }
}
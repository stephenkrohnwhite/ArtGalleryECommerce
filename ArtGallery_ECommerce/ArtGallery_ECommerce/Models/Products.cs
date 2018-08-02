using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public string Artist { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Available")]
        public bool InStock { get; set; }

        public string Description { get; set; }

        [ForeignKey("Size")]
        public int ProductSizeId { get; set; }

        [Display(Name = "Size")]
        public ProductSize Size { get; set; }

        public IEnumerable<ProductSize> SizeList { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Categories Category { get; set; }

        public IEnumerable<Categories> CategoriesList { get; set; }
        [Display(Name="Image")]
        public string FileImagePath { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtGallery_ECommerce.Models;

namespace ArtGallery_ECommerce.ViewModels
    {
        public class StoreFrontIndexViewModel : IEnumerable<Products>
        {
            public Categories Category { get; set; }

            public IEnumerable<Products> DisplayProducts { get; set; }

            public IEnumerable<Products> AllProducts { get; set; }


            public IEnumerator<Products> GetEnumerator()
            {
                foreach (Products product in DisplayProducts)
                {
                    if (product == null)
                    {
                        break;
                    }
                    yield return product;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }

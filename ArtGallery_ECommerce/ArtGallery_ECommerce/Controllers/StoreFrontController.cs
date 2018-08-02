using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArtGallery_ECommerce.Models;
using ArtGallery_ECommerce.ViewModels;

namespace ArtGallery_ECommerce.Controllers
{
    public class StoreFrontController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: StoreFront
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var viewModel = new StoreFrontIndexViewModel()
                {
                    Category = null,
                    DisplayProducts = db.Products.Where(c => c.InStock == true).ToList()
                };

                return View(viewModel);
            }

            Categories category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                var viewModel = new StoreFrontIndexViewModel()
                {
                    Category = category,
                    DisplayProducts = db.Products.ToList().Where(c => c.InStock == true && c.Category == category).AsEnumerable()
                };
                return View(viewModel);
            }
        }
    }
}
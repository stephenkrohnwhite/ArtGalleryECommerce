using ArtGallery_ECommerce.Models;
using ArtGallery_ECommerce.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtGallery_ECommerce.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new CartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
                //Use Lock to prevent multiple purchase of same item
            };
            
            
            return View(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var addedProduct = db.Products.Single(product => product.ProductId == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct.ProductId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var cartitem = db.Cart.FirstOrDefault(item => item.ProductId == id);
            var productName = db.Products.Where(p => cartitem.ProductId == p.ProductId).First(); 
            int itemCount = cart.RemoveFromCart(id);

            var results = new CartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName.Name) + " has been removed from your shopping cart",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }
    }
}
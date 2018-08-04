using ArtGallery_ECommerce.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtGallery_ECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private double CartTotal;
        private int StripeTotal;
        
        // GET: Checkout
        public ActionResult Index(int? messager)
        {
            if(messager == null)
            {
                var stripePublishKey = ConfigurationManager.AppSettings["pk_test_19AURg22luvozWqAuOChS8uC"];
                var cart = ShoppingCart.GetCart(this.HttpContext);
                CartTotal = cart.GetTotal();
                StripeTotal = Convert.ToInt32(CartTotal * 100);
                ViewBag.StripeTotal = StripeTotal.ToString();
                ViewBag.CashTotal = CartTotal.ToString();
                ViewBag.StripePublishKey = stripePublishKey;
                return View();
            }
            if(messager == 1)
            {
                ViewBag.Message = "Some products in your cart are no longer available";
                return View();
            }
            return View();
            
        }
       
        public ActionResult Charge(string stripeEmail, string stripeToken)
        {  
            try
            {
                Order order = ProcessToDatabase(this.HttpContext);
                {
                    foreach(var item in order.CartItems)
                    {
                        if(item.Product.InStock == false)
                        {
                            return RedirectToAction("Index", "Checkout", 1);
                        }
                    }
                }
                db.Order.Add(order);
                db.SaveChanges();
                
                
            }
            catch
            {
                throw new Exception();  
                
            }
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            // Need to not hard code total as amount
            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = 4900,//charge in cents
                Description = "Sample Charge",
                Currency = "usd",
                CustomerId = customer.Id
            });
            return RedirectToAction("Index", "Home");
        }

        private Order ProcessToDatabase(HttpContextBase context)
        {
            DateTime dt = DateTime.Now;
            var cart = ShoppingCart.GetCart(context);
            Order order = new Order();
            order.CartItems = cart.GetCartItemsNoTrack();
            foreach(var item in order.CartItems)
            {
                if(item.Product.InStock == false)
                {
                    ViewBag.Message = string.Format("Item in cart no longer available");
                }
            }
            order.Total = cart.GetTotal();
            order.Quantity = cart.GetCount();
            var customer = db.Customer.Where(c => c.Email == cart.ShoppingCartId).First();
            order.CustomerId = customer.CustomerId;
            order.Buyer = customer;
            order.OrderStatus = db.Status.Where(s => s.Name == "Processing").First();
            order.StatusId = order.OrderStatus.StatusId;
            order.OrderTime = dt;
            cart.EmptyCart();
            return order;
            
        }

    }
}
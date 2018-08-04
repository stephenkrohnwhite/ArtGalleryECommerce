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
        public ActionResult Index()
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
        public ActionResult Charge(string stripeEmail, string stripeToken)
        {
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
            try
            {
                ProcessToDatabase(this.HttpContext);
            }
            catch
            {
                throw new Exception();
                
            }
            // need to add create order helper to build a order using cart/products/customer and save to db

            // further application specific code goes here

            return RedirectToAction("Index", "Home");
        }

        private void ProcessToDatabase(HttpContextBase context)
        {
            DateTime dt = DateTime.Now;
            var cart = ShoppingCart.GetCart(context);
            Order order = new Order();
            order.CartItems = cart.GetCartItemsNoTrack();
            order.Total = cart.GetTotal();
            order.Quantity = cart.GetCount();
            var customer = db.Customer.Where(c => c.Email == cart.ShoppingCartId).First();
            order.CustomerId = customer.CustomerId;
            order.Buyer = customer;
            order.OrderStatus = db.Status.Where(s => s.Name == "Processing").First();
            order.StatusId = order.OrderStatus.StatusId;
            order.OrderTime = dt;
            db.Order.Add(order);
            db.SaveChanges();

        }
        //[HttpPost]
        //public ActionResult AddressAndPayment(FormCollection values)
        //{
        //    var order = new Customer();

        //    TryUpdateModel(order);

        //    try
        //    {

        //            //order.CustomerUserName = User.Identity.Name;
        //            //order.DateCreated = DateTime.Now;

        //            //db.CustomerOrders.Add(order);
        //            //db.SaveChanges();

        //            var cart = ShoppingCart.GetCart(this.HttpContext);
        //            cart.CreateOrder(order);

        //            db.SaveChanges();//we have received the total amount lets update it

        //            return RedirectToAction("Complete", new { id = order.CustomerId });

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.InnerException.ToString();
        //        return View(order);
        //    }
        //}

        //public ActionResult Complete(int id)
        //{
        //    bool isValid = db.CustomerOrders.Any(
        //        o => o.Id == id &&
        //             o.CustomerUserName == User.Identity.Name
        //        );

        //    if (isValid)
        //    {
        //        return View(id);
        //    }
        //    else
        //    {
        //        return View("Error");
        //    }
        //}
    }
}
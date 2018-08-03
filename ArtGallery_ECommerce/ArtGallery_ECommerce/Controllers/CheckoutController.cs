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
        // GET: Checkout
        public ActionResult Index()
        {
            var stripePublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];
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

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = 500,//charge in cents
                Description = "Sample Charge",
                Currency = "usd",
                CustomerId = customer.Id
            });

            // further application specific code goes here

            return View();
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
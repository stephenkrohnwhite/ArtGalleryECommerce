using ArtGallery_ECommerce.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
            if (messager == 2)
            {
                ViewBag.Message = "Order could not be processed";
                return View();
            }
            return View();
            
        }
       
        public ActionResult Charge(string stripeEmail, string stripeToken)
        {  
            try
            {
                var order = ProcessToDatabase(this.HttpContext);
                var cart = ShoppingCart.GetCart(this.HttpContext);
                var cartProduct = cart.GetCartItems();
                foreach (var item in cartProduct)
                {
                    Products product = db.Products.Where(p => p.ProductId == item.ProductId).First();
                    if(product.InStock == false)
                    {
                        return RedirectToAction("Index", "Checkout", 1);
                    }
                }
                db.Order.Add(order);
                db.SaveChanges();
                
                
                foreach(var item in cartProduct)
                {
                    OrderItem oi = new OrderItem();
                    oi.OrderId = order.OrderId;
                    oi.Order = order;
                    oi.ProductId = item.ProductId;
                    oi.Product = item.Product;
                    db.OrderItems.Add(oi);
                    db.SaveChanges();
                }
                order.PublicOrderNum = GetRandomizedString(order.OrderId);
                db.SaveChanges();
                var orderList = db.OrderItems.Where(o => o.OrderId == order.OrderId).ToList();
                foreach(var item in orderList)
                {
                    Products product = db.Products.Where(p => p.ProductId == item.ProductId).First();
                    product.InStock = false;
                    db.SaveChanges();
                }
                
                cart.EmptyCart();
            }
            catch
            {
                return RedirectToAction("Index", "Checkout", 2);


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
            order.Total = cart.GetTotal();
            order.Quantity = cart.GetCount();
            var customer = db.Customer.Where(c => c.Email == cart.ShoppingCartId).First();
            order.CustomerId = customer.CustomerId;
            order.Buyer = customer;
            order.OrderStatus = db.Status.Where(s => s.Name == "Processing").First();
            order.StatusId = order.OrderStatus.StatusId;
            order.OrderTime = dt;
            return order;
            
        }
        static String GetRandomizedString(Int32 input)
        {
            Int32 uniqueLength = 6;
            Int32 randomLength = 4;
            String uniqueString;
            String randomString;
            StringBuilder resultString = new StringBuilder(uniqueLength + randomLength);

            Random randomizer = new Random(
                    (Int32)(
                        DateTime.Now.Ticks + (DateTime.Now.Ticks > input ? DateTime.Now.Ticks / (input + 1) : input / DateTime.Now.Ticks)
                    )
                );

            randomString = EncodeInt32AsString(randomizer.Next(1, Int32.MaxValue), randomLength);
            uniqueString = EncodeInt32AsString(input, uniqueLength);

            resultString.AppendFormat("{0}\t {1}\t ", uniqueString, randomString);

            for (Int32 i = 0; i < Math.Min(uniqueLength, randomLength); i++)
            {
                resultString.AppendFormat("{0}{1}", uniqueString[i], randomString[i]);
            }
            resultString.Append((uniqueLength < randomLength ? randomString : uniqueString).Substring(Math.Min(uniqueLength, randomLength)));

            return resultString.ToString();
        }

        static String EncodeInt32AsString(Int32 input, Int32 maxLength = 0)
        {
            Char[] allowedList = new Char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z' };
            Int32 allowedSize = allowedList.Length;
            StringBuilder result = new StringBuilder(input.ToString().Length);

            Int32 moduloResult;
            while (input > 0)
            {
                moduloResult = input % allowedSize;
                input /= allowedSize;
                result.Insert(0, allowedList[moduloResult]);
            }

            if (maxLength > result.Length)
            {
                result.Insert(0, new String(allowedList[0], maxLength - result.Length));
            }

            if (maxLength > 0)
                return result.ToString().Substring(0, maxLength);
            else
                return result.ToString();
        }
    }
}
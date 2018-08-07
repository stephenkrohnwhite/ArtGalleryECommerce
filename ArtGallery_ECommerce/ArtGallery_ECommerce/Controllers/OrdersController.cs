using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ArtGallery_ECommerce.Models;
using ArtGallery_ECommerce.Utils;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.AspNet.Identity;

namespace ArtGallery_ECommerce.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
         
            if (User.IsInRole("Employee"))
            {
                var order = db.Order.Include(o => o.Buyer).Include(o => o.OrderStatus);
                return View(order.ToList());
            }
            if (User.IsInRole("Customer"))
            {
                var userId = User.Identity.GetUserId();
                var customerId = db.Customer.Where(c => c.UserID == userId).First().CustomerId;
                var orders = db.Order.Include(m => m.Buyer).Include(m => m.OrderStatus).Where(m => m.CustomerId == customerId).ToList();
                return View(orders);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult PickList()
        {
            var pickList = db.Order.Where(c => c.OrderStatus.Name == "Processing").Include(o => o.OrderStatus).ToList();
            return View("PickList", pickList);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order = db.Order.Where(o => o.OrderId == order.OrderId).Include(o => o.Buyer).Include(o => o.OrderStatus).First();
            var items = db.OrderItems.Where(i => i.OrderId == order.OrderId).ToList();
            var productList = GetOrderProducts(items);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", order.StatusId);
            ViewBag.ProductList = productList;
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "FirstName");
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProductId,CustomerOrderId,Quantity,TrackingId,OrderTime,StatusId,CustomerId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Order.Add(order);
        //        db.SaveChanges();
        //        order.PublicOrderNum = GetRandomizedString(order.OrderId);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "FirstName", order.CustomerId);
        //    ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", order.StatusId);
        //    return View(order);
        //}

        //static String GetRandomizedString(Int32 input)
        //{
        //    Int32 uniqueLength = 6; 
        //    Int32 randomLength = 4;
        //    String uniqueString;
        //    String randomString;
        //    StringBuilder resultString = new StringBuilder(uniqueLength + randomLength);

        //    Random randomizer = new Random(
        //            (Int32)(
        //                DateTime.Now.Ticks + (DateTime.Now.Ticks > input ? DateTime.Now.Ticks / (input + 1) : input / DateTime.Now.Ticks)
        //            )
        //        );

        //    randomString = EncodeInt32AsString(randomizer.Next(1, Int32.MaxValue), randomLength);           
        //    uniqueString = EncodeInt32AsString(input, uniqueLength);
 
        //    resultString.AppendFormat("{0}\t {1}\t ", uniqueString, randomString);

        //    for (Int32 i = 0; i < Math.Min(uniqueLength, randomLength); i++)
        //    {
        //        resultString.AppendFormat("{0}{1}", uniqueString[i], randomString[i]);
        //    }
        //    resultString.Append((uniqueLength < randomLength ? randomString : uniqueString).Substring(Math.Min(uniqueLength, randomLength)));

        //    return resultString.ToString();
        //}

        //static String EncodeInt32AsString(Int32 input, Int32 maxLength = 0)
        //{ 
        //    Char[] allowedList = new Char[] {
        //    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        //    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        //    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        //    'U', 'V', 'W', 'X', 'Y', 'Z' };
        //    Int32 allowedSize = allowedList.Length;
        //    StringBuilder result = new StringBuilder(input.ToString().Length);

        //    Int32 moduloResult;
        //    while (input > 0)
        //    {
        //        moduloResult = input % allowedSize;
        //        input /= allowedSize;
        //        result.Insert(0, allowedList[moduloResult]);
        //    }

        //    if (maxLength > result.Length)
        //    {
        //        result.Insert(0, new String(allowedList[0], maxLength - result.Length));
        //    }

        //    if (maxLength > 0)
        //        return result.ToString().Substring(0, maxLength);
        //    else
        //        return result.ToString();
        //}

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var items = db.OrderItems.Where(i => i.OrderId == order.OrderId).ToList();
            var productList = GetOrderProducts(items);
            //var products = db.Products.Where(p => p.ProductId == items.P)

            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", order.StatusId);
            ViewBag.ProductList = new SelectList(productList, "Product", "Name");
            return View(order);
        }

        private IEnumerable<Products> GetOrderProducts(List<OrderItem> items)
        {
            var products = new List<Products>();
            foreach(var item in items)
            {
                Products product = db.Products.Where(p => p.ProductId == item.ProductId).First();
                products.Add(product);
            }
            return products;
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "OrderId,PublicOrderNum,OrderStatus,StatusList,Buyer,Total,ProductId,CustomerOrderId,Quantity,TrackingId,OrderTime,StatusId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderStatus = db.Status.Where(s => s.StatusId == order.StatusId).First();
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                if(order.StatusId == 2)
                {
                    string subject = "Order processed";
                    string to = "paintedplunders@gmail.com"; // normally would use order.buyer.email, but for testing/demo purposes we are hardcoding
                    string body = "Thank you for choosing Painted Plunders!";
                    APIFucnction.SendSimpleMessage(subject, to, body);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "FirstName", order.CustomerId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", order.StatusId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

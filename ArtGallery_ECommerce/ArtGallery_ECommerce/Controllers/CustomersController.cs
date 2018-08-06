using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArtGallery_ECommerce.Models;
using ArtGallery_ECommerce.ViewModels;
using Microsoft.AspNet.Identity;

namespace ArtGallery_ECommerce.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var customer = db.Customer.Include(c => c.Address);
            return View(customer.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            Customer customer = null;
            Address currentAddress = null;

            if (id == null)
            {
                var userId = User.Identity.GetUserId();
                customer = db.Customer.Where(c => c.UserID == userId).First();
                currentAddress = db.Address.Where(c => c.AddressId == customer.AddressId).First();
            }
            if (id != null)
            {
                customer = db.Customer.Find(id);
                currentAddress = db.Address.Where(c => c.AddressId == customer.AddressId).First();
            }

  
            var viewModel = new CustomerDetailViewModel()
            {
                CustomerId = customer.CustomerId,
                Name = customer.FirstName + " " + customer.LastName,
                Email = customer.Email,
                StreetAddress = currentAddress.SteetAddress,
                CityStateZip = currentAddress.City + ", " + currentAddress.State + " " + currentAddress.ZipCode,
                PhoneNumber = customer.Phone
            };
            return View(viewModel);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "SteetAddress");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,Phone,UserID,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string currentId = User.Identity.GetUserId();
                var userJunction = db.Users.Where(u => u.Id == currentId).First();
                customer.UserID = userJunction.Id;
                customer.Email = userJunction.Email;

                Address newAddress = new Address();
                newAddress.SteetAddress = customer.Address.SteetAddress;
                newAddress.City = customer.Address.City;
                newAddress.State = customer.Address.State;
                newAddress.ZipCode = customer.Address.ZipCode;
                db.Address.Add(newAddress);
                db.SaveChanges();

                var thisAddress = db.Address.Where(a => a.SteetAddress == customer.Address.SteetAddress).First();
                customer.Address.AddressId = thisAddress.AddressId;

                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "SteetAddress", customer.AddressId);
            return View(customer);
        }

 
        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "SteetAddress", customer.AddressId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,FirstName,LastName,Email,Phone,UserID,AddressId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "SteetAddress", customer.AddressId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
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

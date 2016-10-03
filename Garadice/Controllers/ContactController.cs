using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace Garadice.Controllers
{
    public class ContactController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Contact
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            string order = sortOrder ?? "";

            ViewBag.CurrentSort = order;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var contacts = from c in db.Contacts
                            select c;



            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString) || c.Email.Contains(searchString) || c.Description.Contains(searchString));
            }

            switch (order)
            {
                case "date_asc":
                    contacts = contacts.OrderBy(c => c.CreatedDate);
                    break;
                case "date_desc":
                    contacts = contacts.OrderByDescending(c => c.CreatedDate);
                    break;
                case "lastname_desc":
                    contacts = contacts.OrderByDescending(c => c.LastName);
                    break;
                case "lastname_asc":
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
                case "name_desc":
                    contacts = contacts.OrderByDescending(c => c.FirstName);
                    break;
                case "name_asc":
                default:
                    contacts = contacts.OrderBy(c => c.FirstName);
                    break;

            }

            ViewBag.Total = contacts.Count();

            contacts = contacts.Include(d => d.Company);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name");
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Phone,Mobile,Email,Position,Description,CompanyID")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }

            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", contact.CompanyID);
            return View(contact);
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", contact.CompanyID);
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ContactID,FirstName,LastName,Phone,Mobile,Email,Position,Description,CreatedDate,CompanyID")] Contact contact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(contact).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", contact.CompanyID);
        //    return View(contact);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contactToUpdate = db.Contacts.Find(id);
            if (TryUpdateModel(contactToUpdate, "",
               new string[] { "FirstName","LastName","Phone","Mobile","Email","Position","Description","CompanyID" }))
            {
                try
                {
                    db.Entry(contactToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, email to support@upinthecloud.ie");
                }
            }
            return View(contactToUpdate);
        }


        // GET: Contact/Delete/5
        public ActionResult Delete(int? id, Boolean? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.Msg = "Delete failed. Try again, and if the problem persists, email to support@upinthecloud.ie";
            }
            else
            {
                ViewBag.Msg = "IF YOU CLICK THE BUTTON DELETE, THIS CONTACT WILL BE DELETED AND YOU CAN NOT RECOVER THIS DATA ANYMORE.";
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Contact contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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

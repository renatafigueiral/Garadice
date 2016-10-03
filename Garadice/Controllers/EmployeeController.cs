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
    public class EmployeeController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Employee
        //public ActionResult Index()
        //{
        //    return View(db.Employees.ToList());
        //}
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
            var employees = from e in db.Employees
                           select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FirstName.Contains(searchString) || e.LastName.Contains(searchString) || e.Email.Contains(searchString) || e.Description.Contains(searchString));
            }

            switch (order)
            {
                case "date_asc":
                    employees = employees.OrderBy(e => e.CreatedDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(e => e.CreatedDate);
                    break;
                case "lastname_desc":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;
                case "lastname_asc":
                    employees = employees.OrderBy(e => e.LastName);
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.FirstName);
                    break;
                case "name_asc":
                default:
                    employees = employees.OrderBy(e => e.FirstName);
                    break;

            }

            ViewBag.Total = employees.Count();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
        }
        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Phone,Mobile,Email,Position,Description")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "FirstName,LastName,Phone,Mobile,Email,Position,Description")] Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(employee).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employeeToUpdate = db.Employees.Find(id);
            if (TryUpdateModel(employeeToUpdate, "",
               new string[] { "FirstName", "LastName", "Phone", "Mobile", "Email", "Position", "Description"}))
            {
                try
                {
                    db.Entry(employeeToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, email to support@upinthecloud.ie");
                }
            }
            return View(employeeToUpdate);
        }


        // GET: Employee/Delete/5
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
                ViewBag.Msg = "IF YOU CLICK THE BUTTON DELETE, THIS EMPLOYEE WILL BE DELETED AND YOU CAN NOT RECOVER THIS DATA ANYMORE.";
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
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

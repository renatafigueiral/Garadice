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
    public class JobController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Job
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

            var jobs = from j in db.Jobs
                       select j;

            if (!String.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j => j.JobName.Contains(searchString) || j.Description.Contains(searchString));
            }

            switch (order)
            {
                case "date_asc":
                    jobs = jobs.OrderBy(c => c.CreatedDate);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(c => c.CreatedDate);
                    break;
                case "name_desc":
                    jobs = jobs.OrderByDescending(c => c.JobName);
                    break;
                case "name_asc":
                default:
                    jobs = jobs.OrderBy(c => c.JobName);
                    break;
             }

            ViewBag.Total = jobs.Count();

            jobs = jobs.Include(j => j.Company).Include(j => j.Employee);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(jobs.ToPagedList(pageNumber, pageSize));

        }

        // GET: Job/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FullName");
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobType,JobName,Description,EmployeeID,CompanyID,EstimatedDuration")] Job job)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    job.JobStatus = JobStatus.Assigned;
                    db.Jobs.Add(job);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", job.CompanyID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FullName", job.EmployeeID);
            return View(job);
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", job.CompanyID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FullName", job.EmployeeID);
            return View(job);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "JobID,JobType,JobName,Description,EmployeeID,CompanyID,EstimatedDuration,JobStatus,CreatedDate")] Job job)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(job).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", job.CompanyID);
        //    ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", job.EmployeeID);
        //    return View(job);
        //}


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobToUpdate = db.Jobs.Find(id);
            if (TryUpdateModel(jobToUpdate, "",
               new string[] { "JobType","JobName","Description","EmployeeID","CompanyID","EstimatedDuration","JobStatus" }))
            {
                try
                {
                    db.Entry(jobToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, email to support@upinthecloud.ie");
                }
            }
            return View(jobToUpdate);
        }

        // GET: Job/Delete/5
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
                ViewBag.Msg = "IF YOU CLICK THE BUTTON DELETE, THIS JOB WILL BE DELETED AND ALL TRACKS ASSOCIATED. YOU CAN NOT RECOVER THIS DATA ANYMORE.";
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Job job = db.Jobs.Find(id);
                db.Jobs.Remove(job);
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

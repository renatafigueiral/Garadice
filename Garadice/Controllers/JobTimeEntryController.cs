using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;

namespace Garadice.Controllers
{
    public class JobTimeEntryController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: JobTimeEntry
        public ActionResult Index()
        {
            var jobTimeEntry = db.JobTimeEntry.Include(j => j.Job);
            return View(jobTimeEntry.ToList());
        }

        // GET: JobTimeEntry/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTimeEntry jobTimeEntry = db.JobTimeEntry.Find(id);
            if (jobTimeEntry == null)
            {
                return HttpNotFound();
            }
            return View(jobTimeEntry);
        }

        // GET: JobTimeEntry/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName");
            return View();
        }

        // POST: JobTimeEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobTimeEntryID,Description,Start,StartTime,NumberOfHours,JobID")] JobTimeEntry jobTimeEntry)
        {
            if (ModelState.IsValid)
            {
                db.JobTimeEntry.Add(jobTimeEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTimeEntry.JobID);
            return View(jobTimeEntry);
        }

        // GET: JobTimeEntry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTimeEntry jobTimeEntry = db.JobTimeEntry.Find(id);
            if (jobTimeEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTimeEntry.JobID);
            return View(jobTimeEntry);
        }

        // POST: JobTimeEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobTimeEntryID,Description,Start,StartTime,NumberOfHours,JobID")] JobTimeEntry jobTimeEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobTimeEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTimeEntry.JobID);
            return View(jobTimeEntry);
        }

        // GET: JobTimeEntry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTimeEntry jobTimeEntry = db.JobTimeEntry.Find(id);
            if (jobTimeEntry == null)
            {
                return HttpNotFound();
            }
            return View(jobTimeEntry);
        }

        // POST: JobTimeEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobTimeEntry jobTimeEntry = db.JobTimeEntry.Find(id);
            db.JobTimeEntry.Remove(jobTimeEntry);
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

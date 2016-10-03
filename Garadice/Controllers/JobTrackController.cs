using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;

namespace Garadice.Controllers
{
    public class JobTrackController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: JobTrack
        public ActionResult Index()
        {
            var jobTracks = db.JobTracks.Include(j => j.Job);
            return View(jobTracks.ToList());
        }

        // GET: JobTrack/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTrack jobTrack = db.JobTracks.Find(id);
            if (jobTrack == null)
            {
                return HttpNotFound();
            }
            return View(jobTrack);
        }

        // GET: JobTrack/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName");
            return View();
        }

        // POST: JobTrack/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobTrackID,Description,Start,NumberOfHours,End,JobID")] JobTrack jobTrack)
        {
            if (ModelState.IsValid)
            {
                db.JobTracks.Add(jobTrack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTrack.JobID);
            return View(jobTrack);
        }

        // GET: JobTrack/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTrack jobTrack = db.JobTracks.Find(id);
            if (jobTrack == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTrack.JobID);
            return View(jobTrack);
        }

        // POST: JobTrack/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobTrackID,Description,Start,NumberOfHours,End,JobID")] JobTrack jobTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobName", jobTrack.JobID);
            return View(jobTrack);
        }

        // GET: JobTrack/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTrack jobTrack = db.JobTracks.Find(id);
            if (jobTrack == null)
            {
                return HttpNotFound();
            }
            return View(jobTrack);
        }

        // POST: JobTrack/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobTrack jobTrack = db.JobTracks.Find(id);
            db.JobTracks.Remove(jobTrack);
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

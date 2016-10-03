using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;
using System.Web;
using System.Data;
using System.IO;

namespace Garadice.Controllers
{
    public class DocumentControllerold : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Document
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Company);
            return View(documents.ToList());
        }

        // GET: Document/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Document/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name");
            //ViewBag.TypesDoc = Enum.GetNames(typeof(DocumentType)).ToList();
            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,DocumentType,Description,CompanyID")] Document document, HttpPostedFileBase file)
        {
            try { 
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        document.FileName = fileName;
                        document.Extension = Path.GetExtension(fileName).ToLower();
                        document.Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            document.Content = reader.ReadBytes(file.ContentLength);
                        }

                        var document2 = new Document()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName).ToLower(),
                            Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName),
                            DocumentType = DocumentType.AML,
                           // DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), typeFile1),
                            ContentType = file.ContentType,
                            CompanyID = document.CompanyID
                        };
                        db.Documents.Add(document2);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", document.CompanyID);
            return View(document);
        }

        // GET: Document/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", document.CompanyID);
            return View(document);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentID,Name,FileName,ContentType,Extension,Content,Path,DocumentType,Description,CreatedDate,CompanyID")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", document.CompanyID);
            return View(document);
        }

        // GET: Document/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Document/Download/5
        public FileResult Download(int? id)
        {
            Document document = db.Documents.Find(id);
            byte[] fileContent = document.Content.ToArray();
            return File(fileContent, document.ContentType, document.Path);   
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

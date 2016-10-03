using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;
using System.IO;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace Garadice.Controllers
{
    public class DocumentController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Document
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

            var documents = from c in db.Documents
                            select c;



            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(d => d.Name.Contains(searchString) || d.Description.Contains(searchString) || d.FileName.Contains(searchString));
            }

            switch (order)
            {
                case "date_asc":
                    documents = documents.OrderBy(c => c.CreatedDate);
                    break;
                case "date_desc":
                    documents = documents.OrderByDescending(c => c.CreatedDate);
                    break;
                case "name_desc":
                    documents = documents.OrderByDescending(c => c.Name);
                    break;
                case "name_asc":
                default:
                    documents = documents.OrderBy(c => c.Name);
                    break;
                //case "company_desc":
                //    documents = documents.OrderByDescending(c => c.Company);
                //    break;
                //case "company_asc":
                //default:
                //    documents = documents.OrderBy(c => c.Company);
                //    break;

            }
 
            ViewBag.Total = documents.Count();

            documents = documents.Include(d => d.Company);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(documents.ToPagedList(pageNumber, pageSize));
            //var documents = db.Documents.Include(d => d.Company);
            //return View(documents.ToList());
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
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();

            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,FileName,DocumentType,Description,CompanyID")] Document document, HttpPostedFileBase file)
        {
            try {
                if (file == null ||  file.ContentLength == 0)
                {
                    ViewBag.Msg="The File field is required";
                }
                if (ModelState.IsValid && (file != null && file.ContentLength > 0))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    document.FileName = fileName;
                    document.Extension = Path.GetExtension(fileName).ToLower();
                    document.Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    document.ContentType = file.ContentType;
                    using (var reader = new BinaryReader(file.InputStream))
                    {
                       document.Content = reader.ReadBytes(file.ContentLength);
                    }
                    db.Documents.Add(document);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", document.CompanyID);
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();
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
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();

            return View(document);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult Edit([Bind(Include = "Name,FileName,DocumentType,Description,CompanyID")] Document document)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(document).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", document.CompanyID);
        //    return View(document);
        //}


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, HttpPostedFileBase file)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var documentToUpdate = db.Documents.Find(id);
            if (TryUpdateModel(documentToUpdate, "",
               new string[] { "Name", "FileName", "DocumentType", "Description", "CompanyID" }))
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        documentToUpdate.FileName = fileName;
                        documentToUpdate.Extension = Path.GetExtension(fileName).ToLower();
                        documentToUpdate.Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                        documentToUpdate.ContentType = file.ContentType;
                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            documentToUpdate.Content = reader.ReadBytes(file.ContentLength);
                        }
                    }

                    db.Entry(documentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, email to support@upinthecloud.ie");
                }
            }
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();
            return View(documentToUpdate);
        }




        // GET: Document/Delete/5
        public ActionResult Delete(int? id,bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.Msg = "Delete failed. Try again, and if the problem persists, email to support@upinthecloud.ie";
            }else
            {
                ViewBag.Msg = "IF YOU CLICK THE BUTTON DELETE, THIS DOCUMENT WILL BE DELETED AND YOU CAN NOT RECOVER THIS DATA ANYMORE.";
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
            try{
                Document document = db.Documents.Find(id);
                db.Documents.Remove(document);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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

using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garadice.DAL;
using Garadice.Models;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Data;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace Garadice.Controllers
{
    public class CompanyController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        // GET: Company
        public ActionResult Index(string sortOrder, string   searchString, string currentFilter, int? page)
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

            var companies = from c in db.Companies
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(c => c.Name.Contains(searchString) || c.Description.Contains(searchString));
            }

            switch (order)
            {
                case "date_asc":
                    companies = companies.OrderBy(c => c.CreatedDate);
                    break;
                case "date_desc":
                    companies = companies.OrderByDescending(c => c.CreatedDate);
                    break;
                case "name_desc":
                    companies = companies.OrderByDescending(c => c.Name);
                    break;
                case "name_asc":
                default:
                    companies = companies.OrderBy(c => c.Name);
                    break;
            
            }


            ViewBag.Total= companies.Count();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(companies.ToPagedList(pageNumber, pageSize));

            //return View(companies.ToList());
        }

        // GET: Company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Company company = db.Companies.Find(id);
            Company company = db.Companies.Include(c => c.Documents).SingleOrDefault(c => c.CompanyID == id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,VatNumber,CRO,Phone,Email,SiteUrl,Description")] Company company, HttpPostedFileBase file1, string typeFile1, string nameFile1, string textFile1, HttpPostedFileBase file2, string typeFile2, string nameFile2, string textFile2, HttpPostedFileBase file3, string typeFile3, string nameFile3, string textFile3)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    //var file1 = Request.Form["file1"];
 
                    List<Document> Documents = new List<Document>();
                    if (file1 != null && file1.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file1.FileName);
                        var document = new Document()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName).ToLower(),
                            Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName),
                            //DocumentType = DocumentType.AML,
                            DocumentType = (DocumentType) Enum.Parse(typeof(DocumentType),typeFile1),
                            ContentType = file1.ContentType,
                            Name = nameFile1,
                            Description = textFile1
                        };
                        using (var reader = new BinaryReader(file1.InputStream))
                        {
                            document.Content = reader.ReadBytes(file1.ContentLength);
                        }
                    
                        Documents.Add(document);

                        //db.Documents.Add(document);
                        //db.SaveChanges();

                        //var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), document.Path);
                        //file1.SaveAs(path);
                    }

                    if (file2 != null && file2.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file2.FileName);
                        var document = new Document()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName).ToLower(),
                            Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName),
                            DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), typeFile2),
                            ContentType = file2.ContentType,
                            Name = nameFile2,
                            Description = textFile2
                        };
                        using (var reader = new BinaryReader(file2.InputStream))
                        {
                            document.Content = reader.ReadBytes(file2.ContentLength);
                        }

                        Documents.Add(document);
                    }
                    if (file3 != null && file3.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file3.FileName);
                        var document = new Document()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName).ToLower(),
                            Path = Guid.NewGuid().ToString() + Path.GetExtension(fileName),
                            DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), typeFile3),
                            ContentType = file3.ContentType,
                            Name = nameFile3,
                            Description = textFile3
                        };
                        using (var reader = new BinaryReader(file3.InputStream))
                        {
                            document.Content = reader.ReadBytes(file3.ContentLength);
                        }

                        Documents.Add(document);
                    }

                    company.Documents = Documents;
                    db.Companies.Add(company);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and if the problem persists email to support@upinthecloud.ie");
            }
            ViewData["DocumentTypes"] = Enum.GetNames(typeof(DocumentType)).ToList();
            return View(company);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Company company = db.Companies.Find(id);
            Company company = db.Companies.Include(c => c.Documents).SingleOrDefault(c => c.CompanyID == id);

            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Name,VatNumber,CRO,Phone,Email,SiteUrl,Description")] Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(company).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(company);
        //}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyToUpdate = db.Companies.Find(id);
            if (TryUpdateModel(companyToUpdate, "",
               new string[] { "Name", "VatNumber", "CRO", "Phone" , "Email", "SiteUrl", "Description"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException  /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, email to support@upinthecloud.ie");
                }
            }
            return View(companyToUpdate);
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int? id, Boolean?  saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.Msg = "Delete failed. Try again, and if the problem persists, email to support@upinthecloud.ie ";
            }
            else
            {
                ViewBag.Msg = "IF YOU CLICK THE BUTTON DELETE, THIS COMPANY AND  ALL DOCUMENTS/CONTACTS RELATED, WILL BE DELETED AND YOU CAN NOT RECOVER THIS DATA ANYMORE.";
            }
            //Company company = db.Companies.Find(id);
            Company company = db.Companies.Include(c => c.Documents).SingleOrDefault(c => c.CompanyID == id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Company company = db.Companies.Find(id);
                db.Companies.Remove(company);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
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

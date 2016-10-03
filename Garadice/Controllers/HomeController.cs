using Garadice.DAL;
using System.Linq;
using System.Web.Mvc;

namespace Garadice.Controllers
{
    public class HomeController : Controller
    {
        private GaradiceContext db = new GaradiceContext();

        public ActionResult Index()
        {
            ViewBag.TotalCompanies = db.Companies.Count();
            ViewBag.TotalDocuments = db.Documents.Count();
            ViewBag.TotalContacts = db.Contacts.Count();
            ViewBag.TotalEmployees = db.Employees.Count();
            ViewBag.TotalJobs = db.Jobs.Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
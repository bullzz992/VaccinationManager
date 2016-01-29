using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaccinationManager.DAL;
using VaccinationManager.ViewModels;


namespace VaccinationManager.Controllers
{
    public class HomeController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            // Commenting out LINQ to show how to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from Child in db.Children
            //           group Child by Child.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               ChildCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
         
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
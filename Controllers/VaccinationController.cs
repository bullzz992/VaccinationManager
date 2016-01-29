using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VaccinationManager.DAL;
using VaccinationManager.Models;
using System.Data.Entity.Infrastructure;

namespace VaccinationManager.Controllers
{
    public class VaccinationController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        // GET: Vaccination
        [Authorize]
        public ActionResult Index()
        {
            var Vaccinations = db.VaccinationDefinitions.//Include("Age").//Join(db.Ages, d => d.Id, a => a.VaccinationDefinition_Id, (d, a) => { }).
                OrderBy(q => q.Name).ToList();
            //ViewBag.SelectedVaccination = new SelectList(Vaccinations, "Id", "Code", SelectedVaccination);
            //int departmentID = SelectedVaccination.GetValueOrDefault();

            //var Vaccinations = db.Vaccinations.ToList();

            return View(Vaccinations);
        }

        // GET: Vaccination/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaccinationDefinition Vaccination = db.VaccinationDefinitions.Find(id);
            if (Vaccination == null)
            {
                return HttpNotFound();
            }
            return View(Vaccination);
        }


        public ActionResult Create()
        {
            PopulateVaccinationDefinitionDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Code, Name")]Vaccination Vaccination)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vaccinations.Add(Vaccination);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateVaccinationDefinitionDropDownList(Vaccination.Id);
            return View(Vaccination);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccination Vaccination = db.Vaccinations.Find(id);
            if (Vaccination == null)
            {
                return HttpNotFound();
            }
            PopulateVaccinationDefinitionDropDownList(Vaccination.Id);
            return View(Vaccination);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var VaccinationToUpdate = db.Vaccinations.Find(id);
            if (TryUpdateModel(VaccinationToUpdate, "",
               new string[] { "Title", "Credits", "DepartmentID" }))
            {
                try
                {
                    db.Entry(VaccinationToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateVaccinationDefinitionDropDownList(VaccinationToUpdate.Id);
            return View(VaccinationToUpdate);
        }

        private void PopulateVaccinationDefinitionDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.VaccinationDefinitions
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }


        // GET: Vaccination/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccination Vaccination = db.Vaccinations.Find(id);
            if (Vaccination == null)
            {
                return HttpNotFound();
            }
            return View(Vaccination);
        }

        // POST: Vaccination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vaccination Vaccination = db.Vaccinations.Find(id);
            db.Vaccinations.Remove(Vaccination);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateVaccinationCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateVaccinationCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewBag.RowsAffected = db.Database.ExecuteSqlCommand("UPDATE Vaccination SET Credits = Credits * {0}", multiplier);
            }
            return View();
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

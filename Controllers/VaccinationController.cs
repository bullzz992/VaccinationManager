﻿using System;
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
using DataAccessLayer;

namespace VaccinationManager.Controllers
{
    public class VaccinationController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        // GET: Vaccination
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CurrentPage = "Vaccination";
            var Vaccinations = db.VaccinationDefinitions.//Include("Age").//Join(db.Ages, d => d.Id, a => a.VaccinationDefinition_Id, (d, a) => { }).
                OrderBy(q => q.Age.Code).ToList();
            //ViewBag.SelectedVaccination = new SelectList(Vaccinations, "Id", "Code", SelectedVaccination);
            //int departmentID = SelectedVaccination.GetValueOrDefault();

            //var Vaccinations = db.Vaccinations.ToList();
            
            return View(Vaccinations);
        }

        // GET: Vaccination/Details/5
        public ActionResult Details(Guid? id)
        {
            ViewBag.CurrentPage = "Vaccination";
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
            ViewBag.CurrentPage = "Vaccination";
            List<SelectListItem> ls =  new List<SelectListItem>();

            foreach (Age item in db.Ages)
            {
                ls.Add(new SelectListItem {Text = item.Description, Value = item.Code.ToString()});
            }
            ViewBag.AgeGroups = ls;
            PopulateVaccinationDefinitionDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VaccinationDefinition Vaccination)
        {
            ViewBag.CurrentPage = "Vaccination";
            Vaccination.Age = db.Ages.FirstOrDefault(p => p.Code == Vaccination.Age.Code);
            try
            {
                if (ModelState.IsValid)
                {
                    Vaccination.Id = Guid.NewGuid();
                    VaccinationDefinition resultObj =  db.VaccinationDefinitions.Add(Vaccination);
                    db.SaveChanges();
                    DataAccessLayer.VaccincationPriceDal provider = new VaccincationPriceDal();
                    provider.AddVaccinationPrice(Vaccination.Id.ToString(), (Decimal) 0.0, true);
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
            ViewBag.CurrentPage = "Vaccination";

            List<SelectListItem> ls = new List<SelectListItem>();

            foreach (Age item in db.Ages)
            {
                ls.Add(new SelectListItem { Text = item.Description, Value = item.Code.ToString() });
            }
            ViewBag.AgeGroups = ls;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaccinationDefinition Vaccination = db.VaccinationDefinitions.Find(id);
            if (Vaccination == null)
            {
                return HttpNotFound();
            }
            PopulateVaccinationDefinitionDropDownList(Vaccination.Id);
            return View(Vaccination);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(VaccinationDefinition vaccDefinition)
        {
            ViewBag.CurrentPage = "Vaccination";
            if (vaccDefinition == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                //public string Code { get; set; }
                //public string Name { get; set; }
                //public virtual Age Age { get; set; }
                //public string Description { get; set; }
                //public string ICDCode { get; set; }
                //public decimal? Price { get; set; } = (decimal)0.00;
                var obj = db.VaccinationDefinitions.FirstOrDefault(x => x.Id == vaccDefinition.Id);
                obj.Code = vaccDefinition.Code;
                obj.Name = vaccDefinition.Name;
                obj.Age = db.Ages.FirstOrDefault(x=>x.Code == vaccDefinition.Age.Code);
                obj.Description = vaccDefinition.Description;
                obj.ICDCode = vaccDefinition.ICDCode;
                obj.Price = vaccDefinition.Price;
                //db.Entry(vaccDefinition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vaccDefinition);
            //var VaccinationToUpdate = db.VaccinationDefinitions.Find(id);
            //if (TryUpdateModel(VaccinationToUpdate, "",
            //   new string[] { "Title", "Credits", "DepartmentID" }))
            //{
            //    try
            //    {
            //        db.Entry(VaccinationToUpdate).State = EntityState.Modified;
            //        db.SaveChanges();

            //        return RedirectToAction("Index");
            //    }
            //    catch (RetryLimitExceededException /* dex */)
            //    {
            //        //Log the error (uncomment dex variable name and add a line here to write a log.
            //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            //    }
            //}
            //PopulateVaccinationDefinitionDropDownList(VaccinationToUpdate.Id);
            //return View(VaccinationToUpdate);
        }

        private void PopulateVaccinationDefinitionDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.VaccinationDefinitions
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }


        // GET: Vaccination/Delete/5
        public ActionResult Delete(Guid? id)
        {
            ViewBag.CurrentPage = "Vaccination";
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

        // POST: Vaccination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VaccinationDefinition Vaccination = db.VaccinationDefinitions.Find(id);
            db.VaccinationDefinitions.Remove(Vaccination);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateVaccinationCredits()
        {
            ViewBag.CurrentPage = "Vaccination";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateVaccinationCredits(int? multiplier)
        {
            ViewBag.CurrentPage = "Vaccination";
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

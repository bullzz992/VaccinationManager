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

namespace VaccinationManager.Controllers
{
    public class ExtendedFeesController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        // GET: ExtendedFees
        public ActionResult Index()
        {
            return View(db.ExtendedFees.ToList());
        }

        // GET: ExtendedFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtendedFee extendedFee = db.ExtendedFees.Find(id);
            if (extendedFee == null)
            {
                return HttpNotFound();
            }
            return View(extendedFee);
        }

        // GET: ExtendedFees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExtendedFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeeId,FeeName,FeeDescription,Amount")] ExtendedFee extendedFee)
        {
            if (ModelState.IsValid)
            {
                db.ExtendedFees.Add(extendedFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(extendedFee);
        }

        // GET: ExtendedFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtendedFee extendedFee = db.ExtendedFees.Find(id);
            if (extendedFee == null)
            {
                return HttpNotFound();
            }
            return View(extendedFee);
        }

        // POST: ExtendedFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeeId,FeeName,FeeDescription,Amount")] ExtendedFee extendedFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(extendedFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(extendedFee);
        }

        // GET: ExtendedFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtendedFee extendedFee = db.ExtendedFees.Find(id);
            if (extendedFee == null)
            {
                return HttpNotFound();
            }
            return View(extendedFee);
        }

        // POST: ExtendedFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExtendedFee extendedFee = db.ExtendedFees.Find(id);
            db.ExtendedFees.Remove(extendedFee);
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

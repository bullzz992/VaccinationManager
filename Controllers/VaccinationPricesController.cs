using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using VaccinationManager.DAL;
using VaccinationManager.Models;

namespace VaccinationManager.Controllers
{
    public class VaccinationPricesController : Controller
    {
        private VaccinationContext db = new VaccinationContext();
        private DataAccessLayer.VaccincationPriceDal provider = new VaccincationPriceDal();

        // GET: VaccinationPrices
        public ActionResult Index()
        {
            provider.AddVaccinationPrice("", (Decimal) 0.0);
            provider.FindPriceByFaccinationId("");
            
            Dictionary<string, string> VaccDefString = new Dictionary<string, string>();

            List<VaccinationPrice> list = db.VaccinationPrices.ToList();
            list.RemoveAll(x => string.IsNullOrEmpty(x.VaccinationDefId));
            foreach (VaccinationPrice item in list)
            {
                string value = "";
                VaccinationDefinition def = db.VaccinationDefinitions.FirstOrDefault(x => x.Id.ToString() == item.VaccinationDefId);
                int i = def.Age.Code;
                Age age = db.Ages.Find(i);
                value += def.Code + " - " + def.Name + " - " + age.Description;
                VaccDefString.Add(item.VaccinationDefId, value);
            }
            @ViewBag.DefDescription = VaccDefString;

            List<VaccinationPrice> finalList = db.VaccinationPrices.ToList();
            finalList.RemoveAll(x => string.IsNullOrEmpty(x.VaccinationDefId));
            return View(finalList);
        }

        // GET: VaccinationPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaccinationPrice vaccinationPrice = db.VaccinationPrices.Find(id);
            if (vaccinationPrice == null)
            {
                return HttpNotFound();
            }
            return View(vaccinationPrice);
        }

        // GET: VaccinationPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VaccinationPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VaccinationPriceId,VaccinationDefId,PriceAmount")] VaccinationPrice vaccinationPrice)
        {
            if (ModelState.IsValid)
            {
                db.VaccinationPrices.Add(vaccinationPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vaccinationPrice);
        }

        // GET: VaccinationPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaccinationPrice vaccinationPrice = db.VaccinationPrices.Find(id);
            if (vaccinationPrice == null)
            {
                return HttpNotFound();
            }

            string value = "";
            VaccinationDefinition def = db.VaccinationDefinitions.FirstOrDefault(x => x.Id.ToString() == vaccinationPrice.VaccinationDefId);
            int i = def.Age.Code;
            Age age = db.Ages.Find(i);
            value += def.Code + " - " + def.Name + " - " + age.Description;
            @ViewBag.EditDescription = value;
            return View(vaccinationPrice);
        }

        // POST: VaccinationPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VaccinationDefId,PriceAmount")] VaccinationPrice vaccinationPrice)
        {
            if (ModelState.IsValid)
            {
                provider.AddVaccinationPrice(vaccinationPrice.VaccinationDefId,  vaccinationPrice.PriceAmount);
                
                return RedirectToAction("Index");
            }
            return View(vaccinationPrice);
        }

        // GET: VaccinationPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaccinationPrice vaccinationPrice = db.VaccinationPrices.Find(id);
            if (vaccinationPrice == null)
            {
                return HttpNotFound();
            }
            return View(vaccinationPrice);
        }

        // POST: VaccinationPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VaccinationPrice vaccinationPrice = db.VaccinationPrices.Find(id);
            db.VaccinationPrices.Remove(vaccinationPrice);
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

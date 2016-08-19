using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VaccinationManager.DAL;
using VaccinationManager.Miscellaneous;
using VaccinationManager.Models;

namespace VaccinationManager.Controllers
{
    public class SMSConfigsController : Controller
    {
        private VaccinationContext db = new VaccinationContext();
        private HelpAPI helperObj = new HelpAPI(); 
        // GET: SMSConfigs
        public ActionResult Index()
        {
            return View(db.SmsConfig.ToList());
        }

        // GET: SMSConfigs/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSConfig sMSConfig = db.SmsConfig.Find(id);
            if (sMSConfig == null)
            {
                return HttpNotFound();
            }
            return View(sMSConfig);
        }

        // GET: SMSConfigs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SMSConfigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,PasswordHash")] SMSConfig sMSConfig)
        {
            if (ModelState.IsValid)
            {
                db.SmsConfig.Add(sMSConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sMSConfig);
        }

        // GET: SMSConfigs/Edit/5
        public ActionResult Edit(int? id)
        {
            var firstOrDefault = db.SmsConfig.FirstOrDefault();
            if (firstOrDefault != null) id = firstOrDefault.Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSConfig sMSConfig = db.SmsConfig.Find(id);
            if (sMSConfig == null)
            {
                return HttpNotFound();
            }
            sMSConfig.PasswordHash = helperObj.DecryptPassword(sMSConfig.PasswordHash);
            return View(sMSConfig);
        }

        // POST: SMSConfigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,PasswordHash")] SMSConfig sMSConfig)
        {
            if (ModelState.IsValid)
            {
                sMSConfig.PasswordHash = helperObj.EncryptPassword(sMSConfig.PasswordHash);
                db.Entry(sMSConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(sMSConfig);
        }

        // GET: SMSConfigs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSConfig sMSConfig = db.SmsConfig.Find(id);
            if (sMSConfig == null)
            {
                return HttpNotFound();
            }
            return View(sMSConfig);
        }

        // POST: SMSConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SMSConfig sMSConfig = db.SmsConfig.Find(id);
            db.SmsConfig.Remove(sMSConfig);
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

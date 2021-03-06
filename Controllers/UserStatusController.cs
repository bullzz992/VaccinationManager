﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VaccinationManager.CommunicationAPI;
using VaccinationManager.DAL;
using VaccinationManager.Models;

namespace VaccinationManager.Controllers
{
    public class UserStatusController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        // GET: UserStatus
        public ActionResult Index()
        {
            return View(db.UserStatus.ToList());
        }

        // GET: UserStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatus userStatus = db.UserStatus.Find(id);
            if (userStatus == null)
            {
                return HttpNotFound();
            }
            return View(userStatus);
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        

        public async Task<ActionResult> ResetUserPassword(string id)
        {
            

            var user = UserManager.FindById(id);
            string token = UserManager.GeneratePasswordResetToken(id);
            var result = await UserManager.ResetPasswordAsync(user.Id, token, "Password01");
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "UserStatus");
            }

            return RedirectToAction("Index", "UserStatus");
        }



       

        // GET: UserStatus/Create
        public ActionResult Create()
        {
            List<SelectListItem> obj = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = item.Value = "New";
            obj.Add(item);

            SelectListItem item2 = new SelectListItem();
            item2.Text = item2.Value = "Active";
            obj.Add(item2);

            SelectListItem item3 = new SelectListItem();
            item3.Text = item3.Value = "Inactive";
            obj.Add(item3);

            ViewBag.DopSelectList = obj;

            return View();
        }

        // POST: UserStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Username,Email,Branch_Practice_No,Status")] UserStatus userStatus)
        {
            if (ModelState.IsValid)
            {
                db.UserStatus.Add(userStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userStatus);
        }

        // GET: UserStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatus userStatus = db.UserStatus.Find(id);
            if (userStatus == null)
            {
                return HttpNotFound();
            }
            return View(userStatus);
        }

        // POST: UserStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Username,Email,Branch_Practice_No,Status")] UserStatus userStatus)
        {
            db = new VaccinationContext();
            if (ModelState.IsValid)
            {
                
                string prev = "";
                using (var tmpDb = new VaccinationContext())
                {
                    var firstOrDefault = tmpDb.UserStatus.FirstOrDefault(x => x.Username == userStatus.Username);
                    if (firstOrDefault != null)
                    {
                        prev = firstOrDefault.Status;
                    }
                }

                db.Entry(userStatus).State = EntityState.Modified;
                db.SaveChanges();

                using (var appdb = new ApplicationDbContext())
                {
                    var item = appdb.Users.FirstOrDefault(x => x.UserName == userStatus.Username);
                    item.Email = userStatus.Email;

                    appdb.SaveChanges();
                }

                if (userStatus.Status == "Active" && prev == "New")
                    {
                        MailService mail = new MailService("vaccination@hileya.co.za", "V@@5hY9a");
                        mail.SendMail(userStatus.Email, "Access Granted: Confirmation of Access to Vaccination Manager", "<html><body><h3>Hi " + userStatus.Username +"</h3>" + "<br/><p>This message is to confirm an Approval by Admin for your access request on Vaccination Manager.<br/>You will now be able to log onto the system. For more information you can contact your branch manager or Admin.</p><br/><p>Regards</p><b><p>Vaccination Manager Team</p></b></body></html>");

                    }
                
                return RedirectToAction("Index");
            }
            return View(userStatus);
        }

        // GET: UserStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatus userStatus = db.UserStatus.Find(id);
            if (userStatus == null)
            {
                return HttpNotFound();
            }
            return View(userStatus);
        }

        // POST: UserStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserStatus userStatus = db.UserStatus.Find(id);

            if (User.Identity.Name == "admin01")
            {
                using (var appdb = new ApplicationDbContext())
                {
                    var item     = appdb.Users.FirstOrDefault(x => x.UserName != "admin01" && x.UserName == userStatus.Username);
                    appdb.Users.Remove(item);

                    appdb.SaveChanges();
                }
            }

            db.UserStatus.Remove(userStatus);
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

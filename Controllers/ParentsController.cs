using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using VaccinationManager.DAL;
using VaccinationManager.Models;
using VaccinationManager.ViewModels;

namespace VaccinationManager.Controllers
{
    public class ParentsController : Controller
    {
        private VaccinationContext db = new VaccinationContext();

        // GET: Parents
        [Authorize]
        public ActionResult Index(string filter, string searchString)
        {
            ViewBag.CurrentPage = "Parent";
            var filters = new List<string>() { "ID Number", "Surname", "Name" };
            string loggedBranch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;

            List<Parent> parents = db.Parents.ToList();
            List<Parent> finalParents = new List<Parent>();
            if (loggedBranch != "ADMIN2010")
            {
                //string loggedOnBranch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;

                foreach (Parent item in parents)
                {
                    
                    if (item.Branch == loggedBranch)
                    {
                        finalParents.Add(item);
                    }
                }
            }
            parents = finalParents;
            ViewBag.filter = new SelectList(filters);
            

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (filter)
                {
                    case "ID Number":
                        parents = parents.Where(s => s.IdNumber.Contains(searchString)).ToList();
                        break;
                    case "Surname":
                        parents = parents.Where(s => s.Surname.Contains(searchString)).ToList();
                        break;
                    case "Name":
                        parents = parents.Where(s => s.Name.Contains(searchString)).ToList();
                        break;
                }
            }

            return View(parents);
        }

        // GET: Parents/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.CurrentPage = "Parent";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent pr = (Parent)db.Parents.Find(id);
            ParentViewModel parent = new ParentViewModel()
            {
                Address = pr.Address,
                Alergies = pr.Alergies,
                BloodType = pr.BloodType,
                Cellphone = pr.Cellphone,
                Email = pr.Email,
                Found = pr.Found,
                IdNumber = pr.IdNumber,
                Name = pr.Name,
                Surname = pr.Surname,
                Telephone = pr.Telephone
            };

            List<Child> children = db.Children.Where(c => c.MotherId == parent.IdNumber || c.FatherId == parent.IdNumber).ToList();
            parent.Children = children;
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Parent";
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNumber,Surname,Name,Telephone,Cellphone,Email,BloodType")] Parent parent)
        {
            ViewBag.CurrentPage = "Parent";
            if (ModelState.IsValid)
            {
                string loggedOnBranch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;
                parent.Branch = loggedOnBranch;
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parent);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.CurrentPage = "Parent";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNumber,Surname,Name,Telephone,Cellphone,Email,BloodType")] Parent parent)
        {
            ViewBag.CurrentPage = "Parent";
            if (ModelState.IsValid)
            {
                db.Entry(parent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.CurrentPage = "Parent";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewBag.CurrentPage = "Parent";
            Parent parent = db.Parents.Find(id);
            List<Child> children = db.Children.Where(c => c.MotherId == id || c.FatherId == id).ToList();
            foreach(Child child in children)
            {
                var vaccines = db.Vaccinations.Where(v => v.IdNumber == child.IdNumber);
                db.Vaccinations.RemoveRange(vaccines);
                db.Children.Remove(child);
            }
            db.Parents.Remove(parent);
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

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VaccinationManager.DAL;
using VaccinationManager.Models;
using VaccinationManager.PDF_Generator;
using VaccinationManager.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VaccinationManager.Services;
using System.Web.SessionState;
using VaccinationManager.ViewModels;
using PdfSharp.Pdf;

namespace VaccinationManager.Controllers
{
    public class ChildrenController : Controller
    {
        private VaccinationContext db = new VaccinationContext();
        private static ChildService _childService = new ChildService();
        //private static Child temp;
        private static List<ChildVaccination> Vaccinations { get; set; }

        // GET: Children
        [Authorize]
        public ActionResult Index(string filter, string searchString)
        {
            ViewBag.CurrentPage = "Children";
            var filters = new List<string>() { "ID Number", "Surname", "Name" };

            ViewBag.filter = new SelectList(filters);

            var children = from m in db.Children
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (filter)
                {
                    case "ID Number":
                        children = children.Where(s => s.IdNumber.Contains(searchString));
                        break;
                    case "Surname":
                        children = children.Where(s => s.Surname.Contains(searchString));
                        break;
                    case "Name":
                        children = children.Where(s => s.Name.Contains(searchString));
                        break;
                }

            }

            return View(children);
        }

        // GET: Children/Details/5
        [HttpGet]
        public ActionResult Details(string id)
        {
            ViewBag.CurrentPage = "Children";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            List<ChildMeasurement> measures = db.ChildMeasurements.Where(m => m.ChildID == id).OrderByDescending(c => c.Id).ToList();
            ChildMeasurement measure = measures.FirstOrDefault();
            child.HeadCircumference = (float)measure.HeadCircumference;
            child.Height = (float)measure.Height;
            child.Weight = (float)measure.Weight;
            //HttpSessionState sessionId = (HttpSessionState)HttpContext.Items["AspSession"];
            _childService.SetChildForSession(User.Identity.Name, child);
            //temp = child;
            child.Father = db.Parents.Where(p => p.IdNumber == child.FatherId).FirstOrDefault();
            child.Mother = db.Parents.Where(p => p.IdNumber == child.MotherId).FirstOrDefault();

            List<VaccinationDefinition> VaccinationDefs = db.VaccinationDefinitions.
                                            OrderBy(v => v.Age.Code).
                                            ToList();
            List<Vaccination> childVaccinations = db.Vaccinations.
                                            Where(v => v.IdNumber == child.IdNumber).
                                            ToList();

            Vaccinations = new List<ChildVaccination>();
            Vaccination lastVaccination = db.Vaccinations.OrderBy(v => v.Date).FirstOrDefault();
            if (lastVaccination != null)
            {
                DateTime lastVaccinationDate = lastVaccination.Date;
                foreach (var vaccine in VaccinationDefs)
                {
                    ChildVaccination cv = new ChildVaccination()
                    {
                        Age = vaccine.Age,
                        Code = vaccine.Code,
                        Description = vaccine.Description,
                        Name = vaccine.Name,
                        IdNumber = child.IdNumber,
                        Id = vaccine.Id,
                    };

                    Vaccination Vaccination = childVaccinations != null ? childVaccinations.Where(v => v.VaccinationDefinitionId == vaccine.Id).FirstOrDefault() : null;

                    if (Vaccination != null)
                    {
                        cv.Vaccinated = true;
                        cv.DateVaccinated = Vaccination.Date;
                        cv.Existing = true;
                        cv.Administrator = Vaccination.Administrator;
                        cv.SerialNumber = Vaccination.SerialNumber;
                        cv.Signature = Vaccination.Signature;
                    }
                    else
                    {
                        cv.DueDate = lastVaccinationDate.AddDays(vaccine.Age.Code * 7);
                    }

                    Vaccinations.Add(cv);
                }
            }
            child.VaccinationDetails = Vaccinations;
            var measurements = from cust in db.ChildMeasurements
                               where cust.ChildID == child.IdNumber
                               select cust;
            ViewBag.measurementsList = measurements.ToList();
            return View(child);
        }

        [HttpPost]
        public ActionResult Details(Child child)
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                if (child == null || string.IsNullOrEmpty(child.IdNumber))
                    child = _childService.GetChildForSession(User.Identity.Name);

                VaccinationReport report = new VaccinationReport();
                var usr = User;
                string branch = db.Branches.Find(User.Identity.Name).Branch;

                Document document = report.CreateDocument(Vaccinations, child, branch);
                //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                renderer.Document = document;

                renderer.RenderDocument();

                using (MemoryStream stream = new MemoryStream())
                {
                    renderer.PdfDocument.Save(stream, false);
                    return File(stream.ToArray(), "application/pdf");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            //eturn RedirectToAction("Index");
        }


        public ActionResult FullDetails(Child child)
        {
            ViewBag.CurrentPage = "Children";
            return View(child);
        }

        // GET: Children/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        //public ActionResult Create([Bind(Include = "IdNumber,Surname,Name,BloodType,FatherId,MotherId")] Child child)
        public ActionResult Create(Child child)
        {
            ViewBag.CurrentPage = "Children";
            if (ModelState.IsValid)
            {
                db.Children.Add(child);
                db.SaveChanges();

                if (!string.IsNullOrEmpty(child.MotherId))
                {
                    child.Mother = FindParent(child.MotherId);
                }

                if (!string.IsNullOrEmpty(child.FatherId))
                {
                    child.Father = FindParent(child.FatherId);
                }
                _childService.SetChildForSession(User.Identity.Name, child);
                return RedirectToAction("AddMeasurements");
            }
            return View(child);
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult AddMeasurements()
        {
            return View();
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddMeasurements(ChildMeasurement measure)
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                Child child = _childService.GetChildForSession(User.Identity.Name);
                measure.ChildID = child.IdNumber;
                measure.Created = DateTime.Now;

                db.ChildMeasurements.Add(measure);
                db.SaveChanges();

                return RedirectToAction("AddParent", child);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult CreateMeasurement(string id)
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                //string sessionId = (string)HttpContext.Items["AspSession"];
                Child child = _childService.GetChildForSession(User.Identity.Name);
                if (child == null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    child = db.Children.Find(id);
                    if (child == null)
                    {
                        return HttpNotFound();
                    }
                    _childService.SetChildForSession(User.Identity.Name, child);
                }
             
                List<ChildMeasurement> measures = db.ChildMeasurements.Where(m => m.ChildID == id).ToList();
                MeasurementViewModel measureVM = new MeasurementViewModel(measures);

                return View(measureVM);
                //if (child == null) child = (Child)Session["User"];
                //var sessionId = HttpContext.Items["AspSession"];
                //return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateMeasurement(MeasurementViewModel measure)
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                //string sessionId = (string)HttpContext.Items["AspSession"];
                Child child = _childService.GetChildForSession(User.Identity.Name);

                measure.CaptureMeasurement.ChildID = child.IdNumber;
                measure.CaptureMeasurement.Created = DateTime.Now;

                db.ChildMeasurements.Add(measure.CaptureMeasurement);
                db.SaveChanges();

                return CreateMeasurement(child.IdNumber);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        private Parent FindParent(string idNumber)
        {
            ViewBag.CurrentPage = "Children";
            if (!IdUtils.IsValidIdNumber(idNumber))
            {
                return null;
            }
            Parent parent = db.Parents.Where(p => p.IdNumber == idNumber).FirstOrDefault();
            if (parent != null)
                parent.Found = true;

            return parent;
        }

        [HttpGet]
        public ActionResult Measurements(string childId)
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                if (string.IsNullOrEmpty(childId))
                {
                    Child child = _childService.GetChildForSession(User.Identity.Name);
                    childId = child.IdNumber;
                }
                List<ChildMeasurement> measures = db.ChildMeasurements.Where(m => m.ChildID == childId).ToList();
                MeasurementViewModel measureVM = new MeasurementViewModel(measures);

                return View(measureVM);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Measurements()
        {
            ViewBag.CurrentPage = "Children";
            string chart = _childService.GetMeasurementChartForSession(User.Identity.Name);

            VaccinationReport report = new VaccinationReport();
            var usr = User;

            PdfDocument document = report.CreateDocumentWithChart(chart);
            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.PdfDocument = document;

            //already pdf
            //renderer.RenderDocument();

            using (MemoryStream stream = new MemoryStream())
            {
                renderer.PdfDocument.Save(stream, false);
                return File(stream.ToArray(), "application/pdf");
            }

        }

        [HttpPost]
        [ActionName("MeasurementChart")]
        public JsonResult MeasurementChartAjax(string data)
        {
            ViewBag.CurrentPage = "Children";
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            input = input.Split(',')[1];
            _childService.SetMeasurementChart(User.Identity.Name, input);
            Debug.WriteLine("Image Data {0}", data);
            return new JsonResult() { Data = "SUCCESS" };
            //return View();
        }

        // GET: Children/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.CurrentPage = "Children";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Child child)
        {
            ViewBag.CurrentPage = "Children";
            if (ModelState.IsValid)
            {
                db.Entry(child).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(child);
        }

        // GET: Children/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.CurrentPage = "Children";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewBag.CurrentPage = "Children";
            Child child = db.Children.Find(id);

            var vaccines = db.Vaccinations.Where(v => v.IdNumber == child.IdNumber);
            db.Vaccinations.RemoveRange(vaccines);
            db.Children.Remove(child);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult AddParent()
        {
            ViewBag.CurrentPage = "Children";
            try
            {
                //string sessionId = (string)HttpContext.Items["AspSession"];
                Child child = _childService.GetChildForSession(User.Identity.Name);

                return View(child);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Children/AddParent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddParent(Child child)
        {
            ViewBag.CurrentPage = "Children";
            if (child.Mother != null && !child.Mother.Found)
            {
                child.Mother.IdNumber = child.MotherId;
                db.Parents.Add(child.Mother);
                db.SaveChanges();
            }

            if (child.Father != null && !child.Father.Found)
            {
                child.Father.IdNumber = child.FatherId;
                db.Parents.Add(child.Father);
                db.SaveChanges();
            }

            return RedirectToAction("ChildVaccinations", child);
        }

        [HttpGet]
        public ActionResult ChildVaccinations(Child child)
        {
            ViewBag.CurrentPage = "Children";
            List<VaccinationDefinition> VaccinationDefs = db.VaccinationDefinitions.
                                                        OrderBy(v => v.Age.Code).
                                                        ToList();
            List<Vaccination> childVaccinations = db.Vaccinations.
                                            Where(v => v.IdNumber == child.IdNumber).
                                            ToList();

            List<ChildVaccination> Vaccinations = new List<ChildVaccination>();
            foreach (var vaccine in VaccinationDefs)
            {
                ChildVaccination cv = new ChildVaccination()
                {
                    Age = vaccine.Age,
                    Code = vaccine.Code,
                    Description = vaccine.Description,
                    Name = vaccine.Name,
                    IdNumber = child.IdNumber,
                    Id = vaccine.Id,
                };

                Vaccination Vaccination = childVaccinations != null ? childVaccinations.Where(v => v.VaccinationDefinitionId == vaccine.Id).FirstOrDefault() : null;

                if (Vaccination != null)
                {
                    cv.Vaccinated = true;
                    cv.DateVaccinated = Vaccination.Date;
                    cv.Existing = true;
                }
                else
                {
                    cv.DateVaccinated = null;
                }
                
                Vaccinations.Add(cv);
            }

            //VaccinationContainer vaxC = new VaccinationContainer()
            //{
            //    IdNumber = child.IdNumber,
            //    Vaccinations = Vaccinations
            //};

            

            return View(Vaccinations);
        }

        [HttpPost]
        public ActionResult ChildVaccinations(List<ChildVaccination> Vaccinations)
        {
            ViewBag.CurrentPage = "Children";
            var vaccinations = ViewBag.Model;
            List<Vaccination> childVaccinations = new List<Vaccination>();
            foreach (ChildVaccination cVaccine in Vaccinations)
            {
                if (!cVaccine.Existing && cVaccine.Vaccinated)
                {
                    db.Vaccinations.Add(new Vaccination()
                        {
                            Date = DateTime.Now,
                            Id = Guid.NewGuid(),//cVax.Id,
                            IdNumber = cVaccine.IdNumber,
                            VaccinationDefinitionId = cVaccine.Id,
                            Administrator = User.Identity.Name,
                            SerialNumber = cVaccine.SerialNumber,
                            Signature = cVaccine.Signature
                        });
                }
            }
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

        //private Child GetChildWithID()
        //{
        //    Child child = _childService.GetChildForSession(User.Identity.Name);
        //    if (child == null)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        child = db.Children.Find(id);
        //        if (child == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        _childService.SetChildForSession(User.Identity.Name, child);
        //    }

        //    return
        //}
    }
}

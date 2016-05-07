using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using VaccinationManager.DAL;
using VaccinationManager.Models;
using VaccinationManager.PDF_Generator;

namespace VaccinationManager.Controllers
{
    public class InvoiceController : Controller
    {
        VaccinationContext db = new VaccinationContext();
        // GET: Invoice
        [Authorize]
        public ActionResult Index(string filter, string searchString)
        {
            ViewBag.CurrentPage = "Children";
            var filters = new List<string>() { "ID Number", "Surname", "Name" };

            ViewBag.filter = new SelectList(filters);

            string loggedBranch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;
            IQueryable<Child> children = null;
            if (loggedBranch == "ADMIN2010")
            {
                children = from m in db.Children
                           select m;
            }
            else
            {
                children = from m in db.Children
                           where m.Branch == loggedBranch
                           select m;
            }

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

        // GET: Invoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Invoice/Create
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                id = globalID;
            }
            Invoice objInvoice = new Invoice();

            objInvoice.PatientChild = db.Children.FirstOrDefault(x => x.IdNumber == id);
            objInvoice.VaccinationList = db.Vaccinations.Where(x => x.IdNumber == id).ToList();
            string branch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;
            var  temp = new List<ExtendedFeesCustom>();
            List<ExtendedFee> listFromDB = null;
            if (branch == "ADMIN2010")
            {
                listFromDB = db.ExtendedFees.ToList();
            }
            else
            {
                listFromDB = db.ExtendedFees.Where(x => x.Branch == branch).ToList();
            }

            foreach (ExtendedFee item in listFromDB)
            {
                temp.Add(FeesToCustomObj(item));
            }

            objInvoice.ExtendedFeeList = temp;

            objInvoice.InvoiceFromDate = DateTime.Now.Date;
            objInvoice.BranchInformation = db.Branches1.FirstOrDefault(x => x.Practice_No == branch);
                

            return View(objInvoice);
        }

        public ExtendedFeesCustom FeesToCustomObj(ExtendedFee input)
        {
            ExtendedFeesCustom outputCustom = new ExtendedFeesCustom();
            outputCustom.FeeId = input.FeeId;
            outputCustom.FeeName = input.FeeName;
            outputCustom.FeeDescription = input.FeeDescription;
            outputCustom.Amount = input.Amount;
            outputCustom.Branch = input.Branch;
            outputCustom.IsChecked = "Exclude";

            return outputCustom;
            
        }
        //[Bind(Include = "PatientChild,VaccinationList,ExtendedFeeList,BranchInformation,InvoiceFromDate,InvoiceToDate")] 
        // POST: Invoice/Create
        FileContentResult resultPDF = null;
        [HttpPost]
        public ActionResult Create(Invoice model)
        {
            Session["DateError"] = "";
            string tempId = Url.RequestContext.RouteData.Values["id"].ToString();
            if (string.IsNullOrEmpty(tempId))
            {
                
            }
            else
            {
                Session["GlobalID"] = tempId;
            }
            try
            {
                //var obj = (List<ExtendedFeesCustom>)Request["Model.ExtendedFeeList"];
                //string test = obj[0].FeeName;
                var obj2 = Request["VaccinationDate"];

                if (!isValidDate(obj2))
                {
                    Session["DateError"] = "Invalid";

                    return View(model);
                    //return RedirectToAction("Create", "Invoice", new { id = Session["GlobalID"] });
                }
                Session["DateError"] = "";
                ViewBag.GlobalDate = outputDate;
                resultPDF = CreateDummyReport();
                return RedirectToAction("Edit", new {id = tempId});
            }
            catch(Exception ex)
            {
                return View(model);
            }
        }

        private static DateTime outputDate;
        private bool isValidDate(string input)
        {
            string[] date = input.Split('/');
            if (date.Length != 3)
            {
                return false;
            }

            int day, month, year;
            bool IsDayValid, isMonthValid, isYearValid;

            IsDayValid = Int32.TryParse(date[0], out day);
            isMonthValid = Int32.TryParse(date[1], out month);
            isYearValid = Int32.TryParse(date[2], out year);

            if (!IsDayValid || !isMonthValid || !isYearValid)
            {
                return false;
            }



            try
            {
                outputDate = new DateTime(year, month, day);
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private string globalID;
        private FileContentResult CreateDummyReport()
        {
            //-------------------------------------------
            //outputDate = ViewBag.GlobalDate;
            string id = Url.RequestContext.RouteData.Values["id"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = invoiceID;
            }
            Invoice objInvoice = new Invoice();

            objInvoice.PatientChild = db.Children.FirstOrDefault(x => x.IdNumber == id);
            objInvoice.VaccinationList = db.Vaccinations.Where(x => x.IdNumber == id && x.Date.Day == outputDate.Day && x.Date.Month == outputDate.Month && x.Date.Year == outputDate.Year).ToList();
            string branch = db.UserStatus.FirstOrDefault(x => x.Username == User.Identity.Name).Branch_Practice_No;
            var temp = new List<ExtendedFeesCustom>();
            List<ExtendedFee> listFromDB = null;
            if (branch == "ADMIN2010")
            {
                listFromDB = db.ExtendedFees.ToList();
            }
            else
            {
                listFromDB = db.ExtendedFees.Where(x => x.Branch == branch).ToList();
            }

            foreach (ExtendedFee item in listFromDB)
            {
                temp.Add(FeesToCustomObj(item));
            }

            objInvoice.ExtendedFeeList = temp;
            objInvoice.InvoiceFromDate = outputDate;
            objInvoice.InvoiceToDate = outputDate;


            objInvoice.BranchInformation = db.Branches1.FirstOrDefault(x => x.Practice_No == branch);

            //-------------------------------------------


            InvoiceReport report = new InvoiceReport();


            Document document = report.CreateDocument(objInvoice, User.Identity.Name);
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

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            return View();
        }

        private static string invoiceID;
        // POST: Invoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                invoiceID = Request["id"];
                return CreateDummyReport();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoice/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

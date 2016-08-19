using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationManager.Models
{
    public class Invoice
    {
        public Child PatientChild;
        public List<Vaccination> VaccinationList;
        public List<ExtendedFeesCustom> ExtendedFeeList;
        public Branch BranchInformation;

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceFromDate { get; set; }
        public DateTime InvoiceToDate;

    }
}

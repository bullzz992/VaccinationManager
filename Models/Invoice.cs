using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationManager.Models
{
    public class Invoice
    {
        public Child PatientChild;
        public List<Vaccination> VaccinationList;
        public List<ExtendedFee> ExtenddFeeList;
        public Branch BranchInformation;
        public DateTime InvoiceDate;

    }
}

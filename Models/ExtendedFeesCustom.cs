using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class ExtendedFeesCustom
    {
        [Key]
        public int FeeId { get; set; }
        public string FeeName { get; set; }
        public string FeeDescription { get; set; }
        public decimal Amount { get; set; }
        public string Branch { get; set; }
        public string IsChecked { get; set; }
    }
}
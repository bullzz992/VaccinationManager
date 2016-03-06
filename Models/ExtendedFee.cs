using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationManager.Models
{
    public class ExtendedFee
    {
        [Key]
        public int FeeId { get; set; }
        public string FeeName { get; set; }
        public string FeeDescription { get; set; }
        public decimal Amount { get; set; }
    }
}

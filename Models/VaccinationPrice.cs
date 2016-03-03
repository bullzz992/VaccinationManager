using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationManager.Models
{
    public class VaccinationPrice
    {
        [Key]
        public int VaccinationPriceId { get; set; }

        public string VaccinationDefId { get; set; }

        public decimal PriceAmount { get; set; }
    }
}

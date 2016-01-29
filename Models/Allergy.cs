using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class Allergy
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
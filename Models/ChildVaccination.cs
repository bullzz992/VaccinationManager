using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class ChildVaccination : VaccinationDefinition
    {
        [NotMapped]
        public bool Vaccinated { get; set; }
        [NotMapped]
        public DateTime? DateVaccinated { get; set; }
        [NotMapped]
        public DateTime? DueDate { get; set; }
        [NotMapped]
        public bool Existing { get; set; }
        //Should be using the VaccinationContainer but ._.
        [NotMapped]
        public string IdNumber { get; set; }
        [NotMapped]
        public string Administrator { get; set; }
        [NotMapped]
        public string SerialNumber { get; set; }
        [NotMapped]
        public string Signature { get; set; }
    }
}
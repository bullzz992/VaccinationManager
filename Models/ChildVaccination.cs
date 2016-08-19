using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateVaccinated { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        [NotMapped]
        public bool Existing { get; set; }
        //Should be using the VaccinationContainer but ._.
        [NotMapped]
        public string IdNumber { get; set; }
        [NotMapped]
        public string Administrator { get; set; }
        [NotMapped]
        [Required]
        public string SerialNumber { get; set; }
        [NotMapped]
        [Required]
        public string Signature { get; set; }
    }
}
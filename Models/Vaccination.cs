using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VaccinationManager.Enums;

namespace VaccinationManager.Models
{
    public class Vaccination
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VaccinationDefinitionId { get; set; }
        public string IdNumber { get; set; }
        public string Administrator { get; set; }
        public string SerialNumber { get; set; }
        public string Signature { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Vaccination Date")]
        public DateTime Date { get; set; }
    }
}        
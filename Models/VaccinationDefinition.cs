using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VaccinationManager.Enums;

namespace VaccinationManager.Models
{
    public class VaccinationDefinition
    {
        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual Age Age { get; set; }
        public string Description { get; set; }
        //public int? AgeId { get; set; }
        //[InverseProperty("Code")]
        //[ForeignKey("AgeId")]
        //public virtual Age Age { get; set; }
    }
}
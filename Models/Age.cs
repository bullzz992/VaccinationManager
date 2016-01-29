using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class Age
    {
        [Key]
        public int Code { get; set; }
        public string Description { get; set; }
        ////[ForeignKey("Id")]
        //public Guid VaccinationId { get; set; }

        ////[ForeignKey("VaccinationDefinition_Id")]
        //public virtual VaccinationDefinition Vaxine { get; set; }
    }
}
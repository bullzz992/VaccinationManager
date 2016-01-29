using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class ChildMeasurement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double HeadCircumference { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("Child")]
        public string ChildID { get; set; }

        //[Required]
        public virtual Child Child { get; set; }
    }
}
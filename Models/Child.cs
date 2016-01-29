using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VaccinationManager.Enums;

namespace VaccinationManager.Models
{
    public class Child
    {
        [Key]
        [Required]
        [StringLength(13)]
        [Display(Name = "Id Number")]
        public string IdNumber { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public float HeadCircumference { get; set; }

        public BloodType BloodType { get; set; }
        
        public Parent Father { get; set; }
        
        public Parent Mother { get; set; }

        public string MotherId { get; set; }

        public string FatherId { get; set; }
        
        public ICollection<Allergy> Allergies { get; set; }
        
        public ICollection<Vaccination> Vaccinations { get; set; }

        [NotMapped]
        public List<ChildVaccination> VaccinationDetails { get; set; }

        public virtual List<ChildMeasurement> Measurements { get; set; }

        public bool MotherFound()
        {

            return (Mother == null) ? false : Mother.Found;
        }

        public bool FatherFound()
        {
            return (Father == null) ? false : Father.Found;
        }
    }
}
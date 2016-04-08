using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VaccinationManager.Enums;

namespace VaccinationManager.Models
{
    public class Parent
    {
        [Key]
        public string IdNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public BloodType BloodType { get; set; }
        public List<Allergy> Alergies { get; set; }
        public string Branch { get; set; }

        [NotMapped]
        public bool Found { get; set; }
    }
}
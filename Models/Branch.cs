using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationManager.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Overseer_Name { get; set; }

        [Required]
        public string Overseer_Surname { get; set; }

        [Required]
        public string Practice_No { get; set; }

        [Required]
        public string Tel_Number { get; set; }

        [Required]
        public string Fax_Number { get; set; }

        [Required]
        public string Email_Address { get; set; }

        public string Bank_Name { get; set; }
        public string Branch_Number { get; set; }
        public string Account_Number { get; set; }

        [Required]
        public string Address { get; set; }
    }
}

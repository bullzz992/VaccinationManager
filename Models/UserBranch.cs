using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class UserBranch
    {
        [Key]
        public string UserName { get; set; }
        public string Branch { get; set; }
    }
}
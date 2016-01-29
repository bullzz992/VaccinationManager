using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class VaccinationContainer
    {
        public List<ChildVaccination> Vaccinations { get; set; }
        public string IdNumber { get; set; }
    }
}
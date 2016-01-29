using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VaccinationManager.Models;

namespace VaccinationManager.ViewModels
{
    public class ParentViewModel : Parent
    {
        public List<Child> Children { get; set; }
    }
}
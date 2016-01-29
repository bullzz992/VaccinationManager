using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VaccinationManager.Enums
{
    public enum ChildAge
    {
        [Description("At Birth")]
        Birth = 0,
        [Display(Name = "6 Weeks")]
        SixWeeks = 6,
        [Display(Name = "10 Weeks")]
        TenWeeks = 10,
        [Display(Name = "14 Weeks")]
        FourteenWeeks = 14,
        [Display(Name = "9 Months")]
        NineMonths = 36,
        [Display(Name = "12 Months")]
        TwelveMonths = 48,
        [Display(Name = "15 to 18 Months")]
        FifToEighteenMonths = 60,
        [Display(Name = "18 Months")]
        EighteenMonths = 72,
        [Display(Name = "6 Years")]
        SixYears = 288,
        [Display(Name = "12 Years")]
        TwelveYears = 576
    };
}
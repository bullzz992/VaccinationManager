using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccinationManager.DAL;

namespace VaccinationManager.Models
{
    public class ExtendedFee
    {
        [Key]
        public int FeeId { get; set; }
        public string FeeName { get; set; }
        public string FeeDescription { get; set; }
        public decimal Amount { get; set; }
        [BranchValidation(ErrorMessage = "This practice number doesn't exist")]
        public string Branch { get; set; }
        public string FeeCode { get; set; }
        public string NappiCode { get; set; }
        public string IncludeInReport { get; set; }
    }

    public class BranchValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            
            string branch = value.ToString();
            using (VaccinationContext db = new VaccinationContext())
            {
                if (db.Branches1.FirstOrDefault(x => x.Practice_No == branch) != null)
                {
                    return ValidationResult.Success;
                }
            }
            var errorMessage = FormatErrorMessage(context.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}

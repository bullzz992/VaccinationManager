using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using VaccinationManager.Models;
using VaccinationManager.Enums;

namespace VaccinationManager.DAL
{
    public class VaccinationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VaccinationContext>
    {
        protected override void Seed(VaccinationContext context)
        {
            var Vaccinations = new List<VaccinationDefinition>
            {
                new VaccinationDefinition{ Code="OPV", Id=Guid.NewGuid(), Name="Oral Polio Vaccine", Age = 
                    new Age() { Code = (int)ChildAge.Birth, Description = Enum.GetName(typeof(ChildAge), ChildAge.Birth) } },

                new VaccinationDefinition{ Code="BCG", Id=Guid.NewGuid(), Name="BCG Vaccine", Age = 
                   new Age() { Code = (int)ChildAge.Birth, Description = Enum.GetName(typeof(ChildAge), ChildAge.Birth) } },


                new VaccinationDefinition{ Code="Pentaxim", Id=Guid.NewGuid(), Name="Diphtheria, Tetanus, Accellular Pertussis, Haemophilus Influenzae type b and Inactivated Polio Vaccine", 
                                            Age = new Age() { Code = (int)ChildAge.SixWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.SixWeeks) } }, 
                new VaccinationDefinition{ Code="Pentaxim", Id=Guid.NewGuid(), Name="Diphtheria, Tetanus, Accellular Pertussis, Haemophilus Influenzae type b and Inactivated Polio Vaccine", 
                                            Age = new Age() { Code = (int)ChildAge.TenWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.TenWeeks) } },
                new VaccinationDefinition{ Code="Pentaxim", Id=Guid.NewGuid(), Name="Diphtheria, Tetanus, Accellular Pertussis, Haemophilus Influenzae type b and Inactivated Polio Vaccine", 
                                            Age = new Age() { Code = (int)ChildAge.EighteenMonths, Description = Enum.GetName(typeof(ChildAge), ChildAge.EighteenMonths) } } , 

                new VaccinationDefinition{ Code="Hep B", Id=Guid.NewGuid(), Name="Hepatitis B Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.SixWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.SixWeeks) } },
                new VaccinationDefinition{ Code="Hep B", Id=Guid.NewGuid(), Name="Hepatitis B Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.TenWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.TenWeeks) } },
                new VaccinationDefinition{ Code="Hep B", Id=Guid.NewGuid(), Name="Hepatitis B Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.EighteenMonths, Description = Enum.GetName(typeof(ChildAge), ChildAge.EighteenMonths) } } , 

                new VaccinationDefinition{ Code="RoteviX", Id=Guid.NewGuid(), Name="Rotavirus Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.SixWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.SixWeeks) } }, 
                new VaccinationDefinition{ Code="RoteviX", Id=Guid.NewGuid(), Name="Rotavirus Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.TenWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.TenWeeks) } },
                new VaccinationDefinition{ Code="RoteviX", Id=Guid.NewGuid(), Name="Rotavirus Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.EighteenMonths, Description = Enum.GetName(typeof(ChildAge), ChildAge.EighteenMonths) } } , 

                new VaccinationDefinition{ Code="Prev-B", Id=Guid.NewGuid(), Name="Pneumococcal Conjugated Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.SixWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.SixWeeks) } },
                new VaccinationDefinition{ Code="Prev-B", Id=Guid.NewGuid(), Name="Pneumococcal Conjugated Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.TenWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.TenWeeks) } },
                new VaccinationDefinition{ Code="Prev-B", Id=Guid.NewGuid(), Name="Pneumococcal Conjugated Vaccine",
                                            Age = new Age() { Code = (int)ChildAge.EighteenMonths, Description = Enum.GetName(typeof(ChildAge), ChildAge.EighteenMonths) } } , 
                //new VaccinationDefinition{ Code="Prev-B", Id=Guid.NewGuid(), Name="Pneumococcal Conjugated Vaccine",
                //                            Ages = new List<Age>(){
                //                            new Age() { Code = (int)ChildAge.SixWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.SixWeeks) } , 
                //                            new Age() { Code = (int)ChildAge.TenWeeks, Description = Enum.GetName(typeof(ChildAge), ChildAge.TenWeeks) } ,
                //                            new Age() { Code = (int)ChildAge.EighteenMonths, Description = Enum.GetName(typeof(ChildAge), ChildAge.EighteenMonths) } } }, 

            };

            Vaccinations.ForEach(s => context.VaccinationDefinitions.Add(s));
            context.SaveChanges();
        }
    }
}
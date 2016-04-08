//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityModelContainer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Child
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Child()
        {
            this.Allergies = new HashSet<Allergy>();
            this.ChildMeasurements = new HashSet<ChildMeasurement>();
            this.Vaccinations = new HashSet<Vaccination>();
        }
    
        public string IdNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float HeadCircumference { get; set; }
        public int BloodType { get; set; }
        public string MotherId { get; set; }
        public string FatherId { get; set; }
        public string ParentViewModel_IdNumber { get; set; }
        public string Father_IdNumber { get; set; }
        public string Mother_IdNumber { get; set; }
        public string Branch { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Allergy> Allergies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChildMeasurement> ChildMeasurements { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual Parent Parent1 { get; set; }
        public virtual Parent Parent2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vaccination> Vaccinations { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonMotion.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            this.address = new HashSet<address>();
            this.education = new HashSet<education>();
            this.familia = new HashSet<familia>();
            this.position = new HashSet<position>();
            this.workhistory = new HashSet<workhistory>();
        }
    
        public int ID { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string secondname { get; set; }
        public string sex { get; set; }
        public System.DateTime birthdate { get; set; }
        public string birthplace { get; set; }
        public Nullable<byte> familyStatusID { get; set; }
        public string passportSerial { get; set; }
        public string passportNumber { get; set; }
        public string FINCODE { get; set; }
        public Nullable<int> nationalityID { get; set; }
        public Nullable<int> partyID { get; set; }
        public Nullable<int> photoID { get; set; }
        public string workPhone { get; set; }
        public string mobilePhone { get; set; }
        public string emailaddr { get; set; }
        public Nullable<decimal> salary { get; set; }
        public Nullable<byte> militaryStatusID { get; set; }
        public Nullable<bool> isdeleted { get; set; }
        public Nullable<bool> isfired { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<address> address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<education> education { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<familia> familia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<position> position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workhistory> workhistory { get; set; }
    }
}

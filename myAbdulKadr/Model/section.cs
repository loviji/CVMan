//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace myAbdulKadr.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class section
    {
        public int ID { get; set; }
        public int departmentID { get; set; }
        public string sectionName { get; set; }
    
        public virtual department department { get; set; }
    }
}

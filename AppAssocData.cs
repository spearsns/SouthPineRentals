//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LandlordV3_MVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class AppAssocData
    {
        public System.Guid ID { get; set; }
        public System.Guid ReferenceID { get; set; }
        public System.Guid PersonalID { get; set; }
        public System.Guid RoleID { get; set; }
        public string Relationship { get; set; }
        public System.DateTime EntryTS { get; set; }
    
        public virtual AppAssocRole AppAssocRole { get; set; }
    }
}

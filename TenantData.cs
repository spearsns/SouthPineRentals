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
    
    public partial class TenantData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TenantData()
        {
            this.Tenant_Address = new HashSet<Tenant_Address>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid LeaseID { get; set; }
        public System.Guid ApplicationID { get; set; }
        public System.Guid EntryTS { get; set; }
        public bool Active { get; set; }
    
        public virtual ApplicationData ApplicationData { get; set; }
        public virtual LeaseData LeaseData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tenant_Address> Tenant_Address { get; set; }
    }
}

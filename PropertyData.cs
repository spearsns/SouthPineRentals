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
    
    public partial class PropertyData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyData()
        {
            this.ApplianceDatas = new HashSet<ApplianceData>();
            this.IncludedDatas = new HashSet<IncludedData>();
            this.LeaseDatas = new HashSet<LeaseData>();
            this.Property_Address = new HashSet<Property_Address>();
        }
    
        public System.Guid ID { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> Rent { get; set; }
        public byte Bedrooms { get; set; }
        public byte Bathrooms { get; set; }
        public Nullable<int> SquareFeet { get; set; }
        public Nullable<byte> Parking { get; set; }
        public Nullable<bool> CentralAC { get; set; }
        public Nullable<bool> Laundry { get; set; }
        public Nullable<bool> Dishwasher { get; set; }
        public Nullable<bool> GarbageDisposal { get; set; }
        public Nullable<bool> Fireplace { get; set; }
        public Nullable<bool> Satellite { get; set; }
        public Nullable<bool> StorageShed { get; set; }
        public Nullable<bool> AllowCats { get; set; }
        public Nullable<bool> AllowDogs { get; set; }
        public Nullable<System.Guid> NeighborhoodID { get; set; }
        public Nullable<System.Guid> HighID { get; set; }
        public Nullable<System.Guid> MiddleID { get; set; }
        public Nullable<System.Guid> ElementaryID { get; set; }
        public System.DateTime EntryTS { get; set; }
        public bool Vacant { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplianceData> ApplianceDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncludedData> IncludedDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaseData> LeaseDatas { get; set; }
        public virtual NeighborhoodData NeighborhoodData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property_Address> Property_Address { get; set; }
        public virtual SchoolData SchoolData { get; set; }
        public virtual SchoolData SchoolData1 { get; set; }
        public virtual SchoolData SchoolData2 { get; set; }
    }
}
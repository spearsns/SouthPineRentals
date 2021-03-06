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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LandlordV3Entities : DbContext
    {
        public LandlordV3Entities()
            : base("name=LandlordV3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddressData> AddressDatas { get; set; }
        public virtual DbSet<AppAssocData> AppAssocDatas { get; set; }
        public virtual DbSet<AppAssocRole> AppAssocRoles { get; set; }
        public virtual DbSet<ApplianceData> ApplianceDatas { get; set; }
        public virtual DbSet<ApplianceType> ApplianceTypes { get; set; }
        public virtual DbSet<ApplicationData> ApplicationDatas { get; set; }
        public virtual DbSet<Business_Address> Business_Address { get; set; }
        public virtual DbSet<BusinessAssocData> BusinessAssocDatas { get; set; }
        public virtual DbSet<BusinessData> BusinessDatas { get; set; }
        public virtual DbSet<IncludedData> IncludedDatas { get; set; }
        public virtual DbSet<LeaseData> LeaseDatas { get; set; }
        public virtual DbSet<LoginData> LoginDatas { get; set; }
        public virtual DbSet<LoginRole> LoginRoles { get; set; }
        public virtual DbSet<NeighborhoodData> NeighborhoodDatas { get; set; }
        public virtual DbSet<Personal_Address> Personal_Address { get; set; }
        public virtual DbSet<PersonalData> PersonalDatas { get; set; }
        public virtual DbSet<PetData> PetDatas { get; set; }
        public virtual DbSet<Property_Address> Property_Address { get; set; }
        public virtual DbSet<PropertyData> PropertyDatas { get; set; }
        public virtual DbSet<SchoolData> SchoolDatas { get; set; }
        public virtual DbSet<Tenant_Address> Tenant_Address { get; set; }
        public virtual DbSet<TenantData> TenantDatas { get; set; }
        public virtual DbSet<VehicleData> VehicleDatas { get; set; }
    }
}

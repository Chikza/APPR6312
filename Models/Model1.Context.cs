﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Part_2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DjPromoWebsiteEntities : DbContext
    {
        public DjPromoWebsiteEntities()
            : base("name=DjPromoWebsiteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AllocateGood> AllocateGoods { get; set; }
        public virtual DbSet<AllocateMoney> AllocateMoneys { get; set; }
        public virtual DbSet<Disaster> Disasters { get; set; }
        public virtual DbSet<GoodDonation> GoodDonations { get; set; }
        public virtual DbSet<MonetaryDonation> MonetaryDonations { get; set; }
        public virtual DbSet<PurchaseGood> PurchaseGoods { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
    }
}

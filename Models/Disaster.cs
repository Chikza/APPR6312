//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Disaster
    {
        public Disaster()
        {
            AllocateGoods = new HashSet<AllocateGood>();
            AllocateMoneys = new HashSet<AllocateMoney>();
            PurchaseGoods = new HashSet<PurchaseGood>();
        }
        public int DisasterID { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Start_date { get; set; }
        public Nullable<System.DateTime> End_date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Aid_types { get; set; }


        public virtual ICollection<AllocateGood> AllocateGoods { get; set; }
        public virtual ICollection<AllocateMoney> AllocateMoneys { get; set; }
        public virtual ICollection<PurchaseGood> PurchaseGoods { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}

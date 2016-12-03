//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRentalAPI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rent
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public int BookingId { get; set; }
        public System.DateTime DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public decimal InitialMileage { get; set; }
        public Nullable<decimal> FinalMileage { get; set; }
        public Nullable<decimal> FinalPrice { get; set; }
        public string FullName { get; set; }
        public string Phones { get; set; }
        public string InsuranceNumber { get; set; }
    
        public virtual Booking Booking { get; set; }
        public virtual Car Car { get; set; }
        public virtual Client Client { get; set; }
    }
}
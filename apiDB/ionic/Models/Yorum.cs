//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ionicFinalProje2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Yorum
    {
        public int yorumId { get; set; }
        public int yorumGonderiId { get; set; }
        public int yorumKullaniciId { get; set; }
        public string yorumIcerik { get; set; }
        public Nullable<System.DateTime> yorumTarih { get; set; }
    
        public virtual Gonderi Gonderi { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}

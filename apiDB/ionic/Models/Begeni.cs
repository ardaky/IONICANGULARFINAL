//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ionic.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Begeni
    {
        public int begeniId { get; set; }
        public int begeniKullaniciId { get; set; }
        public int begeniGonderiId { get; set; }
    
        public virtual Gonderi Gonderi { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
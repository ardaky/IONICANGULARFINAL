using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ionic.ViewModel
{
    public class YorumModel
    {
        public int yorumId { get; set; }
        public int yorumGonderiId { get; set; }
        public int yorumKullaniciId { get; set; }
        public string yorumIcerik { get; set; }
        public Nullable<System.DateTime> yorumTarih { get; set; }
        public KullaniciModel yorumKullaniciBilgi { get; set; }
    }
}
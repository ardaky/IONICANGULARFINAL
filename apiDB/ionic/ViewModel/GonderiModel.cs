using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ionicFinalProje2.ViewModel
{
    public class GonderiModel
    {
        public int gonderiId { get; set; }
        public int gonderiKullaniciId { get; set; }
        public string gonderiIcerik { get; set; }
        public Nullable<System.DateTime> gonderiTarih { get; set; }
        public int gonderiBegeniSayisi { get; set; }
        public KullaniciModel gonderiKullaniciBilgi { get; set; }
    }
}
using ionicFinalProje2.Models;
using ionicFinalProje2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ionicFinalProje2.Auth
{
    public class UyeService
    {
        Database2ionicEntities db = new Database2ionicEntities();

        public KullaniciModel UyeOturumAc(string kadi, string parola)
        {
            KullaniciModel uye = db.Kullanici.Where(s => s.kullaniciAdi == kadi && s.sifre == parola).Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                foto = x.foto,
                rol = x.rol,
                sifre = x.sifre
            }).SingleOrDefault();
            
            return uye;

        }
    }
}
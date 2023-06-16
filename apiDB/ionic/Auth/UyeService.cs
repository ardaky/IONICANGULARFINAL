using ionic.Models;
using ionic.ViewModel;
using System.Linq;

namespace ionic.Auth
{
    public class UyeService
    {
        DatabaseIONICEntities db = new DatabaseIONICEntities();

        public KullaniciModel UyeOturumAc(string kadi, string parola)
        {
            KullaniciModel uye = db.Kullanici.Where(s => s.kullaniciAdi == kadi && s.sifre == parola).Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                foto = x.foto,
                sifre = x.sifre,
                rol = x.rol

            }).SingleOrDefault();
            
            return uye;

        }
    }
}

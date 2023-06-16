using ionicFinalProje2.Models;
using ionicFinalProje2.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ionicFinalProje2.Controllers
{
    public class ServisController : ApiController
    {
        Database2ionicEntities db = new Database2ionicEntities();
        SonucModel sonuc = new SonucModel();

        #region Kullanici


        // tüm kullanıcıları listele
        [HttpGet]
        [Route("api/kullanicilistele")]
        public List<KullaniciModel> kullaniciListele()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                sifre = x.sifre,
                foto = x.foto,
                rol = x.rol

            }).ToList();

            return liste;
        }

        // ıd numarasına göre listele
        [HttpGet]
        [Route("api/kullanicibyid/{kullaniciId}")]
        public KullaniciModel kullaniciById(int kullaniciId)
        {
            KullaniciModel kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                sifre = x.sifre,
                foto = x.foto,
                rol = x.rol
            }).SingleOrDefault();

            return kayit;
        }

        // kullanıcı ekle
        [HttpPost]
        [Route("api/kullaniciekle")]
        public SonucModel kullaniciEkle(KullaniciModel model)
        {
            if (db.Kullanici.Count(s => s.email == model.email || s.kullaniciAdi == model.kullaniciAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girmiş olduğunuz e-posta ya da kullanıcı adı zaten kayıtlıdır !";
                return sonuc;
            }

            Kullanici yeni = new Kullanici();
            yeni.adSoyad = model.adSoyad;
            yeni.email = model.email;
            yeni.sifre = model.sifre;
            yeni.foto = model.foto;
            yeni.kullaniciAdi = model.kullaniciAdi;
            yeni.rol = model.rol;
            db.Kullanici.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Kaydı Başarıyla Yapıldı !";
            return sonuc;
        }


        // Kullanıcı bilgilerini güncelle
        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public SonucModel kullaniciDuzenle(KullaniciModel model)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == model.kullaniciId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            if (db.Kullanici.Count(s => s.email == model.email) > 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu mail adresi zaten bir üyeye ait.";
                return sonuc;
            }

            if (db.Kullanici.Count(s => s.kullaniciAdi == model.kullaniciAdi) > 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu kullanıcı adı zaten bir üyeye ait.";
                return sonuc;
            }

            kayit.adSoyad = model.adSoyad;
            kayit.email = model.email;
            kayit.sifre = model.sifre;
            kayit.rol = model.rol;
            kayit.foto = model.foto;
            kayit.kullaniciAdi = model.kullaniciAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Başarıyla Güncellendi !";
            return sonuc;
        }


        //Kullanıcının kaydını sil
        [HttpDelete]
        [Route("api/kullanicisil/{kullaniciId}")]
        public SonucModel kullaniciSil(int kullaniciId)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            List<BegeniModel> begeniler = begeniByKullanici(kullaniciId);
            foreach (var i in begeniler)
            {
                begeniSil(i.begeniId);
            }

            List<YorumModel> yorumlar = yorumListeleByKullanici(kullaniciId);
            foreach (var i in yorumlar)
            {
                begeniSil(i.yorumId);
            }

            List<GonderiModel> gonderiler = gonderiListeleByKullanici(kullaniciId);
            foreach (var i in gonderiler)
            {
                gonderiSil(i.gonderiId);
            }

            
            db.Kullanici.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcıya ait tüm yorumlar, gönderiler, beğeniler ve bilgiler silinmiştir.";

            return sonuc;
        }


        [HttpPost]
        [Route("api/kullanicifotoguncelle")]
        public SonucModel kullaniciFotoGuncelle(KullaniciFotoModel model)
        {
            Kullanici kullanici = db.Kullanici.Where(s => s.kullaniciId == model.kullaniciId).SingleOrDefault();
            if (kullanici == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            if (kullanici.foto != "profil.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + kullanici.foto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;

            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbytes = Convert.FromBase64String(base64);
            string dosyaAdi = kullanici.kullaniciId + model.fotoUzanti.Replace("image/",
                ".");

            using (var ms = new MemoryStream(imgbytes, 0, imgbytes.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));
            }

            kullanici.foto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf Güncellendi !";


            return sonuc;
        }

        #endregion

        #region Gonderi


        // tüm gönderileri listele
        [HttpGet]
        [Route("api/gonderilistele")]
        public List<GonderiModel> gonderiListele()
        {
            List<GonderiModel> liste = db.Gonderi.Select(x => new GonderiModel()
            {

                gonderiId = x.gonderiId,
                gonderiKullaniciId = x.gonderiKullaniciId,
                gonderiIcerik = x.gonderiIcerik,
                gonderiTarih = x.gonderiTarih,
                gonderiBegeniSayisi = db.Begeni.Count(s => s.begeniGonderiId == x.gonderiId),
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.gonderiKullaniciBilgi = kullaniciById(kayit.gonderiKullaniciId);
            }

            return liste;
        }


        // ıd numarasına göre gönderiyi döndür
        [HttpGet]
        [Route("api/gonderibygonderiId/{gonderId}")]
        public GonderiModel gonderiListeleById(int gonderiId)
        {
            GonderiModel kayit = db.Gonderi.Where(s => s.gonderiId == gonderiId).Select(x => new GonderiModel()
            {
                gonderiId = x.gonderiId,
                gonderiKullaniciId = x.gonderiKullaniciId,
                gonderiIcerik = x.gonderiIcerik,
                gonderiTarih = x.gonderiTarih,
                gonderiBegeniSayisi = db.Begeni.Count(s => s.begeniGonderiId == x.gonderiId),
            }).FirstOrDefault();

            if (kayit != null)
            {
                kayit.gonderiKullaniciBilgi = kullaniciById(kayit.gonderiKullaniciId);
            }

            return kayit;
        }


        // bir kullanıcıya ait gönderileri listeler
        [HttpGet]
        [Route("api/gonderibykullaniciid/{kullaniciId}")]
        public List<GonderiModel> gonderiListeleByKullanici(int kullaniciId)
        {
            List<GonderiModel> liste = db.Gonderi.Where(s => s.gonderiKullaniciId == kullaniciId).Select(x => new GonderiModel()
            {

                gonderiId = x.gonderiId,
                gonderiKullaniciId = x.gonderiKullaniciId,
                gonderiIcerik = x.gonderiIcerik,
                gonderiTarih = x.gonderiTarih,
                gonderiBegeniSayisi = db.Begeni.Count(s => s.begeniGonderiId == x.gonderiId),
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.gonderiKullaniciBilgi = kullaniciById(kayit.gonderiKullaniciId);
            }

            return liste;
        }

        // Yeni gönderi ekler.
        [HttpPost]
        [Route("api/gonderiekle")]
        public SonucModel gonderiEkle(GonderiModel model)
        {
            if (db.Gonderi.Count(s => s.gonderiIcerik == model.gonderiIcerik && s.gonderiKullaniciId == model.gonderiKullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Zaten Olan Bir Gönderi Tekrar Eklenemez !";
                return sonuc;
            }

            Gonderi yeni = new Gonderi();
            yeni.gonderiKullaniciId = model.gonderiKullaniciId;
            yeni.gonderiTarih = DateTime.Now;
            yeni.gonderiIcerik = model.gonderiIcerik;

            db.Gonderi.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Gönderi Başarıyla Eklendi !";

            return sonuc;
        }

        // Gönderi Düzenler
        [HttpPut]
        [Route("api/gonderiduzenle")]
        public SonucModel gonderiDuzenle(GonderiModel model)
        {
            Gonderi kayit = db.Gonderi.Where(s => s.gonderiId == model.gonderiId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Gönderi Bulunamadı !";
                return sonuc;
            }

            kayit.gonderiIcerik = model.gonderiIcerik;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Başarıyla Düzenlendi !";

            return sonuc;
        }


        // Gönderiyi siler
        [HttpDelete]
        [Route("api/gonderisil/{gonderiId}")]
        public SonucModel gonderiSil(int gonderiId)
        {
            Gonderi kayit = db.Gonderi.Where(s => s.gonderiId == gonderiId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Gönderi Bulunamadı !";
                return sonuc;
            }

            List<BegeniModel> begeniler = db.Begeni.Where(s => s.begeniGonderiId == gonderiId).Select(x => new BegeniModel()
            {
                begeniId = x.begeniId,
                begeniGonderiId = x.begeniGonderiId,
                begeniKullaniciAdi = x.Kullanici.adSoyad,
                begeniKullaniciId = x.begeniKullaniciId,
            }).ToList();

            foreach (var begenim in begeniler)
            {
                begeniSil(begenim.begeniId);
            }

            List<YorumModel> yorumlar = db.Yorum.Where(s => s.yorumGonderiId == gonderiId).Select(x => new YorumModel()
            {

                yorumId = x.yorumId,
                yorumGonderiId = x.yorumGonderiId,
                yorumIcerik = x.yorumIcerik,
                yorumKullaniciId = x.yorumKullaniciId,
                yorumTarih = x.yorumTarih

            }).ToList();

            foreach (var yorumum in yorumlar)
            {
                yorumSil(yorumum.yorumId);
            }

            db.Gonderi.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Gönderi Başarıyla Silindi !";
            return sonuc;
        }

        #endregion

        #region Yorum

        // Tüm yorumları listeler.
        [HttpGet]
        [Route("api/yorumlistele")]
        public List<YorumModel> yorumListele()
        {
            List<YorumModel> liste = db.Yorum.Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumGonderiId = x.yorumGonderiId,
                yorumKullaniciId = x.yorumKullaniciId,
                yorumIcerik = x.yorumIcerik,
                yorumTarih = x.yorumTarih

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumKullaniciBilgi = kullaniciById(kayit.yorumKullaniciId);
            }

            return liste;
        }

        // ıd numarasına göre listeler.
        [HttpGet]
        [Route("api/yorumlistelebyid/{yorumId}")]
        public YorumModel yorumListeleById(int yorumId)
        {
            YorumModel kayit = db.Yorum.Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumGonderiId = x.yorumGonderiId,
                yorumKullaniciId = x.yorumKullaniciId,
                yorumIcerik = x.yorumIcerik,
                yorumTarih = x.yorumTarih

            }).SingleOrDefault();

            if (kayit != null)
            {
                kayit.yorumKullaniciBilgi = kullaniciById(kayit.yorumKullaniciId);
            }

            return kayit;
        }

        // bir kullanıcıya ait yorumları getirir.
        [HttpGet]
        [Route("api/yorumlistelebykullanici/{kullaniciId}")]
        public List<YorumModel> yorumListeleByKullanici(int kullaniciId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.yorumKullaniciId == kullaniciId).Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumGonderiId = x.yorumGonderiId,
                yorumKullaniciId = x.yorumKullaniciId,
                yorumIcerik = x.yorumIcerik,
                yorumTarih = x.yorumTarih

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumKullaniciBilgi = kullaniciById(kayit.yorumKullaniciId);
            }

            return liste;
        }

        // bir gönderiye ait yorumları getirir.
        [HttpGet]
        [Route("api/yorumlistelebygonderi/{gonderiId}")]
        public List<YorumModel> yorumListeleByGonderi(int gonderiId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.yorumGonderiId == gonderiId).Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumGonderiId = x.yorumGonderiId,
                yorumKullaniciId = x.yorumKullaniciId,
                yorumIcerik = x.yorumIcerik,
                yorumTarih = x.yorumTarih

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumKullaniciBilgi = kullaniciById(kayit.yorumKullaniciId);
            }

            return liste;
        }

        // Yorum Ekler.
        [HttpPost]
        [Route("api/yorumekle")]
        public SonucModel yorumEkle(YorumModel model)
        {
            if (db.Yorum.Count(s => s.yorumIcerik == model.yorumIcerik && s.yorumGonderiId == model.yorumGonderiId && s.yorumKullaniciId == model.yorumKullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu yorum zaten bu kullanıcı tarafından bu gönderiye daha önce yapılmış !";
                return sonuc;
            }

            Yorum yeni = new Yorum();
            yeni.yorumKullaniciId = model.yorumKullaniciId;
            yeni.yorumGonderiId = model.yorumGonderiId;
            yeni.yorumIcerik = model.yorumIcerik;
            yeni.yorumTarih = model.yorumTarih;
            db.Yorum.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Eklendi !";
            return sonuc;
        }

        // Yorumu siler.
        [HttpDelete]
        [Route("api/yorumsil/{yorumId}")]
        public SonucModel yorumSil(int yorumId)
        {
            Yorum kayit = db.Yorum.Where(s => s.yorumId == yorumId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            db.Yorum.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Silindi !";
            return sonuc;
        }

        #endregion

        #region Begeni

        // bir gönderinin tüm beğenilerini döndürür.
        [HttpGet]
        [Route("api/begenilistelebygonderi/{gonderiId}")]
        public List<BegeniModel> begeniByGonderi(int gonderiId)
        {
            List<BegeniModel> liste = db.Begeni.Select(x => new BegeniModel()
            {

                begeniId = x.begeniId,
                begeniGonderiId = x.begeniGonderiId,
                begeniKullaniciId = x.begeniKullaniciId,
                begeniKullaniciAdi = x.Kullanici.adSoyad


            }).ToList();

            return liste;
        }

        // kullanıcıya ait beğenileri listeler.
        [HttpGet]
        [Route("api/begenilistelebykullanici/{kullaniciId}")]
        public List<BegeniModel> begeniByKullanici(int kullaniciId)
        {
            List<BegeniModel> liste = db.Begeni.Select(x => new BegeniModel()
            {

                begeniId = x.begeniId,
                begeniGonderiId = x.begeniGonderiId,
                begeniKullaniciId = x.begeniKullaniciId,
                begeniKullaniciAdi = x.Kullanici.adSoyad


            }).ToList();

            return liste;
        }


        // Gönderiye bir beğeni ekler (kullanıcının bir gönderiyi beğenmesi)
        [HttpPost]
        [Route("api/begeniEkle")]
        public SonucModel begeniEkle(BegeniModel model)
        {
            if (db.Begeni.Count(s => s.begeniGonderiId == model.begeniGonderiId && s.begeniKullaniciId == model.begeniKullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu kullanıcı bu gönderiyi zaten beğenmiş !";
                return sonuc;
            }

            Begeni yeni = new Begeni();
            yeni.begeniKullaniciId = model.begeniKullaniciId;
            yeni.begeniGonderiId = model.begeniGonderiId;
            db.Begeni.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Beğeni Yapıldı !";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/begenisil/{begeniId}")]
        public SonucModel begeniSil(int begeniId)
        {
            Begeni kayit = db.Begeni.Where(s => s.begeniId == begeniId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Beğeni bulunamadı !";
                return sonuc;
            }

            db.Begeni.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Beğeni silindi !";
            return sonuc;
        }

        #endregion
    }
}

import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AlertController, ModalController } from '@ionic/angular';
import { Gonderi } from 'src/app/models/Gonderi';
import { Kullanici } from 'src/app/models/Kullanici';
import { KullaniciFoto } from 'src/app/models/KullaniciFoto';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-kullanicilar',
  templateUrl: './kullanicilar.component.html',
  styleUrls: ['./kullanicilar.component.css']
})
export class KullanicilarComponent implements OnInit {
  secilenFoto!:any;
  kullaniciFoto:KullaniciFoto = new KullaniciFoto();
  aktifKullanici!:Kullanici;
  seciliKullanici!:Kullanici;
  gonderiler!:Gonderi[];
  kullanicilar!:Kullanici[];

  kullaniciDuzenleme: FormGroup = new FormGroup({
    kullaniciAdi: new FormControl(),
    email: new FormControl(),
    sifre: new FormControl(),
    adSoyad: new FormControl(),
    rol: new FormControl(),

  });

  kullaniciEkleme: FormGroup = new FormGroup({
    kullaniciAdi: new FormControl(),
    email: new FormControl(),
    sifre: new FormControl(),
    adSoyad: new FormControl(),
    rol: new FormControl(),
  });


  
  constructor(
    public servis:ApiService,
    public modalcik:ModalController,
    public alert:AlertController
  ) { }

  ngOnInit() {
    this.kullaniciListele();
    this.gonderiListele();
  }

  kullaniciListele(){
    this.servis.kullaniciListele().subscribe((d:any)=> {
      this.kullanicilar = d;
    })
  }

  async presentAlert(mesaj:string) {
    const alert = await this.alert.create({
      header: '',
      message: mesaj,
      buttons: ['Tamam']
    });
  
    await alert.present();
  }

  kullaniciDuzenle() {
      this.seciliKullanici.kullaniciAdi = this.kullaniciDuzenleme.value.kullaniciAdi;
      this.seciliKullanici.email = this.kullaniciDuzenleme.value.email;
      this.seciliKullanici.sifre = this.kullaniciDuzenleme.value.sifre;
      this.seciliKullanici.adSoyad = this.kullaniciDuzenleme.value.adSoyad;
      this.seciliKullanici.rol = this.kullaniciDuzenleme.value.rol;
     
      this.servis.kullaniciDuzenle(this.seciliKullanici).subscribe((d:any) => {
        this.presentAlert(d.mesaj)
        this.modalcik.dismiss();
        this.kullaniciListele();
      }
    )
  }

  duzenleModalAyarla(kullanici:Kullanici){
    this.kullaniciDuzenleme.reset();
    this.kullaniciDuzenleme.patchValue(kullanici);
    this.seciliKullanici = kullanici;
  }

  aktifKullaniciAl(){
    if(this.servis.oturumKontrol()){
      var kullaniciId = parseInt(localStorage.getItem("kullaniciId") || "0");
      this.servis.kullaniciById(kullaniciId).subscribe((d:any)=> {
        this.aktifKullanici = d;
      })
    }
    }

    gonderiListele(){
      this.servis.gonderiListele().subscribe((d:any) => {
        this.gonderiler = d;
      })
    }

    kullaniciSil(kullanici:Kullanici){
      this.seciliKullanici = kullanici;
    }

    kullaniciSilKaydet(){
      this.servis.kullaniciSil(this.seciliKullanici.kullaniciId).subscribe((d:any) => {
        this.presentAlert(d.mesaj);
        this.kullaniciListele();
      })
    }

    kullaniciEkle() {
      var yenikullanici : Kullanici =  new Kullanici();
      yenikullanici.adSoyad = this.kullaniciEkleme.value.adSoyad;
      yenikullanici.kullaniciAdi = this.kullaniciEkleme.value.kullaniciAdi;
      yenikullanici.email = this.kullaniciEkleme.value.email;
      yenikullanici.sifre = this.kullaniciEkleme.value.sifre;
      yenikullanici.rol = this.kullaniciEkleme.value.rol;
      yenikullanici.foto = "profil.jpg";

      this.servis.kullaniciEkle(yenikullanici).subscribe((d:any) => {
        this.presentAlert(d.mesaj);
        this.kullaniciListele();
        this.modalcik.dismiss();
      })
        
    }

    fotoSec(e:any,kullanici:Kullanici){
      var fotolar = e.target.files;
      var foto = fotolar[0];
  
      var fr = new FileReader();
      fr.onloadend=()=>{
        this.secilenFoto = fr.result;
        if (fr.result !== null) {
          this.kullaniciFoto.fotoData = fr.result.toString();
        }
        this.kullaniciFoto.fotoUzanti = foto.type;
      };
      fr.readAsDataURL(foto);
      this.kullaniciFoto.kullaniciId = kullanici.kullaniciId;
      var eklenecekFoto: KullaniciFoto = new KullaniciFoto();
      eklenecekFoto.fotoData = this.kullaniciFoto.fotoData;
      eklenecekFoto.fotoUzanti = this.kullaniciFoto.fotoUzanti;
      eklenecekFoto.kullaniciId = kullanici.kullaniciId
      this.servis.kullaniciFotoGuncelle(kullanici.kullaniciId,this.kullaniciFoto.fotoData,this.kullaniciFoto.fotoUzanti).subscribe(d=> {
        this.kullaniciListele();
      })

    }
  }



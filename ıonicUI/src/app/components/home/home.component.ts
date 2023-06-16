import { ApiService } from './../../services/api.service';
import { Sonuc } from './../../models/Sonuc';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertController, IonModal } from '@ionic/angular';
import { Gonderi } from 'src/app/models/Gonderi';
import { Kullanici } from 'src/app/models/Kullanici';
import { Yorum } from 'src/app/models/Yorum';
import { Begeni } from 'src/app/models/Begeni';


@Component({  
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent  implements OnInit {
  kullanicilar!:Kullanici[];
  yorumlar!:Yorum[];
  aktifKullanici!:Kullanici;
  gonderiler!:Gonderi[];
  seciligGonderiId!:number;
  @ViewChild(IonModal) modal!: IonModal;
  constructor(

    public apiServis:ApiService,
    public alert:AlertController
  ) { }

  ngOnInit() {
    this.kullaniciListele();
    this.gonderiListele();
    this.aktifKullaniciAl();
  }
  
  Sonuc = new Sonuc();
  begeni: Begeni = new Begeni();
  
  // modal 
  cancel() {
    this.modal.dismiss(null, 'kapat');
  }



  alertVer(sonuc:Sonuc){
    this.presentAlert(sonuc.mesaj)
  }
  async presentAlert(mesaj:string) {
    const alert = await this.alert.create({
      header: '',
      message: mesaj,
      buttons: ['Tamam']
    });
  
    await alert.present();
  }

  kullaniciListele() {
    this.apiServis.kullaniciListele().subscribe((d:any) => {
      this.kullanicilar = (d);
      console.log(this.kullanicilar);
    })
  }

  gonderiListele(){
    this.apiServis.gonderiListele().subscribe((d:any) => {
      this.gonderiler = (d);
      console.log(d);
    })
  }

  gonderiYorumlariListele(gonderiId:any){
    this.seciligGonderiId = gonderiId;
    this.apiServis.yorumByGonderiId(gonderiId).subscribe((d: Yorum[]) =>{
      this.yorumlar = d;
      console.log(d);
    })
  }

  begeniEkle(gonderiId:number){
    this.begeni.begeniGonderiId = gonderiId;
    this.begeni.begeniKullaniciId = this.aktifKullanici.kullaniciId;
    this.apiServis.begeniEkle(this.begeni).subscribe((d: any) => {
      this.gonderiListele();
      this.presentAlert(d.mesaj);
    });

  }

  gonderiEkle(gonderi:any){
    var Ekgonderi : Gonderi = new Gonderi();
    Ekgonderi.gonderiIcerik = gonderi;
    Ekgonderi.gonderiKullaniciId = this.aktifKullanici.kullaniciId;
    Ekgonderi.gonderiBegeniSayisi = 0;
    this.apiServis.gonderiEkle(Ekgonderi).subscribe((d:any) => {
      this.gonderiListele();
      this.presentAlert(d.mesaj);
    }
    )

    
  }

  aktifKullaniciAl(){
    if(this.apiServis.oturumKontrol()){
      var kullaniciId = parseInt(localStorage.getItem("kullaniciId") || "0");
      this.apiServis.kullaniciById(kullaniciId).subscribe((d:any)=> {
        this.aktifKullanici = d;
      })
    }
    }

    yorumEkle(yorum:any){
      var eklenecek: Yorum = new Yorum();
      eklenecek.yorumIcerik = yorum;
      eklenecek.yorumGonderiId = this.seciligGonderiId;
      eklenecek.yorumKullaniciId = this.aktifKullanici.kullaniciId,
      
      this.apiServis.yorumEkle(eklenecek).subscribe((d:any) =>{
        this.presentAlert(d.mesaj);
        this.gonderiListele();
      })
    }
}

import { Component, OnInit } from '@angular/core';
import { AlertController } from '@ionic/angular';
import { Gonderi } from 'src/app/models/Gonderi';
import { Kullanici } from 'src/app/models/Kullanici';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrls: ['./profil.component.scss'],
})
export class ProfilComponent  implements OnInit {
  gonderiler!:Gonderi[];
  aktifKullanici!:Kullanici;
  constructor(
    public servis:ApiService,
    public alert:AlertController
  ) { }

  ngOnInit() {
    this.aktifKullaniciAl();
    setTimeout(() => {
      this.kullaniciGonderiListele();
    }, 100);
    
  }
  async presentAlert(mesaj:string) {
    const alert = await this.alert.create({
      header: '',
      message: mesaj,
      buttons: ['Tamam']
    });
  
    await alert.present();
  }
  kullaniciGonderiListele(){
    this.servis.gonderiByKullaniciId(this.aktifKullanici.kullaniciId).subscribe((d:any) => {
      this.gonderiler = d;
    })
    console.log(this.gonderiler)
  }

  aktifKullaniciAl(){
    if(this.servis.oturumKontrol()){
      var kullaniciId = parseInt(localStorage.getItem("kullaniciId") || "0");
      this.servis.kullaniciById(kullaniciId).subscribe((d:any)=> {
        this.aktifKullanici = d;
      })
    }
    }

    gonderiSil(gonderiId:number){
      this.servis.gonderiSil(gonderiId).subscribe((d:any) => {
        this.presentAlert(d.mesaj);
        this.kullaniciGonderiListele();
      })
    }
}

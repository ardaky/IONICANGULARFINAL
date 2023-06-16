import { Component, OnInit } from '@angular/core';
import { AlertController } from '@ionic/angular';
import { Sonuc } from 'src/app/models/Sonuc';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent  implements OnInit {
  kadi!: string;
  parola!:string;
  constructor(
    public servis:ApiService,
    public alert:AlertController
  ) { }

  ngOnInit() {}
  
  async presentAlert(mesaj:string) {
    const alert = await this.alert.create({
      header: '',
      message: mesaj,
      buttons: ['Tamam']
    });
  
    await alert.present();
  }
  
  
  oturumAc(kAdi: any, parola: any) {
    this.servis.tokenAl(kAdi, parola).subscribe({
      next: (d: any) => {
        console.log(d);
        localStorage.setItem("token", d.access_token);
        localStorage.setItem("kullaniciId", d.uyeId);
        localStorage.setItem("kullaniciAdi", d.uyeKadi);
        localStorage.setItem("uyeYetkileri", d.uyeYetkileri);
        location.href = "/";
      },
      error: (err: any) => {
        var s: Sonuc = new Sonuc();
        s.islem = false;
        s.mesaj = "Hatalı kullanıcı adı ya da parola";
        this.presentAlert(s.mesaj)
      }
    });
  }
}

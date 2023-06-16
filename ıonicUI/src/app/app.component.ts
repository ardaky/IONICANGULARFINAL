import { Component } from '@angular/core';
import { ApiService } from './services/api.service';
import { Kullanici } from './models/Kullanici';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  
  aktifKullanici!:Kullanici;
  constructor(
    public servis:ApiService
  ) {}
  
  ngOnInit() {
    this.aktifKullaniciAl();
  }

  oturumKapat(){
    localStorage.clear();
    location.href = "/";
  }
  
  aktifKullaniciAl(){
    if(this.servis.oturumKontrol()){
      var kullaniciId = parseInt(localStorage.getItem("kullaniciId") || "0");
      this.servis.kullaniciById(kullaniciId).subscribe((d:any)=> {
        this.aktifKullanici = d;
      })
    }
    }


}

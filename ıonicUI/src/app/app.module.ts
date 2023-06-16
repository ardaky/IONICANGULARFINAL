import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './components/home/home.component';
import { ProfilComponent } from './components/profil/profil.component';
import { LoginComponent } from './components/login/login.component';
import { HotToastModule } from '@ngneat/hot-toast';
import { ApiService } from './services/api.service';
import { KullanicilarComponent } from './components/kullanicilar/kullanicilar.component';
@NgModule({
  declarations: [AppComponent, HomeComponent, ProfilComponent, LoginComponent,KullanicilarComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule,HotToastModule.forRoot(),AppRoutingModule,HttpClientModule,FormsModule,ReactiveFormsModule,],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy },ApiService],
  bootstrap: [AppComponent],
})
export class AppModule {}

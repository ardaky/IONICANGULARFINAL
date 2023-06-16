import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProfilComponent } from './components/profil/profil.component';
import { LoginComponent } from './components/login/login.component';
import { KullanicilarComponent } from './components/kullanicilar/kullanicilar.component';

const routes: Routes = [
{
  path:'',
  component:HomeComponent
},
{
  path:'home',
  component:HomeComponent
},
{
  path:'profil',
  component:ProfilComponent
},
{
  path:'login',
  component:LoginComponent
},
{
  path:'kullanicilar',
  component:KullanicilarComponent
}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

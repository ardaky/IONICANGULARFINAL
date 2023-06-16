import { Kullanici } from "./Kullanici";

export class Yorum {
    yorumId!:number;
    yorumGonderiId!:number;
    yorumKullaniciId!:number;
    yorumIcerik!:string;
    yorumTarih!:string;
    yorumKullaniciBilgi!:Kullanici;
}

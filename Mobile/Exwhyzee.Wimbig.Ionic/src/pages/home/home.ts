import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { HomeService } from '../../services/home-service';
import { Client, PagedListOfRaffleDto, MapImageToRaffle, IImageOfARaffle, ImageOfARaffle } from '../../services/service-proxies/service-proxies';

@IonicPage()
@Component({
  selector: 'page-home',
  templateUrl: 'home.html',
  providers: [HomeService, Client]

})
export class HomePage {
  toolbarTitle: string;
  data: any = {};
  raffles: any;

  constructor(public navCtrl: NavController, public service: HomeService, public wimService: Client) {
    this.toolbarTitle = "WIMBIG";
    service.load().subscribe(snapshot => {
      this.data = snapshot;
      console.log(snapshot);
    });

   
    this.wimService.getAllRaflles(1, undefined, undefined, 0, 30, undefined)
      .subscribe((result: PagedListOfRaffleDto) => {

        this.raffles = result.source;
        
      })
  }
}

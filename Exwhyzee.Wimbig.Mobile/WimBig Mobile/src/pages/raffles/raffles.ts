import {Component} from "@angular/core";
import {NavController, List} from "ionic-angular";
import {TripDetailPage} from "../trip-detail/trip-detail";
import { RaffleService, RaffleDto, PagedListOfRaffleDto } from "../../services/generated-baseService";

@Component({
  selector: 'page-raffles',
  templateUrl: 'raffles.html'
})
export class RafflesPage {
  // list of trips
  public raffle: RaffleDto = new RaffleDto();
  public raffles: PagedListOfRaffleDto = new PagedListOfRaffleDto();

  constructor(public nav: NavController, public _raffleService: RaffleService) {
    // set sample data
    this.getRalles();
   
  }

  // view trip detail
  viewDetail(id) {
    this.nav.push(TripDetailPage, {id: id});
  }

  viewRaffle(id: any) {
    this._raffleService.getRaffleById(id)
      .subscribe((result: RaffleDto) => {
        this.raffle = result;
      }, (error) => {
        console.log(error);
      })
  }

  getRalles(): void{
    this._raffleService.getAllRaflles(null,null,null,null,null,null)
      .subscribe((result: any) => {
        this.raffles = result;
        console.log(result);
      }, (error) => {
        console.log(error)
      })
  }
}

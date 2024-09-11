import { Component, Input, AfterViewInit } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'spinner',
    templateUrl: 'spinner.html'
})
export class Spinner implements AfterViewInit {
  @Input('data') data: any;
  path : string;

  constructor() {}

  ngAfterViewInit() {
    this.path = "assets/svg/" + this.data.icon + ".svg";
  }

  getData = ():any => {
    return this.data;
  }
}

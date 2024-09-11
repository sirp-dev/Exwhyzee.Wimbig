import { Component, Input, OnChanges } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'raffles-layout',
  templateUrl: 'raffles.html'
})
export class RafflesLayout implements OnChanges {
  @Input('data') data: any;
  @Input('events') events: any;


  constructor() { }

  ngOnChanges(changes: { [propKey: string]: any }) {
    this.data = changes['data'].currentValue;
  }

  onEvent = (event: string, item: any, e: any): void => {
    if (e) {
      e.stopPropagation();
    }
    if (this.events[event]) {
      this.events[event](item);
    }
  }
}

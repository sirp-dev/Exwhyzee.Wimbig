import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'select-layout-3',
    templateUrl: 'select.html'
})
export class SelectLayout3 {
  @Input('data') data: any;
  @Input('events') events: any;

  constructor() {}

  onEvent = (event: string, item: any): void => {
    if (this.events[event]) {
        this.events[event](item);
    }
  }
}

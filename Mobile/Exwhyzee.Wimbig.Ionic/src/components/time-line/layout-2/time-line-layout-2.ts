import { Component, Input, OnChanges } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'time-line-layout-2',
    templateUrl: 'time-line.html'
})
export class TimeLineLayout2 implements OnChanges {
    @Input('data') data: any;
    @Input('events') events: any;

    constructor() { }

    ngOnChanges(changes: { [propKey: string]: any }) {
      this.data = changes['data'].currentValue;
    }

    onEvent = (event: string, item: any): void => {
        if (this.events[event]) {
            this.events[event](item);
        }
    }
}

import { Component, Input, OnChanges } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'segment-layout-3',
    templateUrl: 'segment.html'
})
export class SegmentLayout3 implements OnChanges {
    @Input('data') data: any;
    @Input('events') events: any;

    public selectedItem = "Page1";

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

    isEnabled(value:string): boolean {
       return this.selectedItem == value;
    }
}

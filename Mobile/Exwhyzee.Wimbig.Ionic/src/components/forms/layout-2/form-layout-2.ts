import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'form-layout-2',
    templateUrl: 'form.html'
})
export class FormLayout2 {
    @Input() data: any;
    @Input() events: any;

    description:String;

    constructor() { }

    onEvent(event: string, e: any) {
        if (e) {
            e.stopPropagation();
        }
        if (this.events[event]) {
            this.events[event](this.getItemData());
            this.resetValue();
        }
    }

    getItemData() {
        return {
            'description': this.description
        };
    }

    resetValue() {
        this.description = "";
    }

    onStarClass(items: any, index: number, e: any) {
        for (var i = 0; i < items.length; i++) {
          items[i].isActive = i <= index;
        }
        if (this.events['onRates']) {
            this.events['onRates'](index);
        }
    };
}

import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'form-layout-3',
    templateUrl: 'form.html'
})
export class FormLayout3 {
    @Input() data: any;
    @Input() events: any;

    firstName:String;
    lastName:String;
    address1:String;
    address2:String;
    city:String;
    zipCode:String;

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
            'firstName': this.firstName,
            'lastName':this.lastName,
            'address1':this.address1,
            'address2':this.address2,
            'city':this.city,
            'zipCode':this.zipCode

        };
    }

    resetValue() {
        this.firstName = "";
        this.lastName = "";
        this.address1 = "";
        this.address2 = "";
        this.city = "";
        this.zipCode = "";

    }
}

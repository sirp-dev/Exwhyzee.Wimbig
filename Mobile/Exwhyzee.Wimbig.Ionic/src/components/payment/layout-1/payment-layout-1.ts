import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'payment-layout-1',
    templateUrl: 'payment.html'
})
export class PaymentLayout1 {
    @Input() data: any;
    @Input() events: any;

    cardItem = {}

    constructor() { }

    onPay() {
        if (this.events["onPay"]) {
            this.events["onPay"](this.cardItem);
        }
    }
}

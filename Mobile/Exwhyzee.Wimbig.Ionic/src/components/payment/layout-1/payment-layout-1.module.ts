import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { PaymentLayout1 } from './payment-layout-1';

@NgModule({
    declarations: [
        PaymentLayout1,
    ],
    imports: [
        IonicPageModule.forChild(PaymentLayout1),
    ],
    exports: [
        PaymentLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class PaymentLayout1Module { }

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPagePayment } from './item-details-payment';
import { PaymentLayout1Module } from '../../components/payment/layout-1/payment-layout-1.module';

@NgModule({
  declarations: [
    ItemDetailsPagePayment,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPagePayment),
    PaymentLayout1Module
  ],
  exports: [
    ItemDetailsPagePayment
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPagePaymentModule {}

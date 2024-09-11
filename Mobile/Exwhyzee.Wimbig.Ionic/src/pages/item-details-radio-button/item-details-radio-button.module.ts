import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageRadioButton } from './item-details-radio-button';

import { RadioButtonLayout1Module } from '../../components/radio-button/layout-1/radio-button-layout-1.module';
import { RadioButtonLayout2Module } from '../../components/radio-button/layout-2/radio-button-layout-2.module';
import { RadioButtonLayout3Module } from '../../components/radio-button/layout-3/radio-button-layout-3.module';

@NgModule({
  declarations: [
    ItemDetailsPageRadioButton,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageRadioButton),
    RadioButtonLayout1Module, RadioButtonLayout2Module,
    RadioButtonLayout3Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageRadioButtonModule {}

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { RafflesLayout } from './raffles-layout';

@NgModule({
  declarations: [
    RafflesLayout,
  ],
  imports: [
    IonicPageModule.forChild(RafflesLayout),
  ],
  exports: [
    RafflesLayout
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class RafflesLayoutModule { }

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { HomePage } from './home';
import { RafflesLayoutModule } from '../../components/raffles/raffles-layout.module';

@NgModule({
    declarations: [
        HomePage,
    ],
  imports: [
    IonicPageModule.forChild(HomePage), RafflesLayoutModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class HomePageModule { }

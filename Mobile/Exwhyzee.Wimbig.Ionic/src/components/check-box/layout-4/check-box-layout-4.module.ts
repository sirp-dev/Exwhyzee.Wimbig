import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CheckBoxLayout4 } from './check-box-layout-4';

@NgModule({
    declarations: [
        CheckBoxLayout4,
    ],
    imports: [
        IonicPageModule.forChild(CheckBoxLayout4),
    ],
    exports: [
        CheckBoxLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]

})

export class CheckBoxLayout4Module { }

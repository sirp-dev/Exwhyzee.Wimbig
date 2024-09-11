import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FormLayout3 } from './form-layout-3';

@NgModule({
    declarations: [
        FormLayout3,
    ],
    imports: [
        IonicPageModule.forChild(FormLayout3),
    ],
    exports: [
        FormLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class FormLayout3Module { }

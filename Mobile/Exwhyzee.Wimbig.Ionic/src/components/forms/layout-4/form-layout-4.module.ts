import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FormLayout4 } from './form-layout-4';

@NgModule({
    declarations: [
        FormLayout4,
    ],
    imports: [
        IonicPageModule.forChild(FormLayout4),
    ],
    exports: [
        FormLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class FormLayout4Module { }

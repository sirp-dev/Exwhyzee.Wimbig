import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FormLayout1 } from './form-layout-1';

@NgModule({
    declarations: [
        FormLayout1,
    ],
    imports: [
        IonicPageModule.forChild(FormLayout1),
    ],
    exports: [
        FormLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class FormLayout1Module { }

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FormLayout2 } from './form-layout-2';

@NgModule({
    declarations: [
        FormLayout2,
    ],
    imports: [
        IonicPageModule.forChild(FormLayout2),
    ],
    exports: [
        FormLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class FormLayout2Module { }

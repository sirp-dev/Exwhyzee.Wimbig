import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProfileLayout4 } from './profile-layout-4';

@NgModule({
    declarations: [
        ProfileLayout4,
    ],
    imports: [
        IonicPageModule.forChild(ProfileLayout4),
    ],
    exports: [
        ProfileLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ProfileLayout4Module { }

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProfileLayout3 } from './profile-layout-3';

@NgModule({
    declarations: [
        ProfileLayout3,
    ],
    imports: [
        IonicPageModule.forChild(ProfileLayout3),
    ],
    exports: [
        ProfileLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ProfileLayout3Module { }

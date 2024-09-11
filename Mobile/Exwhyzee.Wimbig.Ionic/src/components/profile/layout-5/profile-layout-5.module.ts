import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProfileLayout5 } from './profile-layout-5';

@NgModule({
    declarations: [
        ProfileLayout5,
    ],
    imports: [
        IonicPageModule.forChild(ProfileLayout5),
    ],
    exports: [
        ProfileLayout5
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ProfileLayout5Module { }

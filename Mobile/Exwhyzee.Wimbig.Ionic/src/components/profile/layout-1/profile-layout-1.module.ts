import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProfileLayout1 } from './profile-layout-1';

@NgModule({
    declarations: [
        ProfileLayout1,
    ],
    imports: [
        IonicPageModule.forChild(ProfileLayout1),
    ],
    exports: [
        ProfileLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ProfileLayout1Module { }

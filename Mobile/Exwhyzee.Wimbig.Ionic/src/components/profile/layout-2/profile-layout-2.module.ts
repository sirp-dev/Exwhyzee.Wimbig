import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProfileLayout2 } from './profile-layout-2';

@NgModule({
    declarations: [
        ProfileLayout2,
    ],
    imports: [
        IonicPageModule.forChild(ProfileLayout2),
    ],
    exports: [
        ProfileLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ProfileLayout2Module { }

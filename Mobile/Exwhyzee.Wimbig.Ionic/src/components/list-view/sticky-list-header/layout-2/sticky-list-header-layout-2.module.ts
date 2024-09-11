import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StickyListHeaderLayout2 } from './sticky-list-header-layout-2';
import { IonAffixModule } from "ion-affix";

@NgModule({
    declarations: [
        StickyListHeaderLayout2,
    ],
    imports: [
        IonicPageModule.forChild(StickyListHeaderLayout2),
        IonAffixModule
    ],
    exports: [
        StickyListHeaderLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class StickyListHeaderLayout2Module { }

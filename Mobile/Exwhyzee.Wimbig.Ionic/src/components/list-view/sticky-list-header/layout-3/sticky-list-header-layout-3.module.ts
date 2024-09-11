import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StickyListHeaderLayout3 } from './sticky-list-header-layout-3';
import { IonAffixModule } from "ion-affix";

@NgModule({
    declarations: [
        StickyListHeaderLayout3,
    ],
    imports: [
        IonicPageModule.forChild(StickyListHeaderLayout3),
        IonAffixModule
    ],
    exports: [
        StickyListHeaderLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class StickyListHeaderLayout3Module { }

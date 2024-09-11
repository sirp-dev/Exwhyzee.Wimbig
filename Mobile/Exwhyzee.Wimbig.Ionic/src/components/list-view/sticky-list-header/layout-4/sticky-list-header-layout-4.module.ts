import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StickyListHeaderLayout4 } from './sticky-list-header-layout-4';
import { IonAffixModule } from "ion-affix";

@NgModule({
    declarations: [
        StickyListHeaderLayout4,
    ],
    imports: [
        IonicPageModule.forChild(StickyListHeaderLayout4),
        IonAffixModule
    ],
    exports: [
        StickyListHeaderLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class StickyListHeaderLayout4Module { }

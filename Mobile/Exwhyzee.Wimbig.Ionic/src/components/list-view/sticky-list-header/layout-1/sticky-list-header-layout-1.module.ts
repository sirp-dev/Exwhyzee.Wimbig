import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StickyListHeaderLayout1 } from './sticky-list-header-layout-1';
import { IonAffixModule } from "ion-affix";

@NgModule({
    declarations: [
        StickyListHeaderLayout1,
    ],
    imports: [
        IonicPageModule.forChild(StickyListHeaderLayout1),
        IonAffixModule
    ],
    exports: [
        StickyListHeaderLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
    
})

export class StickyListHeaderLayout1Module { }
